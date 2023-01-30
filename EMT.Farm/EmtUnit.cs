using Divine.Entity.Entities;
using Divine.Entity.Entities.Units;
using Divine.Entity.Entities.Units.Creeps;
using Divine.Extensions;
using Divine.Game;
using Divine.Projectile;
using Divine.Update;

namespace EMT.Farm
{
    public class EmtUnit : IDisposable
    {
        public readonly Unit unit;
        private readonly Dictionary<Unit, float> attackers; // Attacked unit, last attacked GameTime
        private readonly SortedDictionary<float, float> forecastHealth; // Forecast Health <Gametime, health>
        private const float forecastDuration = 3f;
        private const float forecastQuant = 0.02f;

        public EmtUnit(Unit unit)
        {
            this.unit = unit;
            this.attackers = new();
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

            this.AddAttackerToList(unit);
        }

        private void AddAttackerToList(Unit unit)
        {
            if (unit == null) return;

            this.attackers[unit] = GameManager.GameTime;
        }

        private void RemoveIdleAttackersFromList()
        {
            foreach (KeyValuePair<Unit, float> attacker in this.attackers)
            {
                if (attacker.Key is not Creep) return;
                float idleTime = 1.5f;
                if (attacker.Key.AttackDamageType == Divine.Entity.Entities.Units.Components.AttackDamageType.Siege) idleTime = 3f;

                if ((GameManager.GameTime - attacker.Value) > idleTime)
                {
                    this.attackers.Remove(attacker.Key);
                }
            }
        }

        private void CalculateForecastHealth()
        {
            this.forecastHealth.Clear();
            var notValidAttackers = this.attackers.Where(u => !u.Key.IsValid);
            foreach (var attacker in notValidAttackers)
            {
                this.attackers.Remove(attacker.Key);
            }


            float fTime = GameManager.GameTime;
            float damage;

            while (fTime <= GameManager.GameTime + forecastDuration)
            {
                damage = 0;
                foreach (KeyValuePair<Unit, float> attackerUnit in this.attackers)
                {
                    damage += this.GetForecastDamage(attackerUnit.Key, fTime - GameManager.GameTime);
                }
                this.forecastHealth.Add(fTime, (float)this.unit.Health - damage);
                fTime += forecastQuant;
            }
        }

        private float GetForecastDamage(Unit unit, float timeDuration)
        {
            float timeDiff = 0;
            if (this.attackers.ContainsKey(unit))
            {
                timeDiff = GameManager.GameTime - this.attackers[unit];
            }

            // TO DO: Delay before attack to be clarified            

            float projecttileTime = UnitExtensions.GetAutoAttackArrivalTime(unit, this.unit, true);
            float damage = (float)Math.Floor((timeDiff + timeDuration - projecttileTime) * unit.AttacksPerSecond) * unit.GetAttackDamage(this.unit, true);
            float regenedHealth = unit.HealthRegeneration * timeDuration;
            return damage - regenedHealth;
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
