using Divine.Entity;
using Divine.Entity.Entities.Units;
using Divine.Extensions;
using Divine.Game;
using Divine.Helpers;
using Divine.Update;

namespace EMT.Farm
{
    internal class HeroManager : IDisposable
    {
        private Context context;
        private Sleeper sleeper;
        public HeroManager(Context context)
        {
            this.context = context;
            this.sleeper = new();
            this.context.pluginMenu.modeFarm.ValueChanged += ModeFarm_ValueChanged;
        }
        public void Dispose()
        {
            this.context.pluginMenu.modeFarm.ValueChanged -= ModeFarm_ValueChanged;
            UpdateManager.DestroyIngameUpdate(Update);
        }

        private void ModeFarm_ValueChanged(Divine.Menu.Items.MenuHoldKey holdKey, Divine.Menu.EventArgs.HoldKeyEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.CreateIngameUpdate(Update);
            }
            else
            {
                UpdateManager.DestroyIngameUpdate(Update);
            }
        }
        private void Update()
        {
            if (EntityManager.LocalHero == null) return;
            if (this.context.EmtUnitManager == null) return;
            if (this.context?.EmtUnitManager?.unitsTracker.Count == 0) return;

            this.SimpleAttack();
        }

        private void SimpleAttack()
        {
            if (this.sleeper.Sleeping) return;

            float requiredTime;
            float aaaTime;
            foreach (KeyValuePair<uint, EmtUnit> u in this.context.EmtUnitManager.unitsTracker!)
            {
                if (!u.Value.unit.IsAlive) continue;

                requiredTime = this.GetMinRequiredTimeToKill(EntityManager.LocalHero, u.Value);
                aaaTime = EntityManager.LocalHero.GetAutoAttackArrivalTime(u.Value.unit, true);

                if (requiredTime < aaaTime + GameManager.AvgPing)
                {
                    EntityManager.LocalHero.Attack(u.Value.unit);
                    this.sleeper.Sleep(300);
                    return;
                }
            }
        }

        private float GetMinRequiredTimeToKill(Unit hero, EmtUnit creep)
        {
            float time = float.MaxValue;
            float damage = EntityManager.LocalHero.GetAttackDamage(creep.unit, true);

            var unitForecastHealth = creep.GetForecastHealth;

            foreach (var item in unitForecastHealth)
            {
                if (item.Value < damage)
                {
                    return item.Key - GameManager.GameTime;
                }
            }

            return time;
        }
    }
}
