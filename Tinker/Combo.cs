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
            
            if (TargetManager.CurrentTarget != null) if (executeCombo()) return;

            if (TargetManager.CurrentTarget == null)
            {
                if (c.castDefensiveMatrix()) return;
                if (c.castBlink()) return;
                Context.TargetManager.TargetUpdater();
                if (TargetManager.CurrentTarget == null)
                {
                    if (c.castSoulRing()) return;
                    if (c.castGuardianGreaves()) return;                    
                    if (c.castRearm()) return;
                }
                    
            }
            
        }

        private bool executeCombo()
        {
            CastItemsAndAbilities c = Context.CastItemsAndAbilities;
            if (executeLinkenSphereBreaking()) return true;

            if (antiGangWithWarp()) return true;

            if (c.castLaser()) return true;

            if (c.castSoulRing()) return true;
            if (c.castGuardianGreaves()) return true;

            if (c.castHeatSeekingMissile()) return true;

            if (c.castShivasGuard()) return true;
            if (c.castBloodstone()) return true;
            if (c.castEternalShroud()) return true;

            if (c.castLotusOrb()) return true;
            if (c.castGhostScepter()) return true;
            if (c.castGlimmerCape()) return true;

            if (c.castVeilOfDiscord()) return true;
            if (c.castNullifier()) return true;
            if (c.castRodOfAtos()) return true;

            if (c.castOrchid()) return true;
            if (c.castBloodthorn()) return true;
            
            if (c.castEtheralBlade()) return true;
            if (c.castDagon()) return true;
            if (c.castScytheOfVyse()) return true;            

            if (c.castBlink()) return true;
            if (c.castRearm()) return true;          
            return false;
        }



        private bool executeLinkenSphereBreaking()
        {
            CastItemsAndAbilities c = Context.CastItemsAndAbilities;            
            if (TargetManager.CurrentTarget == null) return false;
            if (!TargetManager.CurrentTarget.HasAnyModifiers("modifier_item_sphere_target", "modifier_item_sphere")) return false;            
            if (Context.PluginMenu.ComboLinkenBreakerMode == "First what can be used (not Hex)")
            {
                if (c.castEtheralBlade()) return true;
                if (c.castDagon()) return true;
                if (c.castOrchid()) return true;
                if (c.castBloodthorn()) return true;
                if (c.castRodOfAtos()) return true;
                if (c.castNullifier()) return true;                
            }
            
            if (Context.PluginMenu.ComboLinkenBreakerMode == "Laser") if (c.castLaser()) return true;
            
            return false;            
        }    
        
        private bool antiGangWithWarp()
        {
            if (TargetManager.CurrentTarget == null) return false;
            if (LocalHero.Distance2D(TargetManager.CurrentTarget) <= 250)
            {                
                if (Context.CastItemsAndAbilities.castWarpGrenade()) return true;
            }                
            return false;            
        }
    }
}