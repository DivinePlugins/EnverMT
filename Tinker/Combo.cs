using System;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Update;
using Divine.Helpers;
using Divine.Extensions;
using Tinker.AbilitiesAndItems;
using Divine.Entity;
using Divine.Game;

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
            if (!this.LocalHero.IsAlive)
            {
                return;
            }

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
            if (sleeper.Sleeping) return;
            if (executeCombo()) return; // use Combo on enemy before blink
            if (executeBlink()) return;
            if (executeCombo()) return; // in case, if enemy around after blink, Combo should be used on enemy
            if (executeRearm()) return;
        }
        private bool executeBlink()
        {            
            if (!this.sleeper.Sleeping && this.items.blink.CanBeCasted())
            {
                this.items.blink.Cast(GameManager.MousePosition, false, false);
                this.sleeper.Sleep(this.items.blink.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                this.comboState = true;
            
                return true;
            }            
            return false;
        }

        private bool executeRearm()
        {            
            if (!this.sleeper.Sleeping && this.comboState && this.abilities.rearm.CanBeCasted())
            {
                this.abilities.rearm.Cast(false, false);                
                this.sleeper.Sleep(this.abilities.rearm.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                this.comboState = false;
                return true;
            }
            return false;
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
            if (!this.sleeper.Sleeping && !UnitExtensions.HasModifier(this.LocalHero, "modifier_tinker_defense_matrix") && this.abilities.defenseMatrix.CanBeCasted())
            {
                this.abilities.defenseMatrix.Cast(this.LocalHero, false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.abilities.defenseMatrix.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);                
                return true;
            }
            return false;
        }
        private bool castGuardianGreaves()
        {
            if (!this.sleeper.Sleeping && this.items.guardianGreaves.CanBeCasted())
            {
                this.items.guardianGreaves.Cast(false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.items.guardianGreaves.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);                
                return true;
            }
            return false;
        }
        private bool castShivasGuard()
        {
            if (!this.sleeper.Sleeping && this.items.shivasGuard.CanBeCasted())
            {
                this.items.shivasGuard.Cast(false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.items.shivasGuard.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }
        private bool castBloodstone()
        {
            if (!this.sleeper.Sleeping && !UnitExtensions.HasModifier(this.LocalHero, "modifier_item_bloodstone_drained") && this.items.bloodStone.CanBeCasted())
            {
                this.items.bloodStone.Cast(false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.items.bloodStone.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }
        private bool castScytheOfVyse()
        {
            if (!this.sleeper.Sleeping && this.items.scytheOfVyse.CanBeCasted())
            {
                this.items.scytheOfVyse.Cast(this.target, false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.items.scytheOfVyse.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }
        private bool castEtheralBlade()
        {
            if (!this.sleeper.Sleeping && this.items.etherealBlade.CanBeCasted())
            {
                this.items.etherealBlade.Cast(this.target, false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.items.etherealBlade.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }
        private bool castDagon()
        {
            if (!this.sleeper.Sleeping && this.items.dagon.CanBeCasted())
            {
                this.items.dagon.Cast(this.target, false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.items.dagon.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }
        private bool castHeatSeekingMissile()
        {
            if (!this.sleeper.Sleeping && this.abilities.heatSeekingMissile.CanBeCasted())
            {
                this.abilities.heatSeekingMissile.Cast(false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.abilities.heatSeekingMissile.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }
        private bool castLaser()
        {
            if (!this.sleeper.Sleeping && this.abilities.laser.CanBeCasted())
            {
                this.abilities.laser.Cast(this.target, false, false);
                this.comboState = true;
                this.sleeper.Sleep(this.abilities.laser.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
                return true;
            }
            return false;
        }


    }
}