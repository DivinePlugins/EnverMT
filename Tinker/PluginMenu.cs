using Divine.Entity.Entities.Units.Heroes.Components;
using Divine.Input;
using Divine.Menu;
using Divine.Menu.Items;

namespace Tinker
{
    internal class PluginMenu
    {
        public MenuSwitcher PluginStatus { get; set; }
        public MenuHoldKey ComboKey { get; set; }
        public MenuItemToggler ComboItems { get; set; }
        public Menu HarassMenu { get; set; }
        public MenuHoldKey HarassKey { get; set; }
        public MenuSelector HarassMode { get; set; }

        private Menu RootMenu;
        public PluginMenu()
        {
            RootMenu = MenuManager.CreateRootMenu("Tinker")
                .SetHeroImage(HeroId.npc_dota_hero_tinker)
                .SetTooltip("V0.5 BETA");

            PluginStatus = RootMenu.CreateSwitcher("On/Off");
            ComboKey = RootMenu.CreateHoldKey("Combo Key", Key.None);            
        }
    }
}