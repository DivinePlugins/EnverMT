using Divine.Service;

namespace EMT.Farm
{
    public class Bootstrap : Bootstrapper
    {
        internal Context? context;

        protected override void OnActivate()
        {
            this.context = new Context();
        }
        protected override void OnDeactivate()
        {
            if (this.context != null)
            {
                this.context.Dispose();
                this.context = null;
            }
        }
    }
}