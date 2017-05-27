using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject.Web.Common;
using SmarterThanYou.Data;
using SmarterThanYou.Data.Assembly;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Data.Factories;

namespace SmarterThanYou.Server.App_Start.NinjectModules
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

            this.Rebind<ISmarterThanYouContext>().To<SmarterThanYouContext>().InRequestScope();
            this.Rebind<ÌSmarterThanYouData>().To<SmarterThanYouData>();
            this.Bind<IUsersFactory>().ToFactory();
            this.Bind<IQuestionsFactory>().ToFactory();
            this.Bind<IScoresFactory>().ToFactory();
            this.Bind<IAnswersFactory>().ToFactory();
        }
    }
}