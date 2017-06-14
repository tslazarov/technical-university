using Lipwig.Desktop.Factories;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Lipwig.Desktop.IocModules
{
    public class DesktopModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IModelFactory>().ToFactory();
        }
    }
}
