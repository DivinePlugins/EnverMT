using Divine.Entity;
using Divine.Extensions;
using Divine.Update;
using Divine.Entity.Entities.Units.Heroes;
using System;
using System.Linq;

namespace Tinker
{
    class TargetManager : IDisposable
    {
        static public Hero CurrentTarget { get; set; }
        private Context Context;
        private static int targerSearchBaseRadius = 700;
        private static int targerSearchAdditionalRadius = 0;

        public TargetManager(Context context)
        {
            Context = context;

            Context.PluginMenu.ComboKey.ValueChanged += ComboKey_ValueChanged;
        }
        private void ComboKey_ValueChanged(Divine.Menu.Items.MenuHoldKey holdKey, Divine.Menu.EventArgs.HoldKeyEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.CreateIngameUpdate(100, TargetUpdater);
            }
            else
            {
                UpdateManager.DestroyIngameUpdate(TargetUpdater);
            }
        }      

        private Hero GetNearestEnemyToLocalHero()
        {
            Hero hero;
            hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) && x.Distance2D(EntityManager.LocalHero) 
                                                            < (targerSearchBaseRadius + targerSearchAdditionalRadius))
                                           .OrderBy(x => x.Distance2D(EntityManager.LocalHero)).FirstOrDefault();            
            if (hero != null) return hero;
            return null;            
        }
        private void TargetUpdater()
        {
            this.calculateAdditionalTargerSearchRadius();
            CurrentTarget = GetNearestEnemyToLocalHero();             
        }

        private void calculateAdditionalTargerSearchRadius()
        {
            targerSearchAdditionalRadius = 0;
            if (Context.Combo.items.lens.CanBeCasted() || Context.Combo.items.octarine.CanBeCasted()) targerSearchAdditionalRadius += 200;
            if (EntityManager.LocalHero.HasAnyModifiers("modifier_item_ultimate_scepter", "modifier_item_ultimate_scepter_consumed", "modifier_wisp_tether_scepter")) targerSearchAdditionalRadius += 200;            
        }
        public void Dispose()
        {
            Context.PluginMenu.ComboKey.ValueChanged -= ComboKey_ValueChanged;
            UpdateManager.DestroyIngameUpdate(TargetUpdater);
            //todo
        }
    }
}
