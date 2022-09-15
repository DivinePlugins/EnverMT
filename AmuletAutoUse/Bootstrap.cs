using Divine.Service;

namespace AmuletAutoUse
{
    public class Bootstrap : Bootstrapper
    {
        public PluginMenu pluginMenu;
        private SpamAmulet spamAmulet;
        protected override void OnActivate()
        {
            pluginMenu = new();
            pluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }
        protected override void OnDeactivate()
        {
            pluginMenu = null;
            spamAmulet.Dispose();
        }
        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                spamAmulet = new();
            }
            else
            {
                spamAmulet.Dispose();
            }
        }
    }
}
