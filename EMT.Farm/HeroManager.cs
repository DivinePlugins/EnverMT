using Divine.Update;

namespace EMT.Farm
{
    internal class HeroManager : IDisposable
    {
        private Context context;
        public HeroManager(Context context)
        {
            this.context = context;
            this.context.pluginMenu.pluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }
        public void Dispose()
        {
            this.context.pluginMenu.pluginStatus.ValueChanged -= PluginStatus_ValueChanged;
            UpdateManager.DestroyIngameUpdate(Update);
        }

        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.CreateIngameUpdate(Update);
            }
            else
            {
                UpdateManager.DestroyIngameUpdate(Update);
            }
        }
        private void Update()
        {

        }
    }
}
