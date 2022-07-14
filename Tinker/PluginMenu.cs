using Divine.Entity.Entities.Units.Heroes.Components;
using Divine.Input;
using Divine.Menu;
using Divine.Menu.Items;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Menu.EventArgs;


namespace Tinker
{
    internal class PluginMenu
    {
        public readonly MenuSwitcher PluginStatus;
        public readonly MenuHoldKey ComboKey;
        public readonly MenuSelector ComboTargetSelectorMode;
        public readonly MenuSlider ComboTargetSelectorRadius;
        public readonly MenuItemToggler ComboItemsToggler;
        public readonly MenuAbilityToggler ComboAbilitiesToggler;
        public readonly MenuSlider ComboWarpGrenadeUseRadius;
        public readonly MenuSwitcher ComboSmartLaser;

        public readonly MenuSelector ComboBlinkMode;
        public readonly MenuSlider ComboBlinkModeRadius;

        public readonly MenuSwitcher ComboAutoShiva;
        public readonly MenuSlider ComboAutoShivaRadius;

        public readonly MenuSelector ComboLinkenBreakerMode;
        public readonly MenuSwitcher ComboDrawLineToTarget;
        public readonly MenuSwitcher ComboLockTarget;

        private readonly Menu RootMenu;

        public PluginMenu()
        {
            RootMenu = MenuManager.CreateRootMenu("Tinker")
                .SetHeroImage(HeroId.npc_dota_hero_tinker)
                .SetTooltip("V1.0.7 BETA");

            PluginStatus = RootMenu.CreateSwitcher("On/Off");
            
            Menu menu = RootMenu.CreateMenu("Combo").SetAbilityImage(AbilityId.tinker_rearm,MenuAbilityImageType.Default);
            ComboKey = menu.CreateHoldKey("Combo Key", Key.None).SetTooltip("Hold this key to use Combo");

            this.ComboTargetSelectorMode = menu.CreateSelector("Target Selector Mode", Data.Menu.TargetSelectorModes);
            this.ComboTargetSelectorRadius = menu.CreateSlider("Radius", 600, 100, 1000).SetTooltip("Search enemy in radius of");
            
            this.ComboItemsToggler = menu.CreateItemToggler("Items", Data.Menu.ComboItems, false, true).SetTooltip("Items which will be used in Combo");
            this.ComboAbilitiesToggler = menu.CreateAbilityToggler("Abilities", Data.Menu.ComboAbilities, false).SetTooltip("Warp grenade will be used, only if enemy very close to Hero");
            this.ComboWarpGrenadeUseRadius = menu.CreateSlider("Warp Grenade use distanace", 200, 100, 600).SetAbilityImage(AbilityId.tinker_warp_grenade, MenuAbilityImageType.Default).SetTooltip("Warp grenade will be used, if Enemy closer than this distance");
            this.ComboSmartLaser = menu.CreateSwitcher("Smart Laser On/Off").SetAbilityImage(AbilityId.tinker_laser, MenuAbilityImageType.Default).SetTooltip("If target has Lotus or Antimage with shied, Laser will try to use on possible nearest unit");

            this.ComboBlinkMode = menu.CreateSelector("Blink Mode", Data.Menu.ComboBlinkModes).SetAbilityImage(AbilityId.item_blink, MenuAbilityImageType.Default).SetTooltip("Recommended to use {to Cursor}");
            this.ComboBlinkModeRadius = menu.CreateSlider("Safe Blink radius", 600, 100, 1000).SetTooltip("Radius of safe zone");
            
            this.ComboLinkenBreakerMode = menu.CreateSelector("Linken`s Breaker Mode", Data.Menu.LinkenBreakerModes).SetAbilityImage(AbilityId.item_sphere, MenuAbilityImageType.Default);

            this.ComboDrawLineToTarget = menu.CreateSwitcher("Draw line to Target");
            this.ComboLockTarget = menu.CreateSwitcher("Lock Target during Combo").SetTooltip("Target locked while Combo key holds");

            this.ComboAutoShiva = menu.CreateSwitcher("Auto Shiva").SetAbilityImage(AbilityId.item_shivas_guard, MenuAbilityImageType.Default);
            this.ComboAutoShivaRadius = menu.CreateSlider("Distance to Enemy for activating Auto Shiva", 900, 300, 2000);

            this.ComboTargetSelectorMode.ValueChanged += new MenuSelector.SelectorEventHandler(this.ComboTargetSelectorMode_ValueChanged);
            this.ComboBlinkMode.ValueChanged += new MenuSelector.SelectorEventHandler(this.ComboBlinkMode_ValueChanged);
        }

        private void ComboTargetSelectorMode_ValueChanged(MenuSelector selector, SelectorEventArgs e)
        {
            if (e.NewValue == "In radius of Cursor")
            {                                
                this.ComboTargetSelectorMode.SetTooltip("Enemy will be searched in range of cursor");
                this.ComboTargetSelectorRadius.IsHidden = false;
                return;
            }
            if (e.NewValue == "Nearest to Hero")
            {
                this.ComboTargetSelectorMode.SetTooltip("Enemy will be searched in range is 600 {+200 if has Lens} {+200 if has Scepter} from Hero position");
                this.ComboTargetSelectorRadius.IsHidden = true;
                return;
            }
        }        
        private void ComboBlinkMode_ValueChanged(MenuSelector selector, SelectorEventArgs e)
        {            
            if (e.NewValue == "In radius")
            {
                this.ComboBlinkModeRadius.IsHidden = false;                
                this.ComboBlinkMode.SetTooltip("use Blink to safe position");
                return;
            }
            if (e.NewValue == "To cursor")
            {
                this.ComboBlinkModeRadius.IsHidden = true;
                this.ComboBlinkMode.SetTooltip("use Blink to Cursor position");
                return;
            }
        }

        internal void Dispose()
        {
            this.ComboTargetSelectorMode.ValueChanged -= new MenuSelector.SelectorEventHandler(this.ComboTargetSelectorMode_ValueChanged);
        }
    }
}