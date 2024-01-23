using Divine.Entity;
using Divine.Entity.Entities.Units.Heroes.Components;
using Divine.Service;

namespace Emt.Tinker
{
    public class Bootstrap : Bootstrapper
    {
        private bool context;

        protected override void OnMainActivate()
        {
            Context.InitMenu();
        }

        protected override void OnActivate()
        {
            if (EntityManager.LocalHero.Id == HeroId.npc_dota_hero_tinker)
            {
                context = Context.Init();
            }
        }

        protected override void OnDeactivate()
        {
            if (context)
            {
                Context.Dispose();
                context = false;
            }
        }
    }
}
