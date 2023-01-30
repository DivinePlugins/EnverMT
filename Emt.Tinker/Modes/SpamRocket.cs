using Divine.Entity;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Extensions;
using Divine.Update;
using Emt.Tinker.Managers;
using System;

namespace Emt.Tinker.Modes
{
    class SpamRocket : IDisposable
    {
        #region Variables               
        private Hero _localHero = EntityManager.LocalHero;
        #endregion


        public SpamRocket()
        {
            PluginMenu.SpamRocketKey.ValueChanged += ComboKey_ValueChanged;
        }
        public void Dispose()
        {
            PluginMenu.SpamRocketKey.ValueChanged -= ComboKey_ValueChanged;
            UpdateManager.IngameUpdate -= UpdateManager_IngameUpdate;
        }
        private void ComboKey_ValueChanged(Divine.Menu.Items.MenuHoldKey holdKey, Divine.Menu.EventArgs.HoldKeyEventArgs e)
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
            if (TargetManager.targetForRocket != null
                && _localHero.IsAlive
                && TargetManager.targetForRocket.IsAlive
                && !TargetManager.targetForRocket.IsInvulnerable()
                && TargetManager.targetForRocket.IsVisible
                && !TargetManager.targetForRocket.IsMagicImmune()
                )
            {
                if (executeSpamRocket()) return;
            }
        }

        private bool executeSpamRocket()
        {
            if (CastManager.castItem(AbilityId.item_soul_ring)) return true;
            if (CastManager.castItem(AbilityId.item_guardian_greaves)) return true;
            if (CastManager.castItem(AbilityId.item_glimmer_cape, EntityManager.LocalHero)) return true;
            if (CastManager.castItem(AbilityId.item_ghost)) return true;
            if (CastManager.castItem(AbilityId.item_bloodstone)) return true;
            if (CastManager.castItem(AbilityId.item_eternal_shroud)) return true;

            if (CastManager.castHeatSeekingRocket()) return true;

            if (PluginMenu.ComboItems.GetValue(AbilityId.item_blink))
            {
                if (CastManager.castBlink(AbilityId.item_blink)) return true;
                if (CastManager.castBlink(AbilityId.item_arcane_blink)) return true;
                if (CastManager.castBlink(AbilityId.item_overwhelming_blink)) return true;
                if (CastManager.castBlink(AbilityId.item_swift_blink)) return true;
            }

            if (CastManager.castRearm()) return true;

            return false;
        }
    }
}