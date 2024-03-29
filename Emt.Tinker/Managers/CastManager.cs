﻿using Divine.Entity;
using Divine.Entity.Entities.Abilities;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Abilities.Items;
using Divine.Entity.Entities.Abilities.Items.Components;
using Divine.Entity.Entities.Units;
using Divine.Extensions;
using Divine.Game;
using Divine.Helpers;
using Divine.Numerics;
using System.Collections.Generic;


namespace Emt.Tinker.Managers
{
    static class CastManager
    {
        #region Variables
        static private Ability ability;
        static private Item item;
        static public Sleeper sleeper = new Sleeper();
        static public Sleeper rearmSleeper = new Sleeper();
        static public Sleeper laserSleeper = new Sleeper();
        #endregion

        static public bool castBlink(AbilityId abilityId, float additionalSleepTime = 0f)
        {
            if (sleeper.IsSleeping) return false;

            if (!ItemCanBeCasted(abilityId)) return false;

            if (PluginMenu.ComboBlinkMode == "To cursor") CastItem(GameManager.MousePosition, false, false);

            if (PluginMenu.ComboBlinkMode == "In radius")
            {
                if (TargetManager.currentTarget != null) CastItem(TargetManager.currentTarget.Position.Extend(GameManager.MousePosition, PluginMenu.ComboBlinkModeRadius), false, false);
                if (TargetManager.currentTarget == null) CastItem(GameManager.MousePosition, false, false);
            }

            sleeper.Sleep(item.CastPoint * 1000f + 80f + GameManager.AvgPing + additionalSleepTime);

            return true;
        }

        static public bool castRearm()
        {
            if (rearmSleeper.IsSleeping) return false;
            if (!AbilityCanBeCasted(AbilityId.tinker_rearm)) return false;

            if (!CastAbility(false, false)) return false;
            rearmSleeper.Sleep(ability.CastPoint * 1000f + 80f + GameManager.AvgPing + ability.ChannelTime * 1000f + 1000f);
            if (CastManager.castNeutralItem(AbilityId.item_trickster_cloak)) return true;
            return true;
        }

        static public bool castLaser()
        {
            if (sleeper.IsSleeping) return false;
            if (laserSleeper.IsSleeping) return false;
            if (!AbilityCanBeCasted(AbilityId.tinker_laser)) return false;
            if (!PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_laser)) return false;

            Divine.Entity.Entities.Units.Unit _target = null;
            if (EntityManager.LocalHero.HasAghanimsScepter() && TargetManager.nearestEnemyUnitFromTarget != null)
            {
                _target = TargetManager.nearestEnemyUnitFromTarget;
            }

            if (_target == null && TargetManager.currentTarget.IsReflectingAbilities() && TargetManager.nearestEnemyUnitFromTarget != null)
            {
                _target = TargetManager.nearestEnemyUnitFromTarget;
            }

            if (_target == null) _target = TargetManager.currentTarget;

            if (!CanBeCasted(_target)) return false;
            CastAbility(_target, false, false);
            sleeper.Sleep(ability.CastPoint * 1000f + 80f + GameManager.AvgPing);
            laserSleeper.Sleep(ability.CastPoint * 1000f + 80f + GameManager.AvgPing + ability.ChannelTime + 1000f);

            return true;
        }

        static public bool castHeatSeekingRocket()
        {
            if (sleeper.IsSleeping) return false;
            if (!AbilityCanBeCasted(AbilityId.tinker_heat_seeking_missile)) return false;
            if (!PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_heat_seeking_missile)) return false;

            CastAbility(false, false);
            sleeper.Sleep(ability.CastPoint * 1000f + 80f + GameManager.AvgPing);

            return true;
        }

        static public bool castDefenseMatrix(bool checkSleeper = true)
        {
            if (checkSleeper) if (sleeper.IsSleeping) return false;
            if (!AbilityCanBeCasted(AbilityId.tinker_defense_matrix)) return false;
            if (!PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_defense_matrix)) return false;
            if (EntityManager.LocalHero.HasModifier("modifier_tinker_defense_matrix")) return false;

            CastAbility(EntityManager.LocalHero, false, false);
            sleeper.Sleep(ability.CastPoint * 1000f + 80f + GameManager.AvgPing);

            return true;
        }

        static public bool castWarpGrenade()
        {
            if (sleeper.IsSleeping) return false;
            if (!AbilityCanBeCasted(AbilityId.tinker_warp_grenade, EntityManager.LocalHero)) return false;
            if (!PluginMenu.ComboAbilitiesToggler.GetValue(AbilityId.tinker_warp_grenade)) return false;
            if (!EntityManager.LocalHero.IsInRange(TargetManager.currentTarget, PluginMenu.ComboWarpGrenadeUseRadius)) return false;

            CastAbility(TargetManager.currentTarget, false, false);
            sleeper.Sleep(ability.CastPoint * 1000f + 80f + GameManager.AvgPing);

            return true;
        }

        static public bool castItem(AbilityId abilityId, Unit unit = null, bool searchInMenu = true, bool checkSleeper = true)
        {
            if (checkSleeper) if (sleeper.IsSleeping) return false;
            if (searchInMenu) if (!PluginMenu.ComboItems.GetValue((ItemId)abilityId)) return false;

            if (!ItemCanBeCasted(abilityId, unit)) return false;
            if (!IsInMainInventory(item)) return false;

            if (unit != null) CastItem(unit, false, false);
            if (unit == null) CastItem(false, false);

            sleeper.Sleep(item.CastPoint * 1000f + 80f + GameManager.AvgPing);

            return true;
        }

        static public bool castNeutralItem(AbilityId abilityId, Unit unit = null, bool searchInMenu = true)
        {
            if (sleeper.IsSleeping) return false;

            if (searchInMenu) if (!PluginMenu.ComboNeutralItems.GetValue((ItemId)abilityId)) return false;

            if (!ItemCanBeCasted(abilityId, unit)) return false;

            var neutralItem = EntityManager.LocalHero.Inventory.GetItem(ItemSlot.NeutralItemSlot);

            if (neutralItem?.Id != abilityId) return false;

            if (unit != null) CastItem(unit, false, false);
            if (unit == null) CastItem(false, false);

            sleeper.Sleep(item.CastPoint * 1000f + 80f + GameManager.AvgPing);

            return true;
        }

        static public bool castNeutralItem(AbilityId abilityId, Vector3 position, bool searchInMenu = true)
        {
            if (sleeper.IsSleeping) return false;

            if (searchInMenu) if (!PluginMenu.ComboNeutralItems.GetValue((ItemId)abilityId)) return false;

            if (!ItemCanBeCasted(abilityId)) return false;

            var neutralItem = EntityManager.LocalHero.Inventory.GetItem(ItemSlot.NeutralItemSlot);

            if (neutralItem?.Id != abilityId) return false;

            CastItem(position, false, false);

            sleeper.Sleep(item.CastPoint * 1000f + 80f + GameManager.AvgPing);

            return true;
        }

        static bool IsInMainInventory(Item item)
        {
            IEnumerable<Item> itemsInInventory = EntityManager.LocalHero.Inventory.MainItems;
            foreach (Item i in itemsInInventory)
            {
                if (item.Id == i.Id) return true;
            }

            return false;
        }

        #region CanBeCasted
        static bool ItemCanBeCasted(AbilityId abilityId, Unit unit = null)
        {
            item = UnitExtensions.GetItemById(EntityManager.LocalHero, abilityId);
            if (item == null) return false;
            if (item.AbilityState != AbilityState.Ready) return false;
            if (item.Cooldown > 0f) return false;
            if (item.Level == 0) return false;
            if (EntityManager.LocalHero.Mana < item.ManaCost) return false;
            return CanBeCasted(unit);
        }
        static bool AbilityCanBeCasted(AbilityId abilityId, Unit unit = null)
        {
            ability = UnitExtensions.GetAbilityById(EntityManager.LocalHero, abilityId);
            if (ability == null) return false;
            if (ability.Cooldown > 0f) return false;
            if (ability.Level == 0) return false;
            if (EntityManager.LocalHero.Mana < ability.ManaCost) return false;
            return CanBeCasted(unit);
        }

        static private bool CanBeCasted(Unit unit = null)
        {
            if (EntityManager.LocalHero == null) return false;
            if (!EntityManager.LocalHero.IsAlive) return false;
            if (EntityManager.LocalHero.IsMuted()) return false;
            if (EntityManager.LocalHero.IsSilenced()) return false;
            if (EntityManager.LocalHero.IsStunned()) return false;
            if (EntityManager.LocalHero.IsChanneling()) return false;

            if (unit != null)
            {
                if (!unit.IsVisible) return false;
                if (!unit.IsAlive) return false;
                if (unit.IsMagicImmune()) return false;
                if (unit.IsInvulnerable()) return false;
            }

            return true;
        }
        #endregion

        #region Cast
        static bool CastAbility(Vector3 position, bool queue = false, bool bypass = false)
        {
            return ability.Cast(position, queue, bypass);
        }
        static bool CastAbility(Unit unit, bool queue = false, bool bypass = false)
        {
            return ability.Cast(unit, queue, bypass);
        }
        static bool CastAbility(bool queue = false, bool bypass = false)
        {
            return ability.Cast(queue, bypass);
        }
        static bool CastItem(Vector3 position, bool queue = false, bool bypass = false)
        {
            return item.Cast(position, queue, bypass);
        }
        static bool CastItem(Unit unit, bool queue = false, bool bypass = false)
        {
            return item.Cast(unit, queue, bypass);
        }
        static bool CastItem(bool queue = false, bool bypass = false)
        {
            return item.Cast(queue, bypass);
        }
        #endregion

    }
}
