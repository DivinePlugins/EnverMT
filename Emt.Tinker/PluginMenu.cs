﻿using Divine.Entity.Entities.Units.Heroes.Components;
using Divine.Input;
using Divine.Menu;
using Divine.Menu.Items;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Menu.EventArgs;

namespace Emt.Tinker
{
    static internal class PluginMenu
    {
        static public MenuSwitcher PluginStatus;
        static public MenuHoldKey ComboKey;
        static public MenuSelector ComboTargetSelectorMode;
        static public MenuSlider ComboTargetSelectorRadius;

        static public MenuItemToggler ComboItems;
        static public MenuItemToggler ComboNeutralItems;

        static public MenuAbilityToggler ComboAbilitiesToggler;
        static public MenuSlider ComboWarpGrenadeUseRadius;

        static public MenuSelector ComboBlinkMode;
        static public MenuSlider ComboBlinkModeRadius;

        static public MenuSelector ComboLinkenBreakerMode;
        static public MenuSwitcher ComboDrawLineToTarget;
        static public MenuSwitcher ComboLockTarget;

        static public MenuHoldKey SpamRocketKey;

        static public MenuSwitcher FailSafeSwitcher_Rocket;
        static public MenuSwitcher FailSafeSwitcher_Rearm;
        static public MenuSwitcher FailSafeSwitcher_Rearm_ignoreWithShiva;

        static public MenuSwitcher AutoShivaSwitcher;
        static public MenuSlider AutoShivaRadius;

        static public MenuItemToggler PreCastItems;
        static public MenuAbilityToggler PreCastAbilities;
        static public MenuSwitcher PreCastDefenceBeforeKeen;

        static private Menu RootMenu;



        static public void Activate()
        {
            CreatePluginMenu();
            SubscribeToChangeEvents();
        }

        static public void CreatePluginMenu()
        {
            RootMenu = MenuManager.CreateRootMenu("Emt.Tinker")
                .SetHeroImage(HeroId.npc_dota_hero_tinker)
                .SetTooltip("V1.4");

            PluginStatus = RootMenu.CreateSwitcher("On/Off");

            Menu menu = RootMenu.CreateMenu("Combo").SetAbilityImage(AbilityId.tinker_rearm, MenuAbilityImageType.Default);
            ComboKey = menu.CreateHoldKey("Combo Key", Key.None).SetTooltip("Hold this key to use Combo");

            ComboTargetSelectorMode = menu.CreateSelector("Target Selector Mode", Data.Menu.TargetSelectorModes);
            ComboTargetSelectorRadius = menu.CreateSlider("Radius", 600, 100, 1000).SetTooltip("Search enemy in radius of");

            ComboItems = menu.CreateItemToggler("Combo Items", Data.Menu.ComboItems, false, true);
            ComboNeutralItems = menu.CreateItemToggler("Combo Neutral Items", Data.Menu.ComboNeutralItems, false, true);


            ComboAbilitiesToggler = menu.CreateAbilityToggler("Abilities", Data.Menu.ComboAbilities, false).SetTooltip("Warp grenade will be used, only if enemy very close to Hero");
            ComboWarpGrenadeUseRadius = menu.CreateSlider("Warp Grenade use distanace", 200, 100, 600).SetAbilityImage(AbilityId.tinker_warp_grenade, MenuAbilityImageType.Default).SetTooltip("Warp grenade will be used, if Enemy closer than this distance");


            ComboBlinkMode = menu.CreateSelector("Blink Mode", Data.Menu.ComboBlinkModes).SetAbilityImage(AbilityId.item_blink, MenuAbilityImageType.Default).SetTooltip("Recommended to use {to Cursor}");
            ComboBlinkModeRadius = menu.CreateSlider("Safe Blink radius", 600, 100, 1000).SetTooltip("Radius of safe zone");

            ComboLinkenBreakerMode = menu.CreateSelector("Linken`s Breaker Mode", Data.Menu.LinkenBreakerModes).SetAbilityImage(AbilityId.item_sphere, MenuAbilityImageType.Default);

            ComboDrawLineToTarget = menu.CreateSwitcher("Draw line to Target");
            ComboLockTarget = menu.CreateSwitcher("Lock Target during Combo").SetTooltip("Target locked while Combo key holds");

            Menu spamRocket = RootMenu.CreateMenu("Spam Rocket").SetAbilityImage(AbilityId.tinker_heat_seeking_missile, MenuAbilityImageType.Default);
            SpamRocketKey = spamRocket.CreateHoldKey("Spam Rocket Key", Key.None).SetTooltip("Hold this key to spam Rocket");

            Menu FailSafe = RootMenu.CreateMenu("Failsafe").SetAbilityImage(AbilityId.tinker_defense_matrix, MenuAbilityImageType.Default);
            FailSafeSwitcher_Rocket = FailSafe.CreateSwitcher("Safe Rocket").SetTooltip("Rocket will be used, if there are enemy in 2500 radius").SetAbilityImage(AbilityId.tinker_heat_seeking_missile, MenuAbilityImageType.Default);
            FailSafeSwitcher_Rearm = FailSafe.CreateSwitcher("Safe Rearm").SetTooltip("Rearm will not be used, if nothing to rearm").SetAbilityImage(AbilityId.tinker_rearm, MenuAbilityImageType.Default);
            FailSafeSwitcher_Rearm_ignoreWithShiva = FailSafe.CreateSwitcher("Ignore Safe Rearm if Tinker has Shiva").SetAbilityImage(AbilityId.item_shivas_guard, MenuAbilityImageType.Default);

            Menu AutoShiva = RootMenu.CreateMenu("AutoShiva").SetAbilityImage(AbilityId.item_shivas_guard, MenuAbilityImageType.Default);
            AutoShivaSwitcher = AutoShiva.CreateSwitcher("Use shiva if enemy around");
            AutoShivaRadius = AutoShiva.CreateSlider("Use shiva if enemy closer than", 1300, 300, 3000);

            Menu PreCast = RootMenu.CreateMenu("Use Items before Skills").SetAbilityImage(AbilityId.item_soul_ring, MenuAbilityImageType.Default);
            PreCastItems = PreCast.CreateItemToggler("PreCast Items", Data.Menu.PreCastItems, false, true);
            PreCastAbilities = PreCast.CreateAbilityToggler("Abilities", Data.Menu.PreCastAbilities, false);
            PreCastDefenceBeforeKeen = PreCast.CreateSwitcher("Use Defence Matrix on Fountain before Keen Teleport");
        }
        static private void SubscribeToChangeEvents()
        {
            ComboTargetSelectorMode.ValueChanged += new MenuSelector.SelectorEventHandler(ComboTargetSelectorMode_ValueChanged);
            ComboBlinkMode.ValueChanged += new MenuSelector.SelectorEventHandler(ComboBlinkMode_ValueChanged);

        }
        static private void ComboTargetSelectorMode_ValueChanged(MenuSelector selector, SelectorEventArgs e)
        {
            if (e.NewValue == "In radius of Cursor")
            {
                ComboTargetSelectorMode.SetTooltip("Enemy will be searched in range of cursor");
                ComboTargetSelectorRadius.IsHidden = false;
                return;
            }
            if (e.NewValue == "Nearest to Hero")
            {
                ComboTargetSelectorMode.SetTooltip("Enemy will be searched in range is 600 {+200 if has Lens} {+200 if has Scepter} from Hero position");
                ComboTargetSelectorRadius.IsHidden = true;
                return;
            }
            if (e.NewValue == "First In radius of Cursor, then nearest to Hero")
            {
                ComboTargetSelectorMode.SetTooltip("");
                ComboTargetSelectorRadius.IsHidden = false;
                return;
            }
        }
        static private void ComboBlinkMode_ValueChanged(MenuSelector selector, SelectorEventArgs e)
        {
            if (e.NewValue == "In radius")
            {
                ComboBlinkModeRadius.IsHidden = false;
                ComboBlinkMode.SetTooltip("use Blink to safe position");
                return;
            }
            if (e.NewValue == "To cursor")
            {
                ComboBlinkModeRadius.IsHidden = true;
                ComboBlinkMode.SetTooltip("use Blink to Cursor position");
                return;
            }
        }

        static internal void Dispose()
        {
            ComboTargetSelectorMode.ValueChanged -= new MenuSelector.SelectorEventHandler(ComboTargetSelectorMode_ValueChanged);
            ComboBlinkMode.ValueChanged -= new MenuSelector.SelectorEventHandler(ComboBlinkMode_ValueChanged);
        }
    }
}