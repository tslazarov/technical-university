using Lipwig.Desktop.Authentication;
using Lipwig.Desktop.Expense;
using Lipwig.Desktop.Factories;
using Lipwig.Desktop.History;
using Lipwig.Desktop.Home;
using Lipwig.Desktop.Income;
using Lipwig.Desktop.Settings;
using Lipwig.Desktop.Statistics;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
