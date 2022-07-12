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
    class CastItemsAndAbilities
    {
        #region Variaables
        public Items items = new Items();
        public Abilities abilities = new Abilities();
        static public Sleeper sleeper = new Sleeper();

        static private Base item;
        static private Context Context;                
        static private bool comboState = false;        
        static private Hero target = TargetManager.CurrentTarget;
        static private Hero LocalHero = EntityManager.LocalHero;

        #endregion

        public CastItemsAndAbilities(Context context)
        {
            Context = context;            
        }

        public void Dispose()
        {            
        }

        public void updateItemsAndAbilities()
        {
            items.Update();
            abilities.Update();
            target = TargetManager.CurrentTarget;
        }

        private void setSleeper()
        {
            sleeper.Sleep(item.GetAbility().CastPoint * 1000f + 80f + GameManager.AvgPing);
            comboState = true;
        }

        public bool castBlink()
        {            
            item = items.blink;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_blink)) return false;            
            
            if (!item.CanBeCasted()) return false;            
            
            if (Context.PluginMenu.ComboBlinkMode == "To cursor")
                item.Cast(GameManager.MousePosition, false, false);

            if (Context.PluginMenu.ComboBlinkMode == "In radius")
            {
                if (target != null) item.Cast(Vector3Extensions.Extend(target.Position, GameManager.MousePosition, Context.PluginMenu.ComboTargetSelectorRadius), false, false);
                if (target == null) item.Cast(GameManager.MousePosition, false, false);
            }
            setSleeper();
            return true;
        }

        #region Abilities
        public bool castLaser()
        {
            item = abilities.laser;                ;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_laser)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();            
            return true;
        }
        public bool castHeatSeekingMissile()
        {
            item = abilities.heatSeekingMissile;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_heat_seeking_missile)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();            
            return true;
        }
        public bool castDefensiveMatrix()
        {
            item = abilities.defenseMatrix;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_defense_matrix)) return false;
            if (UnitExtensions.HasModifier(LocalHero, "modifier_tinker_defense_matrix")) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(LocalHero, false, false);
            setSleeper();
            return true;
        }
        public bool castWarpGrenade()
        {
            item = abilities.warpGrenade;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_warp_grenade)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castRearm()
        {
            item = abilities.rearm;
            if (!Context.PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_rearm)) return false;
            if (!comboState) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            comboState = false;
            return true;
        }
        #endregion

        #region NoTargetCastItems
        public bool castBloodstone()
        {
            item = items.bloodStone;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_bloodstone)) return false;
            if (UnitExtensions.HasModifier(LocalHero, "modifier_item_bloodstone_drained")) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castEternalShroud()
        {
            item = items.eternalShroud;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_eternal_shroud)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castGhostScepter()
        {
            item = items.ghostScepter;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_ghost)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castGuardianGreaves()
        {
            item = items.guardianGreaves;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_guardian_greaves)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castShivasGuard()
        {
            item = items.shivasGuard;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_shivas_guard)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();
            return true;
        }
        public bool castSoulRing()
        {
            item = items.soulRing;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_soul_ring)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(false, false);
            setSleeper();
            return true;
        }
        #endregion

        #region SelfCastItems
        public bool castLotusOrb()
        {
            item = items.lotusOrb;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_lotus_orb)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(LocalHero, false, false);
            setSleeper();
            return true;
        }
        public bool castGlimmerCape()
        {
            item = items.glimmerCape;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_glimmer_cape)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(LocalHero, false, false);
            setSleeper();
            return true;
        }
        #endregion

        #region TargetCastItems
        public bool castBloodthorn()
        {
            item = items.bloodthorn;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_bloodthorn)) return false;

            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castEtheralBlade()
        {
            item = items.etherealBlade;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_ethereal_blade)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castDagon()
        {
            item = items.dagon;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_dagon)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castOrchid()
        {
            item = items.orchid;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_orchid)) return false;

            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castNullifier()
        {
            item = items.nullifier;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_nullifier)) return false;

            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castRodOfAtos()
        {
            item = items.rodOfAtos;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_rod_of_atos)) return false;

            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castScytheOfVyse()
        {
            item = items.scytheOfVyse;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_sheepstick)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        public bool castVeilOfDiscord()
        {
            item = items.veilOfDiscord;
            if (!Context.PluginMenu.ComboItemsToggler.GetValue(AbilityId.item_veil_of_discord)) return false;
            if (!item.CanBeCasted()) return false;

            item.Cast(target, false, false);
            setSleeper();
            return true;
        }
        #endregion
    }
}
