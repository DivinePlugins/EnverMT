using System;
using System.Linq;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Update;
using Divine.Extensions;
using Divine.Entity;
using Divine.Numerics;
using Divine.Entity.Entities.Abilities.Components;
using Emt.Tinker.Managers;

namespace Emt.Tinker.Modes
{
    class AutoShiva : IDisposable
    {
        public AutoShiva()
        {
            PluginMenu.AutoShivaSwitcher.ValueChanged += AutoShiva_ValueChanged;
        }
        public void Dispose()
        {
            PluginMenu.AutoShivaSwitcher.ValueChanged -= AutoShiva_ValueChanged;
            UpdateManager.IngameUpdate -= UpdateManager_IngameUpdate;
        }
        private void AutoShiva_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.IngameUpdate += UpdateManager_IngameUpdate;
            }
            else
            {
                UpdateManager.IngameUpdate -= UpdateManager_IngameUpdate;
            }
        }
        private void UpdateManager_IngameUpdate()
        {
            if (CastManager.sleeper.Sleeping) return;
            if (UnitExtensions.GetItemById(EntityManager.LocalHero, AbilityId.item_shivas_guard) == null) return;

            Vector3 startPosition = EntityManager.LocalHero.Position;
            int targerSearchRadius = PluginMenu.AutoShivaRadius;

            Hero hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                !x.IsInvulnerable()
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (hero != null) CastManager.castItem(AbilityId.item_shivas_guard);
        }
    }
}