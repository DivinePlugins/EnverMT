using Divine.Renderer;
using Divine.Entity;
using Divine.Numerics;
using Divine.Particle;
using Divine.Entity.Entities.Units;

namespace Tinker
{
    internal class Draw
    {
        private readonly Context Context;
        private Unit _target;
        private Unit _localHero;
        public Draw(Context context)
        {
            Context = context;                      
            Context.PluginMenu.ComboDrawLineToTarget.ValueChanged += ComboDrawLineToTarget_ValueChanged;
        }
        private void ComboDrawLineToTarget_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                RendererManager.Draw += onDraw;
            }
            else
            {
                RendererManager.Draw -= onDraw;
            }
        }

        private void onDraw()
        {
            this._target = Context.TargetManager.currentTarget;
            this._localHero = EntityManager.LocalHero;
            
            if (this._target != null)
            {
                ParticleManager.CreateTargetLineParticle("TargetParticle", this._localHero, this._target.Position, Color.Red);
            } else
            {
                ParticleManager.DestroyParticle("TargetParticle");
            }
        }

        public void Dispose ()
        {
            RendererManager.Draw -= onDraw;
        }
    }
}
