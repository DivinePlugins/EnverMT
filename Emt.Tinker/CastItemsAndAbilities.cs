using System;
using System.Collections.Generic;
using Divine.Helpers;
using Divine.Extensions;
using Divine.Entity;
using Divine.Game;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Units;

namespace Emt_Tinker
{
    class CastItemsAndAbilities
    {
        #region Variables
        
        static public Sleeper sleeper = new Sleeper();
        static public Sleeper rearmSleeper = new Sleeper();
        private Context Context;                        
        private float sleepTime;        
        #endregion

        public CastItemsAndAbilities(Context context)
        {
            Context = context;            
        }       
        

        private void setSleeper()
        {   
            if (Context.abilityManager.selectedAbilItem == Managers.AbilityManager.EnumAbilityOrItem.Ability)
            {
                sleeper.Sleep(Context.abilityManager.ability.CastPoint * 1000f + 80f + GameManager.AvgPing);
                sleepTime = Context.abilityManager.ability.CastPoint * 1000f + (float)80f + (float)GameManager.AvgPing;
            } else
            {
                sleeper.Sleep(Context.abilityManager.item.CastPoint * 1000f + 80f + GameManager.AvgPing);
                sleepTime = Context.abilityManager.item.CastPoint * 1000f + (float)80f + (float)GameManager.AvgPing;
            }
            
            //Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff") + " - sleeper Set: " + sleepTime);                                    
        }
        private void setRearmSleeper()
        {
            rearmSleeper.Sleep(Context.abilityManager.item.CastPoint * 1000f + 80f + GameManager.AvgPing + Context.abilityManager.item.ChannelTime*1000f);
        }
        
        public bool castBlink()
        {            
            if (Context.abilityManager.abilityDictionary.ContainsKey(AbilityId.item_blink)) Context.abilityManager.SetItem(AbilityId.item_blink);
            if (Context.abilityManager.abilityDictionary.ContainsKey(AbilityId.item_swift_blink)) Context.abilityManager.SetItem(AbilityId.item_swift_blink);
            if (Context.abilityManager.abilityDictionary.ContainsKey(AbilityId.item_overwhelming_blink)) Context.abilityManager.SetItem(AbilityId.item_overwhelming_blink);
            if (Context.abilityManager.abilityDictionary.ContainsKey(AbilityId.item_arcane_blink)) Context.abilityManager.SetItem(AbilityId.item_arcane_blink);
            
            if (!Context.abilityManager.CanBeCasted()) return false;
            

            if (Context.PluginMenu.ComboBlinkMode == "To cursor")
                Context.abilityManager.Cast(GameManager.MousePosition, false, false);

            if (Context.PluginMenu.ComboBlinkMode == "In radius")
            {
                if (Context.TargetManager.currentTarget != null) Context.abilityManager.Cast(Vector3Extensions.Extend(Context.TargetManager.currentTarget.Position, GameManager.MousePosition, Context.PluginMenu.ComboBlinkModeRadius), false, false);
                if (Context.TargetManager.currentTarget == null) Context.abilityManager.Cast(GameManager.MousePosition, false, false);
            }           

            if (Context.TargetManager.currentTarget != null)
            {
                this.setSleeper();
            } else
            {
                sleeper.Sleep(Context.abilityManager.ability.CastPoint * 1000f + 400f + GameManager.AvgPing);
            }                        
            return true;
        }       
        
        #region Abilities
        
        public bool castLaser()
        {
            if (sleeper.Sleeping) return false;

            Context.abilityManager.SetAbility(AbilityId.tinker_laser);
            if (!Context.abilityManager.CanBeCasted()) return false;            

            
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
            
            if (!Context.abilityManager.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            Context.abilityManager.Cast(Context.TargetManager.currentTarget, false, false);    
            
            setSleeper();
            return true;                        
        }
        private bool smartLaser(Unit unitNearestToTarget)
        {            
            if (!Context.abilityManager.CanBeCasted(unitNearestToTarget)) return false;
            

            Context.abilityManager.Cast(unitNearestToTarget, false, false);            
            setSleeper();            
            return true;
        }
        
        public bool castHeatSeekingMissile()
        {
            Context.abilityManager.SetAbility(AbilityId.tinker_heat_seeking_missile);
            
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_heat_seeking_missile)) return false;
            if (!Context.abilityManager.CanBeCasted()) return false;

            Context.abilityManager.Cast(false, false);
            setSleeper();            
            return true;
        }
        
        public bool castDefensiveMatrix()
        {
            Context.abilityManager.SetAbility(AbilityId.tinker_defense_matrix);
            
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_defense_matrix)) return false;            
            if (UnitExtensions.HasModifier(EntityManager.LocalHero, "modifier_tinker_defense_matrix")) return false;            
            if (!Context.abilityManager.CanBeCasted()) return false;


            Context.abilityManager.Cast(EntityManager.LocalHero, false, false);            
            sleeper.Sleep(Context.abilityManager.ability.CastPoint * 1000f + 80f + GameManager.AvgPing);            

            return true;
        }
        public bool castWarpGrenade()
        {
            Context.abilityManager.SetAbility(AbilityId.tinker_warp_grenade);
            
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_warp_grenade)) return false;            
            if (!EntityManager.LocalHero.IsInRange(Context.TargetManager.currentTarget, Context.PluginMenu.ComboWarpGrenadeUseRadius)) return false;            
            if (!Context.abilityManager.CanBeCasted(Context.TargetManager.currentTarget)) return false;

            Context.abilityManager.Cast(Context.TargetManager.currentTarget, false, false);
            setSleeper();
            return true;
        }
        
        public bool castRearm()
        {
            if (CastItemsAndAbilities.sleeper.Sleeping) return false;            
            
            if (rearmSleeper.Sleeping) return false;
            
            Context.abilityManager.SetAbility(AbilityId.tinker_rearm);
            if (!Context.abilityManager.CanBeCasted()) return false;

            if (Context.abilityManager.Cast(false, false))
            {
                this.setRearmSleeper();                
            }            
            return true;
        }
      
        #endregion


        public bool castNoTargetCastItems()
        {
            foreach (var item in Data.Menu.ComboNoTargetItems)
            {
                if (!item.Value) continue;

                Context.abilityManager.SetItem(item.Key);                
                if (Context.abilityManager.CanBeCasted()) if (Context.abilityManager.Cast()) return true;
            }
            return false;
        }

        public bool castSelfCastItems()
        {
            foreach (var item in Data.Menu.ComboSelfCastItems)
            {
                if (!item.Value) continue;

                Context.abilityManager.SetItem(item.Key);
                if (Context.abilityManager.CanBeCasted()) if (Context.abilityManager.Cast(EntityManager.LocalHero)) return true;
            }
            return false;
        }

        public bool castTargetCastItems()
        {
            foreach (var item in Data.Menu.ComboTargetItems)
            {
                if (!item.Value) continue;

                Context.abilityManager.SetItem(item.Key);
                if (Context.abilityManager.CanBeCasted()) if (Context.abilityManager.Cast(Context.TargetManager.currentTarget)) return true;
            }
            return false;
        }

        public bool castTargetDagonCastItems()
        {
            foreach (var item in Data.Menu.ComboTargetItemsDagons)
            {
                if (!item.Value) continue;

                Context.abilityManager.SetItem(item.Key);
                if (Context.abilityManager.CanBeCasted()) if (Context.abilityManager.Cast(Context.TargetManager.currentTarget)) return true;
            }
            return false;
        }
    }
}
