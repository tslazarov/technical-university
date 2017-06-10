using Lipwig.Desktop.Authentication;
using Lipwig.Desktop.Expense;
using Lipwig.Desktop.History;
using Lipwig.Desktop.Home;
using Lipwig.Desktop.Income;
using Lipwig.Desktop.Settings;
using Lipwig.Desktop.Statistics;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
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
        private bool isNavigationVisible;
        private decimal balance;
        private string currencyType;
        private string email;

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
            this.registerViewModel.SuccessfulRegistrationRequested += AuthenticationRenavigate;
            this.loginViewModel.SuccessfulLoginRequested += AuthenticationRenavigate;

            this.IsNavigationVisible = false;
        }

        private void LoadViewModels()
        {
            var kernel = IocContainer.Kernel;

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

        public bool IsNavigationVisible
        {
            get
            {
                return this.isNavigationVisible;
            }
            set
            {
                SetProperty(ref this.isNavigationVisible, value);
            }
        }

        public decimal Balance
        {
            get
            {
                return this.balance;
            }
            set
            {
                SetProperty(ref this.balance, value);
            }
        }

        public string CurrencyType
        {
            get
            {
                return this.currencyType;
            }
            set
            {
                SetProperty(ref this.currencyType, value);
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                SetProperty(ref this.email, value);
            }
        }

        public RelayCommand<string> NavigationCommand { get; private set; }

        private void AuthenticationRenavigate(string destination)
        {
            this.IsNavigationVisible = true;
            this.Email = Constants.Email;
            this.Balance = Constants.Balance;
            this.CurrencyType = Constants.CurrencyType;
            this.Navigate(destination);
        }

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
