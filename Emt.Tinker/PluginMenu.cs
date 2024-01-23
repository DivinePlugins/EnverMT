using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Units.Heroes.Components;
using Divine.Input;
using Divine.Menu;
using Divine.Menu.EventArgs;
using Divine.Menu.Items;

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
            RootMenu = MenuManager.HeroesMenu.AddMenu("Emt.Tinker")
                .SetImage(HeroId.npc_dota_hero_tinker)
                .SetTooltip("V1.4");

            PluginStatus = RootMenu.AddSwitcher("On/Off");

            Menu menu = RootMenu.AddMenu("Combo").SetImage(AbilityId.tinker_rearm);
            ComboKey = menu.AddHoldKey("Combo Key", Key.None).SetTooltip("Hold this key to use Combo");

            ComboTargetSelectorMode = menu.AddSelector("Target Selector Mode", Data.Menu.TargetSelectorModes);
            ComboTargetSelectorRadius = menu.AddSlider("Radius", 600, 100, 1000).SetTooltip("Search enemy in radius of");

            ComboItems = menu.AddItemToggler("Combo Items", Data.Menu.ComboItems);
            ComboNeutralItems = menu.AddItemToggler("Combo Neutral Items", Data.Menu.ComboNeutralItems);


            ComboAbilitiesToggler = menu.AddAbilityToggler("Abilities", Data.Menu.ComboAbilities).SetTooltip("Warp grenade will be used, only if enemy very close to Hero");
            ComboWarpGrenadeUseRadius = menu.AddSlider("Warp Grenade use distanace", 200, 100, 600).SetImage(AbilityId.tinker_warp_grenade).SetTooltip("Warp grenade will be used, if Enemy closer than this distance");


            ComboBlinkMode = menu.AddSelector("Blink Mode", Data.Menu.ComboBlinkModes).SetImage(AbilityId.item_blink).SetTooltip("Recommended to use {to Cursor}");
            ComboBlinkModeRadius = menu.AddSlider("Safe Blink radius", 600, 100, 1000).SetTooltip("Radius of safe zone");

            ComboLinkenBreakerMode = menu.AddSelector("Linken`s Breaker Mode", Data.Menu.LinkenBreakerModes).SetImage(AbilityId.item_sphere);

            ComboDrawLineToTarget = menu.AddSwitcher("Draw line to Target");
            ComboLockTarget = menu.AddSwitcher("Lock Target during Combo").SetTooltip("Target locked while Combo key holds");

            Menu spamRocket = RootMenu.AddMenu("Spam Rocket").SetImage(AbilityId.tinker_heat_seeking_missile);
            SpamRocketKey = spamRocket.AddHoldKey("Spam Rocket Key", Key.None).SetTooltip("Hold this key to spam Rocket");

            Menu FailSafe = RootMenu.AddMenu("Failsafe").SetImage(AbilityId.tinker_defense_matrix);
            FailSafeSwitcher_Rocket = FailSafe.AddSwitcher("Safe Rocket").SetTooltip("Rocket will be used, if there are enemy in 2500 radius").SetImage(AbilityId.tinker_heat_seeking_missile);
            FailSafeSwitcher_Rearm = FailSafe.AddSwitcher("Safe Rearm").SetTooltip("Rearm will not be used, if nothing to rearm").SetImage(AbilityId.tinker_rearm);
            FailSafeSwitcher_Rearm_ignoreWithShiva = FailSafe.AddSwitcher("Ignore Safe Rearm if Tinker has Shiva").SetImage(AbilityId.item_shivas_guard);

            Menu AutoShiva = RootMenu.AddMenu("AutoShiva").SetImage(AbilityId.item_shivas_guard);
            AutoShivaSwitcher = AutoShiva.AddSwitcher("Use shiva if enemy around");
            AutoShivaRadius = AutoShiva.AddSlider("Use shiva if enemy closer than", 1300, 300, 3000);

            Menu PreCast = RootMenu.AddMenu("Use Items before Skills").SetImage(AbilityId.item_soul_ring);
            PreCastItems = PreCast.AddItemToggler("PreCast Items", Data.Menu.PreCastItems);
            PreCastAbilities = PreCast.AddAbilityToggler("Abilities", Data.Menu.PreCastAbilities);
            PreCastDefenceBeforeKeen = PreCast.AddSwitcher("Use Defence Matrix on Fountain before Keen Teleport");
        }
        static private void SubscribeToChangeEvents()
        {
            ComboTargetSelectorMode.ValueChanged += ComboTargetSelectorMode_ValueChanged;
            ComboBlinkMode.ValueChanged += ComboBlinkMode_ValueChanged;

        }
        static private void ComboTargetSelectorMode_ValueChanged(MenuSelector selector, SelectorChangedEventArgs e)
        {
            if (e.Value == "In radius of Cursor")
            {
                ComboTargetSelectorMode.SetTooltip("Enemy will be searched in range of cursor");
                ComboTargetSelectorRadius.Show();
                return;
            }
            if (e.Value == "Nearest to Hero")
            {
                ComboTargetSelectorMode.SetTooltip("Enemy will be searched in range is 600 {+200 if has Lens} {+200 if has Scepter} from Hero position");
                ComboTargetSelectorRadius.Hide();
                return;
            }
            if (e.Value == "First In radius of Cursor, then nearest to Hero")
            {
                ComboTargetSelectorMode.SetTooltip("");
                ComboTargetSelectorRadius.Show();
                return;
            }
        }
        static private void ComboBlinkMode_ValueChanged(MenuSelector selector, SelectorChangedEventArgs e)
        {
            if (e.Value == "In radius")
            {
                ComboBlinkModeRadius.Show();
                ComboBlinkMode.SetTooltip("use Blink to safe position");
                return;
            }
            if (e.Value == "To cursor")
            {
                ComboBlinkModeRadius.Hide();
                ComboBlinkMode.SetTooltip("use Blink to Cursor position");
                return;
            }
        }

        static internal void Dispose()
        {
            ComboTargetSelectorMode.ValueChanged -= ComboTargetSelectorMode_ValueChanged;
            ComboBlinkMode.ValueChanged -= ComboBlinkMode_ValueChanged;
        }
    }
}