using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Divine.Renderer;
using Divine.Entity;
using Divine.Numerics;
using Divine.Particle.Particles;
using Divine.Particle;
using Divine.Update;
using Divine.Entity.Entities.Units.Heroes;

namespace Tinker
{
    internal class Draw
    {
        private Context Context;        
        public Draw(Context context)
        {
            Context = context;          
            RendererManager.Draw += onDraw;
        }

        private void onDraw()
        {
            if (TargetManager.CurrentTarget != null && Context.PluginMenu.ComboDrawLineToTarget)
            {
                ParticleManager.CreateTargetLineParticle("TargetParticle", EntityManager.LocalHero, TargetManager.CurrentTarget.Position, Color.Red);
            } else
            {
                ParticleManager.DestroyParticle("TargetParticle");
            }
        }


        public void Dispose ()
        {
            RendererManager.Draw -= this.onDraw;
            UpdateManager.DestroyIngameUpdate(this.onDraw);
        }
    }
}
