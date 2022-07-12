using System;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Update;
using Divine.Helpers;
using Divine.Extensions;
using Tinker.AbilitiesAndItems;
using Divine.Entity;
using Divine.Game;
using Divine.Entity.Entities.Abilities.Components;

namespace Tinker
{
    class Combo : IDisposable
    {
        #region Variables
        private Context Context;
        private Hero LocalHero = EntityManager.LocalHero;
        private bool comboState = false;
        private Sleeper sleeper = new Sleeper();
        public Items items = new Items();
        public Abilities abilities = new Abilities();
        private Hero target;
        #endregion

        public Combo(Context context)
        {
            Context = context;
            Context.PluginMenu.ComboKey.ValueChanged += ComboKey_ValueChanged;            
        }
        public void Dispose()
        {
            Context.PluginMenu.ComboKey.ValueChanged -= ComboKey_ValueChanged;
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
            if (this.sleeper.Sleeping) return;
            if (!this.LocalHero.IsAlive) return;
            

            this.items.Update();
            this.abilities.Update();

            this.target = TargetManager.CurrentTarget;
            if (this.target == null)
            {
                if (executeBlink()) return; // simply blink/rearm if there are no enemy                                
                return;
            }
            if (this.target == null && comboState)
            {
                if (executeRearm()) return; // simply blink/rearm if there are no enemy                                
                return;
            }            
            if (executeCombo()) return; // use Combo on enemy before blink
            if (executeBlink()) return;
            if (executeCombo()) return; // in case, if enemy around after blink, Combo should be used on enemy
            if (executeRearm()) return;
        }
        private bool executeBlink()
        {            
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_blink)) return false;
            if (!this.items.blink.CanBeCasted()) return false;

            if (Context.PluginMenu.ComboBlinkMode== "To cursor") 
                this.items.blink.Cast(GameManager.MousePosition, false, false);

            if (Context.PluginMenu.ComboBlinkMode == "In radius")
            {                
                if (this.target != null) this.items.blink.Cast(Vector3Extensions.Extend(this.target.Position, GameManager.MousePosition, Context.PluginMenu.ComboTargetSelectorRadius), false, false);
                if (this.target == null) this.items.blink.Cast(GameManager.MousePosition, false, false);
            }                

            this.sleeper.Sleep(this.items.blink.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            this.comboState = true;            
            return true;
        }

        private bool executeRearm()
        {
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_rearm)) return false;
            if (!this.comboState) return false;
            if (!this.abilities.rearm.CanBeCasted()) return false;

            this.abilities.rearm.Cast(false, false);
            this.sleeper.Sleep(this.abilities.rearm.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            this.comboState = false;
            return true;
        }

        private bool executeCombo()
        {
            // start of Combo            
            if (this.castDefensiveMatrix()) return true;
            if (this.castGuardianGreaves()) return true;
            if (this.castShivasGuard()) return true;
            if (this.castBloodstone()) return true;
            if (this.castScytheOfVyse()) return true;
            if (this.castEtheralBlade()) return true;
            if (this.castDagon()) return true;
            if (this.castHeatSeekingMissile()) return true;
            if (this.castLaser()) return true;

            return false;
        }

        private bool castDefensiveMatrix()
        {
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_defense_matrix)) return false;
            if (UnitExtensions.HasModifier(this.LocalHero, "modifier_tinker_defense_matrix")) return false;
            if (!this.abilities.defenseMatrix.CanBeCasted()) return false;
            
            this.abilities.defenseMatrix.Cast(this.LocalHero, false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.abilities.defenseMatrix.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);            
            return true;
        }
        private bool castGuardianGreaves()
        {
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_guardian_greaves)) return false;
            if (!this.items.guardianGreaves.CanBeCasted()) return false;

            this.items.guardianGreaves.Cast(false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.items.guardianGreaves.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castShivasGuard()
        {
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_shivas_guard)) return false;
            if (!this.items.shivasGuard.CanBeCasted()) return false;
            
            this.items.shivasGuard.Cast(false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.items.shivasGuard.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castBloodstone()
        {
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_bloodstone)) return false;
            if (UnitExtensions.HasModifier(this.LocalHero, "modifier_item_bloodstone_drained")) return false;
            if (!this.items.bloodStone.CanBeCasted()) return false;

            this.items.bloodStone.Cast(false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.items.bloodStone.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castScytheOfVyse()
        {
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_sheepstick)) return false;
            if (!this.items.scytheOfVyse.CanBeCasted()) return false;

            this.items.scytheOfVyse.Cast(this.target, false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.items.scytheOfVyse.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castEtheralBlade()
        {
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_ethereal_blade)) return false;
            if (!this.items.etherealBlade.CanBeCasted()) return false;

            this.items.etherealBlade.Cast(this.target, false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.items.etherealBlade.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castDagon()
        {
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_dagon)) return false;
            if (!this.items.dagon.CanBeCasted()) return false;
            
            this.items.dagon.Cast(this.target, false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.items.dagon.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castHeatSeekingMissile()
        {
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_heat_seeking_missile)) return false;
            if (!this.abilities.heatSeekingMissile.CanBeCasted()) return false;

            this.abilities.heatSeekingMissile.Cast(false, false);
            this.comboState = true;
            this.sleeper.Sleep(this.abilities.heatSeekingMissile.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
        private bool castLaser()
        {
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_laser)) return false;
            if (!this.abilities.laser.CanBeCasted()) return false;
            
            this.abilities.laser.Cast(this.target, false, false);
            this.comboState = true;
            //this.sleeper.Sleep(this.abilities.laser.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            return true;
        }
    }
}