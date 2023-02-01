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
        private readonly Dictionary<uint, SortedDictionary<float, float>> attackersForecastDamage; // Unit, <GameTime, Damage>        
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
            UpdateManager.CreateGameUpdate(100, this.RemoveIdleAttackersFromList);
            Entity.NetworkPropertyChanged += Entity_NetworkPropertyChanged;
        }

        public void Dispose()
        {
            Entity.AnimationChanged -= Entity_AnimationChanged;
            ProjectileManager.TrackingProjectileAdded -= ProjectileManager_TrackingProjectileAdded;
            UpdateManager.DestroyGameUpdate(this.RemoveIdleAttackersFromList);
            Entity.NetworkPropertyChanged -= Entity_NetworkPropertyChanged;
        }

        private void Entity_NetworkPropertyChanged(Entity sender, Divine.Entity.Entities.EventArgs.NetworkPropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("dota_gamerules")) return;
            if (!e.PropertyName.Contains("m_iHealth")) return;
            //if (sender != this.unit) return;
            //Console.WriteLine($"{GameManager.GameTime:F3} NP sender={sender}   e={e.PropertyName}   new_v={e.NewValue.GetInt32()}  old_v={e.OldValue.GetInt32()}");
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
            this.CalculateForecastHealth();
        }

        private void ForecastAttackerDamage(Unit attacker, TrackingProjectileAddedEventArgs? e = null)
        {
            if (this.attackersForecastDamage.ContainsKey(attacker.Handle)) this.attackersForecastDamage.Remove(attacker.Handle);
            this.attackersForecastDamage.Add(attacker.Handle, new SortedDictionary<float, float>());


            float autoAttackProjectileArriveTime = 0f;
            float time = GameManager.GameTime;
            if (e != null && e.Projectile.IsValid && attacker.IsRanged)
            {
                autoAttackProjectileArriveTime = UnitExtensions.GetProjectileArrivalTime(attacker, this.unit, 0, attacker.ProjectileSpeed());
                this.attackersForecastDamage[attacker.Handle].Add(GameManager.GameTime + autoAttackProjectileArriveTime, attacker.GetAttackDamage(this.unit));
                time = autoAttackProjectileArriveTime;
            }
            else
            {
                //this.attackersForecastDamage[attacker.Handle].Add(GameManager.GameTime, attacker.GetAttackDamage(this.unit));                
            }


            while (time <= forecastDuration)
            {
                time += (1 / attacker.AttacksPerSecond);
                this.attackersForecastDamage[attacker.Handle].Add(time + GameManager.GameTime, attacker.GetAttackDamage(this.unit));
            }
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
                    this.CalculateForecastHealth();
                }
            }
        }

        private void CalculateForecastHealth()
        {
            SortedDictionary<float, float> forecastCumulativeDamage = new(); // <GameTime, cumulativeDamage>
            foreach (KeyValuePair<uint, SortedDictionary<float, float>> attacker in this.attackersForecastDamage)
            {
                Unit unit = (Unit)EntityManager.GetEntityByHandle(attacker.Key);
                if (unit == null) continue;

                if (!unit.IsAlive) continue;

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
            float totalDamage = 0;
            this.forecastHealth.Add(GameManager.GameTime, this.unit.Health);
            foreach (var timeDamage in forecastCumulativeDamage)
            {
                if (timeDamage.Key < GameManager.GameTime) continue;
                totalDamage += timeDamage.Value;
                this.forecastHealth.Add(timeDamage.Key, this.unit.Health - totalDamage + (timeDamage.Key - GameManager.GameTime) * this.unit.HealthRegeneration);
            }
        }
        public SortedDictionary<float, float> GetForecastHealth
        {
            get
            {
                return this.forecastHealth;
            }
        }
    }
}
