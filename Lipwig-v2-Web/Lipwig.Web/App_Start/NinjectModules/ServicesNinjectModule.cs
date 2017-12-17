using Lipwig.Services.Assembly;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Lipwig.Web.App_Start.NinjectModules
{
    public class ServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(x =>
                    x.FromAssemblyContaining<IServicesAssembly>()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                );
        }
    }
}