using Divine.Entity.Entities.Abilities.Components;
using Divine.Menu;
using Divine.Menu.Items;
using System;

namespace EMT.Farm
{
    internal class PluginMenu : IDisposable
    {
        private readonly Menu rootMenu;

        public readonly MenuSwitcher pluginStatus;
        public readonly MenuHoldKey modeFarm;
        public readonly MenuSlider additionalRange;

        public PluginMenu()
        {
            this.rootMenu = MenuManager.CreateRootMenu("EMT.Farm")
                .SetAbilityImage(AbilityId.alchemist_goblins_greed)
                .SetTooltip("V0.1");

            this.pluginStatus = rootMenu.CreateSwitcher("On/Off");
            this.modeFarm = this.rootMenu.CreateHoldKey("Farm key", Divine.Input.Key.B).SetTooltip("Hold this key to auto Farm"); ;
            this.additionalRange = this.rootMenu.CreateSlider("Additional tracking range", 200, 0, 300);
        }

        public void Dispose()
        {
        }

    }
}
