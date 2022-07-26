using Divine.Entity;
using Divine.Extensions;
using Divine.Update;
using Divine.Entity.Entities.Units.Heroes;
using System;
using System.Linq;
using Divine.Game;
using Divine.Numerics;
using Divine.Entity.Entities.Units;

namespace Emt_Tinker
{
    class TargetManager : IDisposable
    {
        public Hero currentTarget { get; private set; }
        public Unit nearestEnemyUnitFromTarget { get; private set; }
        public Hero farestEnemyHeroFromTarget { get; private set; }
        public Hero targetForRocket { get; private set; }
        public int targetSearchDistance { get; private set; }
        
        
        private Context Context;
        private int targerSearchBaseRadius = 650;
        private int targerSearchAdditionalRadius = 0;
        private int rocketFlyDistance = 2500;

        public TargetManager(Context context)
        {
            Context = context;            
            Context.PluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;
            
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
        public void Dispose()
        {
            UpdateManager.DestroyIngameUpdate(Update);
        }
        private void log()
        {            
            Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff") + " target found: " + Context.TargetManager.currentTarget);
        }
        private void Update()
        {
            this.targetSearchDistance = this.targerSearchBaseRadius + this.calculateAdditionalTargerSearchRadius();
            this.targetForRocket = this.GetNearestEnemyHero(EntityManager.LocalHero.Position, rocketFlyDistance);

            if (Context.PluginMenu.ComboLockTarget
                && Context.Combo.comboKeyHolding
                && this.currentTarget != null
                && this.currentTarget.IsAlive
                && !this.currentTarget.IsInvulnerable()
                && this.currentTarget.IsVisible
                && !this.currentTarget.IsMagicImmune()
                && this.currentTarget.Distance2D(EntityManager.LocalHero) < targetSearchDistance) return;

            if (this.currentTarget == null && this.getTarget(targetSearchDistance) != null)
            {
                CastItemsAndAbilities.sleeper.Reset();
            }            
            this.currentTarget = this.getTarget(targetSearchDistance);

            

            if (this.currentTarget == null)
            {
                this.nearestEnemyUnitFromTarget = null;
                this.farestEnemyHeroFromTarget = null;
                return;
            }
            this.nearestEnemyUnitFromTarget = this.GetNearestEnemyUnitToEnemyTarget(this.currentTarget.Position, 250);
            if (this.nearestEnemyUnitFromTarget == null)
            {
                this.farestEnemyHeroFromTarget = null;
                return;
            }
            this.farestEnemyHeroFromTarget = this.GetFarestEnemyHeroInRadius(this.nearestEnemyUnitFromTarget.Position, 700);
        }
       

        private int calculateAdditionalTargerSearchRadius()
        {
            this.targerSearchAdditionalRadius = (int)EntityManager.LocalHero.BonusCastRange;
            if (EntityManager.LocalHero.HasAghanimsScepter()) this.targerSearchAdditionalRadius += 200;
            return this.targerSearchAdditionalRadius;
        }

        public Hero getTarget(int radius)
        {
            if (Context.PluginMenu.ComboTargetSelectorMode == "Nearest to Hero")
            {
                return this.currentTarget = GetNearestEnemyHero(EntityManager.LocalHero.Position, radius);
            }

            if (Context.PluginMenu.ComboTargetSelectorMode == "In radius of Cursor")
            {
                return this.currentTarget = GetNearestEnemyHero(GameManager.MousePosition, Context.PluginMenu.ComboTargetSelectorRadius);
            }

            if (Context.PluginMenu.ComboTargetSelectorMode == "First In radius of Cursor, then nearest to Hero")
            {
                Hero h;
                h = this.currentTarget = GetNearestEnemyHero(GameManager.MousePosition, Context.PluginMenu.ComboTargetSelectorRadius);
                if (h != null) return h;
                return h = this.currentTarget = GetNearestEnemyHero(EntityManager.LocalHero.Position, radius);
            }
            return null;  
        }
        private Hero GetNearestEnemyHero(Vector3 startPosition, int targerSearchRadius)
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
        private Unit GetNearestEnemyUnitToEnemyTarget(Vector3 startPosition, int targerSearchRadius)
        {            
            Unit unit = EntityManager.GetEntities<Unit>().Where(x => x.IsEnemy(EntityManager.LocalHero) &&
                                                                x.Distance2D(startPosition) < targerSearchRadius &&
                                                                x.IsAlive &&
                                                                x.IsVisible &&
                                                                !x.IsMagicImmune() &&
                                                                !x.IsInvulnerable() &&
                                                                x != this.currentTarget
                                                            )
                                           .OrderBy(x => x.Distance2D(startPosition)).FirstOrDefault();
            if (unit != null) return unit;
            return null;
        }

        private Hero GetFarestEnemyHeroInRadius(Vector3 startPosition, int targerSearchRadius)
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

        
    }
}
