namespace Emt.Tinker
{
    using System;

    class Context
    {
        public static Modes.Combo combo;
        public static Modes.SpamRocket spamRocket;
        public static Modes.AutoShiva autoShiva;

        public static void InitMenu()
        {
            PluginMenu.Activate();
        }

        static public bool Init()
        {
            PluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;
            return true;
        }

        static private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherChangedEventArgs e)
        {
            if (e.Value)
            {
                Managers.TargetManager.Activate();
                Render.Draw.Activate();
                Managers.FailSafeManager.Activate();
                Managers.PreCastManager.Activate();
                combo = new Modes.Combo();
                spamRocket = new Modes.SpamRocket();
                autoShiva = new Modes.AutoShiva();
            }
            else
            {
                Managers.TargetManager.Dispose();
                Render.Draw.Dispose();
                Managers.FailSafeManager.Dispose();
                Managers.PreCastManager.Dispose();
                combo.Dispose();
                spamRocket.Dispose();
                autoShiva.Dispose();
            }
        }

        static public void Dispose()
        {
            PluginMenu.PluginStatus.ValueChanged -= PluginStatus_ValueChanged;
        }
    }
}