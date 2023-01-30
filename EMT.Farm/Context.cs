namespace EMT.Farm
{
    internal class Context : IDisposable
    {
        internal readonly PluginMenu pluginMenu;
        internal EmtUnitManager? EmtUnitManager;
        internal HeroManager? heroManager;
        public Context()
        {
            this.pluginMenu = new();
            this.pluginMenu.pluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }

        public void Dispose()
        {
            this.pluginMenu.pluginStatus.ValueChanged -= PluginStatus_ValueChanged;
            this.pluginMenu.Dispose();
        }

        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                this.EmtUnitManager = new(this);
                this.heroManager = new(this);
            }
            else
            {
                this.EmtUnitManager?.Dispose();
                this.heroManager?.Dispose();
            }
        }
    }
}
