using Lipwig.Services;
using Lipwig.Services.Assembly;
using Lipwig.Services.Contracts;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Lipwig.Desktop.IocModules
{
    public class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(x =>
                    x.FromAssemblyContaining<IServicesAssembly>()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                );

            this.Rebind<ICurrenciesService>().To<CurrenciesService>();
        }
    }
}
