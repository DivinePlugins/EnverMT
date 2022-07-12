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
        public readonly MenuSelector ComboBlinkMode;
        public readonly MenuSelector ComboLinkenBreakerMode;

        private Menu RootMenu;

        public PluginMenu()
        {
            RootMenu = MenuManager.CreateRootMenu("Tinker")
                .SetHeroImage(HeroId.npc_dota_hero_tinker)
                .SetTooltip("V0.6 BETA");

            PluginStatus = RootMenu.CreateSwitcher("On/Off");
            ComboKey = RootMenu.CreateHoldKey("Combo Key", Key.None).SetTooltip("Hold this key to use Combo");
            Menu menu = RootMenu.CreateMenu("Combo").SetAbilityImage(AbilityId.tinker_laser,MenuAbilityImageType.Default);
            this.ComboTargetSelectorMode = menu.CreateSelector("Target Selector Mode", Data.Menu.TargetSelectorModes);
            this.ComboTargetSelectorRadius = menu.CreateSlider("Radius", 200, 100, 1000);
            this.ComboItemsToggler = menu.CreateItemToggler("Items", Data.Menu.ComboItems, false, true).SetTooltip("Items which will be used in Combo");
            this.ComboAbilitiesToggler = menu.CreateAbilityToggler("Abilities", Data.Menu.ComboAbilities, false).SetTooltip("Warp grenade will be used, only if enemy very close to Hero");
            this.ComboBlinkMode = menu.CreateSelector("Blink Mode", Data.Menu.ComboBlinkModes).SetAbilityImage(AbilityId.item_blink, MenuAbilityImageType.Default)
                .SetTooltip("Recommended to use {to Cursor}");
            this.ComboLinkenBreakerMode = menu.CreateSelector("Linken`s Breaker Mode", Data.Menu.LinkenBreakerModes).SetAbilityImage(AbilityId.item_sphere, MenuAbilityImageType.Default);
            this.ComboTargetSelectorMode.ValueChanged += new MenuSelector.SelectorEventHandler(this.ComboTargetSelectorMode_ValueChanged);
        }

        private void ComboTargetSelectorMode_ValueChanged(MenuSelector selector, SelectorEventArgs e)
        {
            if (e.NewValue != "In radius of Cursor")
            {
                this.ComboTargetSelectorRadius.IsHidden = true;                
                this.ComboTargetSelectorMode.SetTooltip("default range is 600 {+200 if has Lens} {+200 if has Scepter}");
                return;
            }
            this.ComboTargetSelectorRadius.IsHidden = false;
            this.ComboTargetSelectorMode.SetTooltip("");

        }

        internal void Dispose()
        {
            this.ComboTargetSelectorMode.ValueChanged -= new MenuSelector.SelectorEventHandler(this.ComboTargetSelectorMode_ValueChanged);
        }
    }
}