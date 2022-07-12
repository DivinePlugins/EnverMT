using Divine.Entity;
using Divine.Extensions;
using Divine.Update;
using Divine.Entity.Entities.Units.Heroes;
using System;
using System.Linq;
using Divine.Game;
using Divine.Numerics;

namespace Tinker
{
    class TargetManager : IDisposable
    {
        static public Hero CurrentTarget { get; set; }
        private Context Context;
        private int targerSearchBaseRadius = 700;
        private int targerSearchAdditionalRadius;

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
        private Hero GetNearestEnemy(Vector3 startPosition, int targerSearchRadius)
        {
            Hero hero;
            hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (hero != null) return hero;
            return null;
        }
        private void TargetUpdater()
        {
            if (Context.PluginMenu.ComboTargetSelectorMode == "Nearest to Hero")
            {   
                CurrentTarget = GetNearestEnemy(EntityManager.LocalHero.Position, this.targerSearchBaseRadius + this.calculateAdditionalTargerSearchRadius());
            }

            if (Context.PluginMenu.ComboTargetSelectorMode == "In radius of Cursor")
            {   
                CurrentTarget = GetNearestEnemy(GameManager.MousePosition, Context.PluginMenu.ComboTargetSelectorRadius);
            }
        }
        private int calculateAdditionalTargerSearchRadius()
        {
            this.targerSearchAdditionalRadius = 0;
            
            if (Context.CastItemsAndAbilities.items.lens.CanBeCasted() || Context.CastItemsAndAbilities.items.octarine.CanBeCasted()) this.targerSearchAdditionalRadius += 200;
            if (EntityManager.LocalHero.HasAnyModifiers("modifier_item_ultimate_scepter", "modifier_item_ultimate_scepter_consumed", "modifier_wisp_tether_scepter")) this.targerSearchAdditionalRadius += 200;            
            return this.targerSearchAdditionalRadius;
    }
        public void Dispose()
        {
            Context.PluginMenu.ComboKey.ValueChanged -= ComboKey_ValueChanged;
            UpdateManager.DestroyIngameUpdate(TargetUpdater);            
        }
    }
}
