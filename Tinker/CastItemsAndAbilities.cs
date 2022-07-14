﻿using System;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Helpers;
using Divine.Extensions;
using Divine.Entity;
using Divine.Game;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Units;
using Tinker.AbilitiesAndItems;
using Divine.Update;

namespace Tinker
{
    class CastItemsAndAbilities
    {
        #region Variables
        public Items items = new Items();
        public Abilities abilities = new Abilities();
        static public Sleeper sleeper = new Sleeper();

        private Base item;
        private Context Context;                
        private bool comboState = false;
        #endregion

        public CastItemsAndAbilities(Context context)
        {
            Context = context;
            Context.PluginMenu.PluginStatus.ValueChanged += CastItemsAndAbilities_ValueChanged;
        }

        private void log(Base abil)
        {
            return;
            Console.WriteLine(abil + " used: " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + " on target: "+Context.TargetManager.currentTarget);
        }
              
        private void CastItemsAndAbilities_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.CreateIngameUpdate(500, updateItemsAndAbilities);
            }
            else
            {
                UpdateManager.DestroyIngameUpdate(updateItemsAndAbilities);
            }
        }        

        public void Dispose()
        {
            UpdateManager.DestroyIngameUpdate(updateItemsAndAbilities);
        }

        public void updateItemsAndAbilities()
        {
            this.items.Update();
            this.abilities.Update();            
        }

        private void setSleeper()
        {
            sleeper.Sleep(this.item.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            comboState = true;
        }

        public bool castBlink()
        {            
            this.item = this.items.blink;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_blink)) return false;            
            
            if (!this.item.CanBeCasted()) return false;            
            
            if (Context.PluginMenu.ComboBlinkMode == "To cursor")
                this.item.Cast(GameManager.MousePosition, false, false);

            if (Context.PluginMenu.ComboBlinkMode == "In radius")
            {
                if (Context.TargetManager.currentTarget != null) this.item.Cast(Vector3Extensions.Extend(Context.TargetManager.currentTarget.Position, GameManager.MousePosition, Context.PluginMenu.ComboBlinkModeRadius), false, false);
                if (Context.TargetManager.currentTarget == null) this.item.Cast(GameManager.MousePosition, false, false);
            }
            //
            if (Context.TargetManager.currentTarget != null) sleeper.Sleep(item.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            if (Context.TargetManager.currentTarget == null) sleeper.Sleep(item.GetAbility().CastPoint * 1000f + 350f + GameManager.AvgPing);
            comboState = true;
            this.log(this.item);
            return true;
        }

        #region Abilities
        public bool castLaser()
        {
            this.item = abilities.laser;                ;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_laser)) return false;

            if (Context.PluginMenu.ComboSmartLaser) 
            {
                Unit unitNearestToTarget = Context.TargetManager.nearestEnemyUnitFromTarget;
                if (unitNearestToTarget!=null)
                {
                    if (UnitExtensions.IsReflectingAbilities(Context.TargetManager.currentTarget)) if (smartLaser(unitNearestToTarget)) return true;
                    
                    if (Context.TargetManager.farestEnemyHeroFromTarget == Context.TargetManager.currentTarget && EntityManager.LocalHero.HasAghanimsScepter()) 
                        if (smartLaser(unitNearestToTarget)) return true;
                }
            }             
            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;
            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            this.log(this.item);
            setSleeper();
            return true;                        
        }
        private bool smartLaser(Unit unitNearestToTarget)
        {            
            if (!this.item.CanBeCasted(unitNearestToTarget)) return false;

            this.item.Cast(unitNearestToTarget, false, false);
            this.log(this.item);
            setSleeper();            
            return true;
        }

        public bool castHeatSeekingMissile()
        {
            this.item = this.abilities.heatSeekingMissile;            
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_heat_seeking_missile)) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();            
            return true;
        }
        public bool castDefensiveMatrix()
        {
            this.item = abilities.defenseMatrix;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_defense_matrix)) return false;
            if (UnitExtensions.HasModifier(EntityManager.LocalHero, "modifier_tinker_defense_matrix")) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(EntityManager.LocalHero, false, false);
            setSleeper();
            return true;
        }
        public bool castWarpGrenade()
        {
            this.item = abilities.warpGrenade;            
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_warp_grenade)) return false;            
            if (!EntityManager.LocalHero.IsInRange(Context.TargetManager.currentTarget, Context.PluginMenu.ComboWarpGrenadeUseRadius)) return false;            
            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;            

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castRearm()
        {
            this.item = abilities.rearm;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_rearm)) return false;
            if (!comboState) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            this.log(this.item);
            comboState = false;
            return true;
        }
        #endregion

        #region NoTargetCastItems
        public bool castBloodstone()
        {
            this.item = this.items.bloodStone;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_bloodstone)) return false;
            if (UnitExtensions.HasModifier(EntityManager.LocalHero, "modifier_item_bloodstone_drained")) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castEternalShroud()
        {
            this.item = this.items.eternalShroud;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_eternal_shroud)) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castGhostScepter()
        {
            this.item = this.items.ghostScepter;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_ghost)) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castGuardianGreaves()
        {
            this.item = this.items.guardianGreaves;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_guardian_greaves)) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castShivasGuard()
        {
            this.item = this.items.shivasGuard;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_shivas_guard)) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castSoulRing()
        {
            this.item = this.items.soulRing;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_soul_ring)) return false;
            if (!this.item.CanBeCasted()) return false;

            this.item.Cast(false, false);
            setSleeper();
            return true;
        }
        #endregion

        #region SelfCastItems
        public bool castLotusOrb()
        {
            this.item = this.items.lotusOrb;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_lotus_orb)) return false;
            if (!this.item.CanBeCasted(EntityManager.LocalHero)) return false;

            this.item.Cast(EntityManager.LocalHero, false, false);
            setSleeper();
            return true;
        }
        public bool castGlimmerCape()
        {
            this.item = this.items.glimmerCape;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_glimmer_cape)) return false;
            if (!this.item.CanBeCasted(EntityManager.LocalHero)) return false;

            this.item.Cast(EntityManager.LocalHero, false, false);
            setSleeper();
            return true;
        }
        #endregion

        #region TargetCastItems
        public bool castBloodthorn()
        {
            this.item = this.items.bloodthorn;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_bloodthorn)) return false;

            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castEtheralBlade()
        {
            this.item = this.items.etherealBlade;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_ethereal_blade)) return false;
            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castDagon()
        {
            this.item = this.items.dagon;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_dagon)) return false;
            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castOrchid()
        {
            this.item = this.items.orchid;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_orchid)) return false;

            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castNullifier()
        {
            this.item = this.items.nullifier;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_nullifier)) return false;

            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castRodOfAtos()
        {
            this.item = this.items.rodOfAtos;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_rod_of_atos)) return false;

            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castScytheOfVyse()
        {
            this.item = this.items.scytheOfVyse;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_sheepstick)) return false;
            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        public bool castVeilOfDiscord()
        {
            this.item = this.items.veilOfDiscord;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_veil_of_discord)) return false;
            if (!this.item.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            this.item.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        #endregion
    }
}
