namespace Tinker
{
    class Context
    {   
        public PluginMenu PluginMenu { get; set; }
        public TargetManager TargetManager { get; set; }           
        public Combo Combo { get; set; }
        public Context()
        {
            PluginMenu = new PluginMenu();

            PluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }

        public void Dispose() { }

        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {                        
                TargetManager = new TargetManager(this);                             
                Combo = new Combo(this);
            }
            else
            {   
                TargetManager.Dispose();                
                Combo.Dispose();                
            }
        }
    }
}