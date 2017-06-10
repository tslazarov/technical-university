using Lipwig.Desktop.Authentication;
using Lipwig.Desktop.Expense;
using Lipwig.Desktop.History;
using Lipwig.Desktop.Home;
using Lipwig.Desktop.Income;
using Lipwig.Desktop.Settings;
using Lipwig.Desktop.Statistics;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop
{
    public class MainWindowViewModel : BindableBase
    {
        private BindableBase currentViewModel;

        private LoginViewModel loginViewModel;
        private RegisterViewModel registerViewModel;
        private ExpenseAddEditViewModel expenseAddEditViewModel;
        private HistoryViewModel historyViewModel;
        private HomeViewModel homeViewModel;
        private IncomeAddEditViewModel incomeAddEditViewModel;
        private SettingsViewModel settingsViewModel;
        private StatisticsViewModel statisticsViewModel;

        public MainWindowViewModel()
        {
            this.LoadViewModels();

            this.CurrentViewModel = this.loginViewModel;

            this.NavigationCommand = new RelayCommand<string>(Navigate);

            this.loginViewModel.RegistrationNavigateRequested += Navigate;

            this.IsNavigationVisible = false;
        }

        private void LoadViewModels()
        {
            var kernel = new StandardKernel();

            this.loginViewModel = kernel.Get<LoginViewModel>();
            this.registerViewModel = kernel.Get<RegisterViewModel>();
            this.expenseAddEditViewModel = kernel.Get<ExpenseAddEditViewModel>();
            this.historyViewModel = kernel.Get<HistoryViewModel>();
            this.homeViewModel = kernel.Get<HomeViewModel>();
            this.incomeAddEditViewModel = kernel.Get<IncomeAddEditViewModel>();
            this.settingsViewModel = kernel.Get<SettingsViewModel>();
            this.statisticsViewModel = kernel.Get<StatisticsViewModel>();
        }

        public BindableBase CurrentViewModel
        {
            get
            {
                return this.currentViewModel;
            }
            set
            {
                SetProperty(ref this.currentViewModel, value);
            }
        }

        public bool IsNavigationVisible { get; set; }

        public RelayCommand<string> NavigationCommand { get; private set; }

        private void Navigate(string destination)
        {
            switch (destination)
            {
                case "home":
                    this.CurrentViewModel = this.homeViewModel;
                    break;
                case "expense":
                    this.CurrentViewModel = this.expenseAddEditViewModel;
                    break;
                case "income":
                    this.CurrentViewModel = this.incomeAddEditViewModel;
                    break;
                case "history":
                    this.CurrentViewModel = this.historyViewModel;
                    break;
                case "statistics":
                    this.CurrentViewModel = this.statisticsViewModel;
                    break;
                case "settings":
                    this.CurrentViewModel = this.settingsViewModel;
                    break;
                case "register":
                    this.CurrentViewModel = this.registerViewModel;
                    break;
                default:
                    this.CurrentViewModel = this.loginViewModel;
                    break;
            }
        }
    }
}
