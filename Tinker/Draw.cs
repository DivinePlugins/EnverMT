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
            if (TargetManager.CurrentTarget == null) return;
            if (!Context.PluginMenu.ComboDrawLineToTarget) return;
            this.DrawTargetParticle();
        }

        private void DrawTargetParticle()
        {
            RendererManager.DrawLine(RendererManager.WorldToScreen(EntityManager.LocalHero.Position),
                RendererManager.WorldToScreen(TargetManager.CurrentTarget.Position),Color.Aqua,3f);                        
        }

        public void Dispose ()
        {
            RendererManager.Draw -= this.onDraw;
            UpdateManager.DestroyIngameUpdate(this.onDraw);
        }
    }
}
