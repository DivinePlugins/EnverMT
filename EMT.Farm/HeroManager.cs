using Divine.Entity;
using Divine.Entity.Entities.Units;
using Divine.Extensions;
using Divine.Game;
using Divine.Update;

namespace EMT.Farm
{
    internal class HeroManager : IDisposable
    {
        private Context context;
        public HeroManager(Context context)
        {
            this.context = context;
            this.context.pluginMenu.pluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }
        public void Dispose()
        {
            this.context.pluginMenu.pluginStatus.ValueChanged -= PluginStatus_ValueChanged;
            UpdateManager.DestroyIngameUpdate(Update);
        }

        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
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

            float requiredTime;
            float aaaTime;
            foreach (KeyValuePair<uint, EmtUnit> u in this.context.EmtUnitManager.unitsTracker!)
            {
                requiredTime = this.GetMinRequiredTimeToKill(EntityManager.LocalHero, u.Value);
                aaaTime =  EntityManager.LocalHero.GetAutoAttackArrivalTime(u.Value.unit, true);

                if (requiredTime < aaaTime + GameManager.AvgPing)
                {
                    EntityManager.LocalHero.Attack(u.Value.unit);
                    Console.WriteLine($"Attack to: {u.Value.unit}  . Time= {requiredTime}");
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
