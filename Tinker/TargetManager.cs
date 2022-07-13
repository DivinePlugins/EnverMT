using Divine.Entity;
using Divine.Extensions;
using Divine.Update;
using Divine.Entity.Entities.Units.Heroes;
using System;
using System.Linq;
using Divine.Game;
using Divine.Numerics;
using Divine.Entity.Entities.Units;

namespace Tinker
{
    class TargetManager : IDisposable
    {
        static public Hero CurrentTarget { get; set; }
        private Context Context;
        private int targerSearchBaseRadius = 600;
        private int targerSearchAdditionalRadius;
        private bool comboKeyHolding;

        public TargetManager(Context context)
        {
            Context = context;

            Context.PluginMenu.ComboKey.ValueChanged += ComboKey_ValueChanged;
            //Context.PluginMenu.PluginStatus.ValueChanged += ComboKey_ValueChanged;
            UpdateManager.CreateIngameUpdate(Update);
        }
        private void Update()
        {
            if (Context.PluginMenu.ComboLockTarget && this.comboKeyHolding)
            {
                if (TargetManager.CurrentTarget != null && this.GetNearestEnemyHero(EntityManager.LocalHero.Position, this.targerSearchBaseRadius + this.calculateAdditionalTargerSearchRadius()) ==null) return;
            }
            this.TargetUpdater();
        }
        private void ComboKey_ValueChanged(Divine.Menu.Items.MenuHoldKey holdKey, Divine.Menu.EventArgs.HoldKeyEventArgs e)
        {
            if (e.Value)
            {
                //UpdateManager.CreateIngameUpdate(100, TargetUpdater);
                this.comboKeyHolding = true;
            }
            else
            {
                //UpdateManager.DestroyIngameUpdate(TargetUpdater);
                this.comboKeyHolding = false;
            }
        }

        private int calculateAdditionalTargerSearchRadius()
        {
            this.targerSearchAdditionalRadius = (int)EntityManager.LocalHero.BonusCastRange;

            if (EntityManager.LocalHero.HasAghanimsScepter()) this.targerSearchAdditionalRadius += 200;
            return this.targerSearchAdditionalRadius;
        }

        public void TargetUpdater()
        {
            if (Context.PluginMenu.ComboTargetSelectorMode == "Nearest to Hero")
            {
                CurrentTarget = GetNearestEnemyHero(EntityManager.LocalHero.Position, this.targerSearchBaseRadius + this.calculateAdditionalTargerSearchRadius());
            }

            if (Context.PluginMenu.ComboTargetSelectorMode == "In radius of Cursor")
            {
                CurrentTarget = GetNearestEnemyHero(GameManager.MousePosition, Context.PluginMenu.ComboTargetSelectorRadius);
            }
        }

        public Hero GetNearestEnemyHero(Vector3 startPosition, int targerSearchRadius)
        {
            Hero hero;
            hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                !x.IsIllusion                                                                
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (hero != null) return hero;
            return null;
        }

        public Hero GetFarestEnemyHeroInRadius(Vector3 startPosition, int targerSearchRadius)
        {
            Hero hero;
            hero = EntityManager.GetEntities<Hero>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                !x.IsIllusion
                                                            )
                                           .OrderByDescending(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (hero != null) return hero;
            return null;
        }

        public Unit GetNearestEnemyUnitToEnemyTarget(Vector3 startPosition, int targerSearchRadius)
        {            
            Unit unit = EntityManager.GetEntities<Unit>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                x.Position != startPosition
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (unit != null) return unit;
            return null;
        }

        public void Dispose()
        {
            //Context.PluginMenu.ComboKey.ValueChanged -= ComboKey_ValueChanged;
            UpdateManager.DestroyIngameUpdate(TargetUpdater);            
        }
    }
}
