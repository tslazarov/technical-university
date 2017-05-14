using Ninject.Extensions.Conventions;
using Ninject.Modules;
using SmarterThanYou.Services.Assembly;

namespace SmarterThanYou.Server.App_Start.NinjectModules
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