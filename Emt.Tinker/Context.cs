using Emt_Tinker.Managers;

namespace Emt_Tinker
{
    class Context
    {   
        public PluginMenu PluginMenu { get; set; }
        public TargetManager TargetManager { get; set; }           
        public Combo Combo { get; set; }        
        public Draw draw { get; set; }
        public CastItemsAndAbilities CastItemsAndAbilities;
        public Emt_Tinker.Managers.AbilityManager abilityManager;
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
                CastItemsAndAbilities = new CastItemsAndAbilities(this);
                draw = new Draw(this);
                abilityManager = new AbilityManager(this);
            }
            else
            {   
                TargetManager.Dispose();                
                Combo.Dispose();                
                CastItemsAndAbilities = null;
                draw.Dispose();
                abilityManager.Dispose();
            }
        }
    }
}