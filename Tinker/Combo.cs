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
        /*
        public Items items = new Items();
        public Abilities abilities = new Abilities();
        */
        private Context Context;
        private Hero LocalHero = EntityManager.LocalHero;
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
            if (!this.LocalHero.IsAlive) return;
            if (CastItemsAndAbilities.sleeper.Sleeping) return;
            CastItemsAndAbilities c = Context.CastItemsAndAbilities;
            c.updateItemsAndAbilities();
            
            this.target = TargetManager.CurrentTarget;
            if (executeCombo()) return;
        }

        private bool executeCombo()
        {
            CastItemsAndAbilities c = Context.CastItemsAndAbilities;
            if (c.castHeatSeekingMissile()) return true;
            
            if (c.castBloodstone()) return true;
            if (c.castDagon()) return true;
            if (c.castScytheOfVyse()) return true;
            if (c.castEtheralBlade()) return true;
            if (c.castLaser()) return true;

            if (c.castBlink()) return true;
            if (c.castRearm()) return true;
            return false;
        }
        private bool executeLinkenSphereBreaking()
        {
            /*
            if (!this.target.HasAnyModifiers("modifier_item_sphere_target")) return false;
            if (Context.PluginMenu.ComboLinkenBreakerMode == "First what can be used (not Hex)")
            {
                //if (this.castEtheralBlade()) return true;
                //if (this.castDagon()) return true;
                //if (this.castLaser()) return true;
            }
            */
            //if (Context.PluginMenu.ComboLinkenBreakerMode == "Laser") if (this.castLaser()) return true;
            return false;
        }

        
    }
}