using Divine.Entity;
using Divine.Entity.Entities.Units;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Extensions;
using Divine.Game;
using Divine.Numerics;
using Divine.Update;
using System;
using System.Linq;


namespace Emt.Tinker.Managers
{
    static class TargetManager
    {
        static public Hero currentTarget { get; private set; }
        static public Unit nearestEnemyUnitFromTarget { get; private set; }
        static public Hero farestEnemyHeroFromUnit { get; private set; }
        static public Hero targetForRocket { get; private set; }
        static public int targetSearchDistance { get; private set; }

        static private int targerSearchBaseRadius = 650;
        static private int targerSearchAdditionalRadius = 0;
        static private int rocketFlyDistance = 2500;

        static public void Activate()
        {
            PluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }

        static private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
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

        static public void Dispose()
        {
            PluginMenu.PluginStatus.ValueChanged -= PluginStatus_ValueChanged;
            UpdateManager.DestroyIngameUpdate(Update);
        }

        static private void log()
        {
            Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff") + " target found: " + TargetManager.currentTarget);
        }
        static private void Update()
        {
            targetSearchDistance = targerSearchBaseRadius + calculateAdditionalTargerSearchRadius();
            targetForRocket = GetNearestEnemyHero(EntityManager.LocalHero.Position, rocketFlyDistance);

            if (PluginMenu.ComboLockTarget
                && Context.combo.comboKeyHolding
                && currentTarget != null
                && currentTarget.IsAlive
                && !currentTarget.IsInvulnerable()
                && currentTarget.IsVisible
                && !currentTarget.IsMagicImmune()
                && currentTarget.Distance2D(EntityManager.LocalHero) < targetSearchDistance) return;

            if (currentTarget == null && getTarget(targetSearchDistance) != null)
            {
                CastManager.sleeper.Reset();
            }
            currentTarget = getTarget(targetSearchDistance);

            if (currentTarget == null)
            {
                nearestEnemyUnitFromTarget = null;
                farestEnemyHeroFromUnit = null;
                return;
            }
            nearestEnemyUnitFromTarget = GetNearestEnemyUnitToEnemyTarget(currentTarget.Position, 250);
            if (nearestEnemyUnitFromTarget == null)
            {
                farestEnemyHeroFromUnit = null;
                return;
            }
            farestEnemyHeroFromUnit = GetFarestEnemyHeroInRadius(nearestEnemyUnitFromTarget.Position, 700);
        }


        static private int calculateAdditionalTargerSearchRadius()
        {
            targerSearchAdditionalRadius = (int)EntityManager.LocalHero.BonusCastRange;
            if (EntityManager.LocalHero.HasAghanimsScepter()) targerSearchAdditionalRadius += 200;
            return targerSearchAdditionalRadius;
        }

        static public Hero getTarget(int radius)
        {
            if (PluginMenu.ComboTargetSelectorMode == "Nearest to Hero")
            {
                return currentTarget = GetNearestEnemyHero(EntityManager.LocalHero.Position, radius);
            }

            if (PluginMenu.ComboTargetSelectorMode == "In radius of Cursor")
            {
                return currentTarget = GetNearestEnemyHero(GameManager.MousePosition, PluginMenu.ComboTargetSelectorRadius);
            }

            if (PluginMenu.ComboTargetSelectorMode == "First In radius of Cursor, then nearest to Hero")
            {
                Hero h;
                h = currentTarget = GetNearestEnemyHero(GameManager.MousePosition, PluginMenu.ComboTargetSelectorRadius);
                if (h != null) return h;
                return h = currentTarget = GetNearestEnemyHero(EntityManager.LocalHero.Position, radius);
            }
            return null;
        }
        static private Hero GetNearestEnemyHero(Vector3 startPosition, int targerSearchRadius)
        {
            Hero hero;
            hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                !x.IsIllusion
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (hero != null) return hero;
            return null;
        }
        static private Unit GetNearestEnemyUnitToEnemyTarget(Vector3 startPosition, int targerSearchRadius)
        {
            Unit unit = EntityManager.GetEntities<Unit>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                x != currentTarget
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (unit != null) return unit;
            return null;
        }

        static private Hero GetFarestEnemyHeroInRadius(Vector3 startPosition, int targerSearchRadius)
        {
            Hero hero;
            hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                !x.IsIllusion
                                                            )
                                           .OrderByDescending(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (hero != null) return hero;
            return null;
        }


    }
}
