using Divine.Entity;
using Divine.Service;
using Divine.Entity.Entities.Units.Heroes.Components;
using System;
using Divine.Game;

namespace Emt_Tinker
{
    public class Bootstrap : Bootstrapper
    {
        private Context Context;
        protected override void OnActivate()
        {
            if (EntityManager.LocalHero.HeroId == HeroId.npc_dota_hero_tinker)
            {
                this.Context = new Context();
            }            
        }
        protected override void OnDeactivate()
        {
            
            if (this.Context == null)
            {
                return;
            }
            this.Context.Dispose();
        }        
    }
}
