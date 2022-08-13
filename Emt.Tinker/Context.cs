namespace Emt.Tinker
{
    class Context
    {
        public static Modes.Combo combo;
        public static Modes.SpamRocket spamRocket;
        public static Modes.AutoShiva autoShiva;
        static public void Init()
        {
            PluginMenu.Activate();
            PluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }

        static private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                Managers.TargetManager.Activate();
                Render.Draw.Activate();
                Managers.FailSafeManager.Activate();
                combo = new Modes.Combo();
                spamRocket = new Modes.SpamRocket();
                autoShiva = new Modes.AutoShiva();
            }
            else
            {
                DisposeInstances();
            }
        }

        static public void Dispose()
        {
            DisposeInstances();
            PluginMenu.PluginStatus.ValueChanged -= PluginStatus_ValueChanged;
        }

        static private void DisposeInstances()
        {
            Managers.TargetManager.Dispose();
            Render.Draw.Dispose();
            Managers.FailSafeManager.Dispose();
            combo.Dispose();
            spamRocket.Dispose();
            autoShiva.Dispose();
        }
    }
}