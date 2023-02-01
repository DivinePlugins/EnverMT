using Divine.Entity.Entities;
using Divine.Entity.Entities.Units;
using Divine.Extensions;
using Divine.Game;
using Divine.Projectile;
using Divine.Projectile.EventArgs;
using Divine.Projectile.Projectiles;
using Divine.Update;

namespace EMT.Farm
{
    public class EmtUnit : IDisposable
    {
        public readonly Unit unit;
        private readonly Dictionary<Unit, float> lastAttackedTime; // Attacked unit, last attacked GameTime
        private readonly Dictionary<Unit, SortedDictionary<float, float>> attackersForecastDamage; // Unit, <GameTime, Damage>        
        private readonly SortedDictionary<float, float> forecastHealth; // Forecast Health <Gametime, health>        
        private const float forecastDuration = 3f;
        private const float forecastQuant = 0.02f;

        public EmtUnit(Unit unit)
        {
            this.unit = unit;
            this.lastAttackedTime = new();
            this.attackersForecastDamage = new();
            this.forecastHealth = new();

            Entity.AnimationChanged += Entity_AnimationChanged;
            ProjectileManager.TrackingProjectileAdded += ProjectileManager_TrackingProjectileAdded;
            UpdateManager.CreateGameUpdate(100, this.RemoveIdleAttackersFromList);
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
            if (!e.Name.Contains("attack", StringComparison.OrdinalIgnoreCase)) return;
            if (e.Name.Contains("_idle")) return;
            if (e.Name.Contains("idle_anim")) return;
            if (!senderUnit.IsMelee) return;

            if (!senderUnit.IsDirectlyFacing(this.unit.Position)) return;

            this.AddAttackerToList(senderUnit);
        }
        private void ProjectileManager_TrackingProjectileAdded(Divine.Projectile.EventArgs.TrackingProjectileAddedEventArgs e)
        {
            if (!e.Projectile.IsAttack) return;
            if (e.Projectile.Target != this.unit) return;
            if (e.Projectile.Source is not Unit unit) return;

            this.AddAttackerToList(unit, e);
        }

        private void AddAttackerToList(Unit attacker, TrackingProjectileAddedEventArgs? e = null)
        {
            if (attacker == null) return;

            this.lastAttackedTime[attacker] = GameManager.GameTime;
            this.ForecastAttackerDamage(attacker, e);
        }

        private void ForecastAttackerDamage(Unit attacker, TrackingProjectileAddedEventArgs? e = null)
        {
            float autoAttackProjectileArriveTime = 0f;
            float attackPoint = attacker.AttackPoint();
            if (e != null && e.Projectile.IsValid)
            {
                autoAttackProjectileArriveTime = UnitExtensions.GetProjectileArrivalTime(attacker, this.unit, 0, attacker.ProjectileSpeed());
                attackPoint = 0f;
            }
            this.attackersForecastDamage.Clear();
            this.attackersForecastDamage.Add(attacker, new SortedDictionary<float, float>());

            float firstHitTime = autoAttackProjectileArriveTime + attackPoint;
            this.attackersForecastDamage[attacker].Add(GameManager.GameTime + firstHitTime, attacker.GetAttackDamage(this.unit));

            float time = firstHitTime;
            while (time <= forecastDuration)
            {
                time += 1 / attacker.AttacksPerSecond;
                this.attackersForecastDamage[attacker].Add(time + GameManager.GameTime, attacker.GetAttackDamage(this.unit));
            }
        }

        private void RemoveIdleAttackersFromList()
        {
            foreach (KeyValuePair<Unit, float> attacker in this.lastAttackedTime)
            {
                float idleTime = 1 / attacker.Key.AttacksPerSecond + 0.5f;

                if ((GameManager.GameTime - attacker.Value) > idleTime)
                {
                    this.lastAttackedTime.Remove(attacker.Key);
                    this.attackersForecastDamage.Remove(attacker.Key);
                    this.CalculateForecastHealth();
                }
            }
        }

        private void CalculateForecastHealth()
        {
            SortedDictionary<float, float> forecastCumulativeDamage = new(); // <GameTime, cumulativeDamage>
            foreach (KeyValuePair<Unit, SortedDictionary<float, float>> attacker in this.attackersForecastDamage)
            {
                if (!attacker.Key.IsAlive) continue;

                foreach (KeyValuePair<float, float> timeDamage in this.attackersForecastDamage[attacker.Key])
                {
                    if (forecastCumulativeDamage.ContainsKey(timeDamage.Key))
                    {
                        forecastCumulativeDamage[timeDamage.Key] += timeDamage.Value;
                    }
                    else
                    {
                        forecastCumulativeDamage.Add(timeDamage.Key, timeDamage.Value);
                    }
                }
            }

            if (forecastCumulativeDamage.Count == 0) return;
            this.forecastHealth.Clear();
            this.forecastHealth.Add(GameManager.GameTime, this.unit.Health);
            float regeneratedHealth = 0;
            foreach (var timeDamage in forecastCumulativeDamage)
            {
                regeneratedHealth = (timeDamage.Key - GameManager.GameTime) * this.unit.HealthRegeneration;
                this.forecastHealth.Add(timeDamage.Key, this.unit.Health - timeDamage.Value + regeneratedHealth);
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
