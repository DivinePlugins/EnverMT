using Divine.Entity;
using Divine.Entity.Entities;
using Divine.Entity.Entities.Units;
using Divine.Extensions;
using Divine.Game;
using Divine.Projectile;
using Divine.Projectile.EventArgs;
using Divine.Update;

namespace EMT.Farm
{
    public class EmtUnit : IDisposable
    {
        public readonly Unit unit;
        private readonly Dictionary<uint, float> lastAttackedTime; // Attacked unit Handler, last attacked GameTime
        private readonly Dictionary<uint, List<float>> attackersForecastDamage; // Unit, List of Forecast Attack Time  
        private readonly SortedDictionary<float, float> forecastHealth; // Forecast Health <Gametime, health>        
        private const float forecastDuration = 3f;

        public EmtUnit(Unit unit)
        {
            this.unit = unit;
            this.lastAttackedTime = new();
            this.attackersForecastDamage = new();
            this.forecastHealth = new();

            Entity.AnimationChanged += Entity_AnimationChanged;
            ProjectileManager.TrackingProjectileAdded += ProjectileManager_TrackingProjectileAdded;
            UpdateManager.CreateIngameUpdate(this.RemoveIdleAttackersFromList);
        }
        public void Dispose()
        {
            Entity.AnimationChanged -= Entity_AnimationChanged;
            ProjectileManager.TrackingProjectileAdded -= ProjectileManager_TrackingProjectileAdded;
            UpdateManager.DestroyGameUpdate(this.RemoveIdleAttackersFromList);
        }
        private void Entity_AnimationChanged(Entity sender, Divine.Entity.Entities.EventArgs.AnimationChangedEventArgs e)
        {
            if (!this.unit.IsValid || sender == null) return;
            if (sender is not Unit senderUnit) return;
            if (senderUnit == this.unit) return;
            if (!this.unit.IsEnemy(senderUnit)) return;

            if (!e.Name.Contains("attack", StringComparison.OrdinalIgnoreCase)) return;
            if (e.Name.Contains("_idle")) return;
            if (e.Name.Contains("idle_anim")) return;

            if (!senderUnit.IsDirectlyFacing(this.unit.Position)) return;
            this.AddAttackerToList(senderUnit);
        }
        private void ProjectileManager_TrackingProjectileAdded(TrackingProjectileAddedEventArgs e)
        {
            if (!e.Projectile.IsAttack) return;
            if (e.Projectile.Target != this.unit) return;
            if (e.Projectile.Source is not Unit unit) return;
            
            this.AddAttackerToList(unit, e);
        }
        private void AddAttackerToList(Unit attacker, TrackingProjectileAddedEventArgs? e = null)
        {
            if (attacker == null) return;

            this.lastAttackedTime[attacker.Handle] = GameManager.GameTime;
            this.ForecastAttackerDamage(attacker, e);            
        }
        private void RemoveIdleAttackersFromList()
        {
            foreach (KeyValuePair<uint, float> attacker in this.lastAttackedTime)
            {
                Unit unit = (Unit)EntityManager.GetEntityByHandle(attacker.Key);
                if (unit == null) continue;
                float idleTime = 1 / unit.AttacksPerSecond + 0.5f;

                if ((GameManager.GameTime - attacker.Value) > idleTime)
                {
                    this.lastAttackedTime.Remove(unit.Handle);
                    this.attackersForecastDamage.Remove(unit.Handle);                    
                }
            }
        }

        private void ForecastAttackerDamage(Unit attacker, TrackingProjectileAddedEventArgs? e = null)
        {
            if (!this.attackersForecastDamage.ContainsKey(attacker.Handle)) this.attackersForecastDamage.Add(attacker.Handle, new());
            this.attackersForecastDamage[attacker.Handle].Clear();            

            float firstHitTime = GameManager.GameTime + UnitExtensions.GetAutoAttackArrivalTime(attacker, this.unit, true);

            if (e != null && e.Projectile.IsValid && attacker.IsRanged)
            {
                firstHitTime = GameManager.GameTime + UnitExtensions.GetProjectileArrivalTime(attacker, this.unit, 0, attacker.ProjectileSpeed(), false);
            }

            this.attackersForecastDamage[attacker.Handle].Add(firstHitTime);

            float time = firstHitTime;

            while (time <= GameManager.GameTime + forecastDuration)
            {
                time += (1 / attacker.AttacksPerSecond);
                this.attackersForecastDamage[attacker.Handle].Add(time);
            }
        }

        private void CalculateForecastHealth()
        {
            SortedDictionary<float, float> forecastCumulativeDamage = new(); // <GameTime, cumulativeDamage>

            foreach (KeyValuePair<uint, List<float>> attacker in this.attackersForecastDamage)
            {
                Entity? entity = EntityManager.GetEntityByHandle(attacker.Key);
                if (entity is not Unit unit) continue;

                if (!unit.IsAlive) continue;

                foreach (float time in this.attackersForecastDamage[attacker.Key])
                {
                    if (forecastCumulativeDamage.ContainsKey(time))
                    {
                        forecastCumulativeDamage[time] += unit.GetAttackDamage(this.unit);
                    }
                    else
                    {
                        forecastCumulativeDamage.Add(time, unit.GetAttackDamage(this.unit));
                    }
                }
            }

            if (forecastCumulativeDamage.Count == 0) return;
            this.forecastHealth.Clear();
            float totalDamage = 0;
            this.forecastHealth.Add(GameManager.GameTime, this.unit.Health);
            foreach (var timeDamage in forecastCumulativeDamage)
            {
                if (timeDamage.Key < GameManager.GameTime) continue;
                totalDamage += timeDamage.Value;                
                this.forecastHealth.Add(timeDamage.Key, this.unit.Health - totalDamage + (timeDamage.Key - GameManager.GameTime) * this.unit.HealthRegeneration);
                if (totalDamage > this.unit.Health) return;
            }
        }

        public SortedDictionary<float, float> GetForecastHealth
        {
            get
            {
                this.CalculateForecastHealth();
                return this.forecastHealth;
            }
        }
    }
}
