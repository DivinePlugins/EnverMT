using Divine.Menu;
using Divine.Menu.Items;
using Divine.Entity.Entities.Abilities.Components;

namespace AmuletAutoUse
{
    public class PluginMenu : IDisposable
    {
        public readonly Menu RootMenu;
        public readonly MenuSwitcher PluginStatus;
        public static MenuSlider Cooldown;        

        public PluginMenu()
        {
            RootMenu = MenuManager.CreateRootMenu("Emt.AmuletAutoUse")
                .SetAbilityImage(AbilityId.item_shadow_amulet)
                .SetTooltip("V1.0");

            PluginStatus = RootMenu.CreateSwitcher("On/Off");

            Cooldown = RootMenu.CreateSlider("Use when remain seconds to end invisibility", 2, 0, 13);
        }

        public void Dispose()
        {
        }
    }
}