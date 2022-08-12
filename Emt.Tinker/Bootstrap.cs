using Divine.Entity;
using Divine.Service;
using Divine.Entity.Entities.Units.Heroes.Components;

namespace Emt.Tinker
{
    public class Bootstrap : Bootstrapper
    {   
        protected override void OnActivate()
        {
            if (EntityManager.LocalHero.HeroId == HeroId.npc_dota_hero_tinker)
            {
                Context.Init();
            }            
        }
        protected override void OnDeactivate()
        {               
            Context.Dispose();
        }        
    }
}
