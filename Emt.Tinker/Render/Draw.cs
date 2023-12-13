using Divine.Entity;
using Divine.Entity.Entities.Units;
using Divine.Numerics;
using Divine.Particle;
using Divine.Renderer;

namespace Emt.Tinker.Render
{
    static internal class Draw
    {
        static public void Activate()
        {
            PluginMenu.ComboDrawLineToTarget.ValueChanged += ComboDrawLineToTarget_ValueChanged;
        }
        static private void ComboDrawLineToTarget_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherChangedEventArgs e)
        {
            if (e.Value)
            {
                RendererManager.Draw += onDraw;
            }
            else
            {
                Dispose();
            }
        }

        static private void onDraw()
        {
            Unit _target = Emt.Tinker.Managers.TargetManager.currentTarget;
            Unit _localHero = EntityManager.LocalHero;

            if (_target != null)
            {
                ParticleManager.CreateTargetLineParticle("TargetParticle", _localHero, _target.Position, Color.Red);
            }
            else
            {
                ParticleManager.DestroyParticle("TargetParticle");
            }
        }

        static public void Dispose()
        {
            RendererManager.Draw -= onDraw;
            ParticleManager.DestroyParticle("TargetParticle");
        }
    }
}
