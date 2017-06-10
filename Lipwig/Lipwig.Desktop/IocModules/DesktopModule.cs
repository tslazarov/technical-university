using Lipwig.Desktop.Authentication;
using Lipwig.Desktop.Expense;
using Lipwig.Desktop.History;
using Lipwig.Desktop.Home;
using Lipwig.Desktop.Income;
using Lipwig.Desktop.Settings;
using Lipwig.Desktop.Statistics;
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
            this.Bind<LoginViewModel>().To<LoginViewModel>();
            this.Bind<RegisterViewModel>().To<RegisterViewModel>();
            this.Bind<ExpenseAddEditViewModel>().To<ExpenseAddEditViewModel>();
            this.Bind<HistoryViewModel>().To<HistoryViewModel>();
            this.Bind<HomeViewModel>().To<HomeViewModel>();
            this.Bind<IncomeAddEditViewModel>().To<IncomeAddEditViewModel>();
            this.Bind<SettingsViewModel>().To<SettingsViewModel>();
            this.Bind<StatisticsViewModel>().To<StatisticsViewModel>();
        }
    }
}
