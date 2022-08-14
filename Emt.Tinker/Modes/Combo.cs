using System;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Update;
using Divine.Extensions;
using Divine.Entity;
using Divine.Entity.Entities.Abilities.Components;
using Emt.Tinker.Managers;

namespace Emt.Tinker.Modes
{
    class Combo : IDisposable
    {
        #region Variables       
        public bool comboKeyHolding;

        private Hero _localHero = EntityManager.LocalHero;
        #endregion


        public Combo()
        {
            PluginMenu.ComboKey.ValueChanged += ComboKey_ValueChanged;
        }
        public void Dispose()
        {
            PluginMenu.ComboKey.ValueChanged -= ComboKey_ValueChanged;
            UpdateManager.IngameUpdate -= UpdateManager_IngameUpdate;
        }
        private void ComboKey_ValueChanged(Divine.Menu.Items.MenuHoldKey holdKey, Divine.Menu.EventArgs.HoldKeyEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.IngameUpdate += UpdateManager_IngameUpdate;
                comboKeyHolding = true;
            }
            else
            {
                UpdateManager.IngameUpdate -= UpdateManager_IngameUpdate;
                comboKeyHolding = false;
            }
        }
        private void UpdateManager_IngameUpdate()
        {
            if (CastManager.sleeper.Sleeping) return;


            if (TargetManager.currentTarget == null || !EntityManager.LocalHero.IsInRange(TargetManager.currentTarget, 1025))
            {
                if (CastManager.castNeutralItem(AbilityId.item_ninja_gear)) return;
            }


            if (TargetManager.currentTarget != null
                && _localHero.IsAlive
                && TargetManager.currentTarget.IsAlive
                && !TargetManager.currentTarget.IsInvulnerable()
                && TargetManager.currentTarget.IsVisible
                && !TargetManager.currentTarget.IsMagicImmune()
                && _localHero.IsInRange(TargetManager.currentTarget, TargetManager.targetSearchDistance)
                )
            {
                if (executeCombo()) return;
            }

            freeRoam();

        }

        private bool executeCombo()
        {
            //Console.WriteLine("============");
            //Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff") + " - exec combo - Mana: " + EntityManager.LocalHero.Mana);            

            if (CastManager.castNeutralItem(AbilityId.item_seer_stone, TargetManager.currentTarget.Position)) return true;

            if (PluginMenu.ComboLinkenBreakerMode == "Laser" && TargetManager.currentTarget.IsLinkensProtected())
            {
                if (CastManager.castLaser()) return true;
            }

            if (EntityManager.LocalHero.MaximumMana - EntityManager.LocalHero.Mana >= 200)
            {
                if (CastManager.castItem(AbilityId.item_guardian_greaves)) return true;
            }

            if (EntityManager.LocalHero.MaximumMana - EntityManager.LocalHero.Mana >= 75)
            {
                if (CastManager.castNeutralItem(AbilityId.item_arcane_ring)) return true;
            }

            if (CastManager.castWarpGrenade()) return true;

            if (EntityManager.LocalHero.IsInRange(TargetManager.currentTarget, PluginMenu.ComboWarpGrenadeUseRadius))
            {
                if (CastManager.castNeutralItem(AbilityId.item_psychic_headband, TargetManager.currentTarget)) return true;
            }

            if (CastManager.castDefenseMatrix()) return true;

            if (CastManager.castItem(AbilityId.item_soul_ring)) return true;

            if (CastManager.castNeutralItem(AbilityId.item_bullwhip, TargetManager.currentTarget)) return true;


            if (CastManager.castItem(AbilityId.item_lotus_orb, EntityManager.LocalHero)) return true;
            if (CastManager.castItem(AbilityId.item_glimmer_cape, EntityManager.LocalHero)) return true;
            if (CastManager.castItem(AbilityId.item_ghost)) return true;
            if (CastManager.castItem(AbilityId.item_shivas_guard)) return true;
            if (CastManager.castItem(AbilityId.item_bloodstone)) return true;
            if (CastManager.castItem(AbilityId.item_eternal_shroud)) return true;
            if (CastManager.castItem(AbilityId.item_ethereal_blade, TargetManager.currentTarget)) return true;
            if (CastManager.castItem(AbilityId.item_orchid, TargetManager.currentTarget)) return true;
            if (CastManager.castItem(AbilityId.item_bloodthorn, TargetManager.currentTarget)) return true;
            if (CastManager.castItem(AbilityId.item_rod_of_atos, TargetManager.currentTarget)) return true;
            if (CastManager.castItem(AbilityId.item_nullifier, TargetManager.currentTarget)) return true;

            if (PluginMenu.ComboLinkenBreakerMode == "First what can be used (not Hex)" && TargetManager.currentTarget.IsLinkensProtected())
            {
                if (CastManager.castLaser()) return true;
            }

            if (!TargetManager.currentTarget.IsReflectingAbilities() && !TargetManager.currentTarget.HasModifier("modifier_antimage_counterspell"))
            {
                if (CastManager.castItem(AbilityId.item_sheepstick, TargetManager.currentTarget)) return true;
            }

            if (PluginMenu.ComboItems.GetValue(AbilityId.item_dagon))
            {
                if (CastManager.castItem(AbilityId.item_dagon, TargetManager.currentTarget, false)) return true;
                if (CastManager.castItem(AbilityId.item_dagon_2, TargetManager.currentTarget, false)) return true;
                if (CastManager.castItem(AbilityId.item_dagon_3, TargetManager.currentTarget, false)) return true;
                if (CastManager.castItem(AbilityId.item_dagon_4, TargetManager.currentTarget, false)) return true;
                if (CastManager.castItem(AbilityId.item_dagon_5, TargetManager.currentTarget, false)) return true;
            }

            if (CastManager.castHeatSeekingRocket()) return true;
            if (CastManager.castLaser()) return true;

            if (CastManager.castNeutralItem(AbilityId.item_ex_machina)) return true;

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

        private bool freeRoam()
        {
            float additionalSleepTime = 320f;

            if (TargetManager.targetForRocket != null) if (CastManager.castHeatSeekingRocket()) return true;
            if (PluginMenu.ComboItems.GetValue(AbilityId.item_blink))
            {
                if (CastManager.castBlink(AbilityId.item_blink, additionalSleepTime)) return true;
                if (CastManager.castBlink(AbilityId.item_arcane_blink, additionalSleepTime)) return true;
                if (CastManager.castBlink(AbilityId.item_overwhelming_blink, additionalSleepTime)) return true;
                if (CastManager.castBlink(AbilityId.item_swift_blink, additionalSleepTime)) return true;
            }

            if (CastManager.castItem(AbilityId.item_soul_ring)) return true;
            if (CastManager.castItem(AbilityId.item_guardian_greaves)) return true;

            if (CastManager.castRearm()) return true;
            return false;
        }
    }
}