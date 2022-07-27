using System;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Update;
using Divine.Extensions;
using Divine.Entity;
using Divine.Entity.Entities.Abilities.Components;

namespace Emt_Tinker
{
    class Combo : IDisposable
    {
        #region Variables       
        public bool comboKeyHolding;
        private readonly Context Context;
        private Hero _localHero = EntityManager.LocalHero;        
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
            this.comboKeyHolding = false;
        }
        private void ComboKey_ValueChanged(Divine.Menu.Items.MenuHoldKey holdKey, Divine.Menu.EventArgs.HoldKeyEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.IngameUpdate += UpdateManager_IngameUpdate;
                this.comboKeyHolding = true;                
            }
            else
            {
                UpdateManager.IngameUpdate -= UpdateManager_IngameUpdate;
                this.comboKeyHolding = false;
            }
        }
        private void UpdateManager_IngameUpdate()
        {
            if (CastItemsAndAbilities.sleeper.Sleeping) return;           

            if (Context.TargetManager.currentTarget != null 
                && this._localHero.IsAlive
                && Context.TargetManager.currentTarget.IsAlive
                && !Context.TargetManager.currentTarget.IsInvulnerable()
                && Context.TargetManager.currentTarget.IsVisible
                && !Context.TargetManager.currentTarget.IsMagicImmune()
                && this._localHero.IsInRange(Context.TargetManager.currentTarget, Context.TargetManager.targetSearchDistance)
                )
            {
                if (executeCombo()) return;
            }
            
            CastItemsAndAbilities c = Context.CastItemsAndAbilities;
            if (Context.TargetManager.targetForRocket!=null) if (c.castHeatSeekingMissile()) return;
            if (c.castDefensiveMatrix()) return;
            if (c.castNoTargetCastItems()) return;
            if (c.castBlink()) return;            
            if (c.castRearm()) return;
            
        }

        private bool executeCombo()
        {

            CastItemsAndAbilities c = Context.CastItemsAndAbilities;
            if (c.castDefensiveMatrix()) return true;

            if (executeLinkenSphereBreaking()) return true;

            if (c.castNoTargetCastItems()) return true;
            if (c.castSelfCastItems()) return true;
            if (c.castTargetCastItems()) return true;
            if (c.castTargetDagonCastItems()) return true;

            if (c.castWarpGrenade()) return true;
            if (c.castHeatSeekingMissile()) return true;
            if (c.castLaser()) return true;
            if (c.castBlink()) return true;
            if (c.castRearm()) return true;
           
            return false;
        }

        private bool executeLinkenSphereBreaking()
        {
            CastItemsAndAbilities c = Context.CastItemsAndAbilities;            
            if (Context.TargetManager.currentTarget == null) return false;
            if (!UnitExtensions.IsLinkensProtected(Context.TargetManager.currentTarget)) return false;            

            if (Context.PluginMenu.ComboLinkenBreakerMode == "First what can be used (not Hex)")
            {                
                if (c.castTargetDagonCastItems()) return true;
                if (c.castLaser()) return true;
                if (c.castTargetCastItems()) return true;
            }
            
            if (Context.PluginMenu.ComboLinkenBreakerMode == "Laser") if (c.castLaser()) return true;
            
            return false;            
        }    
    }
}