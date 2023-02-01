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
        enum SleeperType
        {
            Movement = 0,
            Attack = 1
        }

        private Context context;
        public HeroManager(Context context)
        {
            this.context = context;
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
            this.MoveToMousePosition();
        }

        private void SimpleAttack()
        {
            if (MultiSleeper<SleeperType>.Sleeping(SleeperType.Attack)) return;

            float requiredTime;
            float aaaTime;
            float sleepTime;
            foreach (KeyValuePair<uint, EmtUnit> u in this.context.EmtUnitManager.unitsTracker!)
            {
                if (!u.Value.unit.IsAlive) continue;

                requiredTime = this.GetMinRequiredTimeToKill(EntityManager.LocalHero, u.Value);
                aaaTime = EntityManager.LocalHero.GetAutoAttackArrivalTime(u.Value.unit, true);

                if (requiredTime - GameManager.GameTime < aaaTime + GameManager.AvgPing + EntityManager.LocalHero.AttackPoint())
                {
                    EntityManager.LocalHero.Attack(u.Value.unit);
                    sleepTime = (GameManager.AvgPing + EntityManager.LocalHero.AttackPoint() + aaaTime) * 1000;

                    MultiSleeper<SleeperType>.Sleep(SleeperType.Attack, sleepTime);
                    MultiSleeper<SleeperType>.Sleep(SleeperType.Movement, sleepTime);

                    Console.WriteLine("--------------------");
                    Console.WriteLine($"GameTime={GameManager.GameTime:F4}   SelectedTime= {requiredTime}");
                    Console.WriteLine($"AAtime={aaaTime:F4}     AP={EntityManager.LocalHero.AttackPoint()}");
                    Console.WriteLine($"unit HP={u.Value.unit.Health}");
                    /*
                    foreach (var item in u.Value.GetForecastHealth)
                    {
                        Console.WriteLine($"time: {item.Key}   HP: {item.Value}");
                    }
                    */
                    return;
                }
            }

        }

        private float GetMinRequiredTimeToKill(Unit hero, EmtUnit creep)
        {
            float time = float.MaxValue;
            float damage = EntityManager.LocalHero.GetAttackDamage(creep.unit, true);

            var unitForecastHealth = creep.GetForecastHealth;
            if (unitForecastHealth.Count == 0) return time;

            foreach (var item in unitForecastHealth)
            {
                if (item.Value < damage)
                {
                    return item.Key;
                }
            }

            return time;
        }

        private void MoveToMousePosition()
        {
            if (MultiSleeper<SleeperType>.Sleeping(SleeperType.Movement)) return;
            if (EntityManager.LocalHero.Distance2D(GameManager.MousePosition) < 100f) return;

            EntityManager.LocalHero.Move(GameManager.MousePosition);
            MultiSleeper<SleeperType>.Sleep(SleeperType.Movement, 100f);

        }
    }
}
