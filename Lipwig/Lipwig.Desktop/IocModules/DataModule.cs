using Lipwig.Data;
using Lipwig.Data.Assembly;
using Lipwig.Data.Contracts;
using Lipwig.Data.Factories;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Lipwig.Desktop.IocModules
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(x =>
                    x.FromAssemblyContaining<IDataAssembly>()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                );

            this.Rebind<ILipwigContext>().To<LipwigContext>().InThreadScope();
            this.Bind<IUsersFactory>().ToFactory();
            this.Bind<IIncomesFactory>().ToFactory();
            this.Bind<IExpensesFactory>().ToFactory();
        }
    }
}
