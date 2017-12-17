using Lipwig.Data;
using Lipwig.Data.Assembly;
using Lipwig.Data.Contracts;
using Lipwig.Data.Factories;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Lipwig.Web.App_Start.NinjectModules
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(x =>
                    x.FromAssemblyContaining<IDataAssembly>()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                );

            this.Rebind<ILipwigContext>().To<LipwigContext>().InRequestScope();
            this.Rebind<ILipwigData>().To<LipwigData>();
            this.Bind<IUsersFactory>().ToFactory();
            this.Bind<IExpensesFactory>().ToFactory();
            this.Bind<IIncomesFactory>().ToFactory();
        }
    }
}