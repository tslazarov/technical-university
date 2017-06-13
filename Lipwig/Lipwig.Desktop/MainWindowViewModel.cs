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
        private IKernel kernel;

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
            this.kernel = IocContainer.Kernel;

            this.NavigationCommand = new RelayCommand<string>(Navigate);
            this.IsNavigationVisible = false;

            this.Navigate("login");
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
                    this.CurrentViewModel = kernel.Get<HomeViewModel>();
                    break;

                case "expense":
                    if(this.expenseAddEditViewModel != null)
                    {
                        this.expenseAddEditViewModel.SuccessfulExpenseRequested -= UpdateBalance;

                    }
                    this.expenseAddEditViewModel = kernel.Get<ExpenseAddEditViewModel>();
                    this.expenseAddEditViewModel.SuccessfulExpenseRequested += UpdateBalance;

                    this.CurrentViewModel = this.expenseAddEditViewModel;
                    break;

                case "income":
                    if(this.incomeAddEditViewModel != null)
                    {
                        this.incomeAddEditViewModel.SuccessfulIncomeRequested -= UpdateBalance;
                    }
                    this.incomeAddEditViewModel = kernel.Get<IncomeAddEditViewModel>();
                    this.incomeAddEditViewModel.SuccessfulIncomeRequested += UpdateBalance;

                    this.CurrentViewModel = this.incomeAddEditViewModel;
                    break;

                case "history":
                    if (this.settingsViewModel != null)
                    {
                        this.historyViewModel.SuccessfulDeleteRequested -= UpdateBalance;
                    }
                    this.historyViewModel = kernel.Get<HistoryViewModel>();
                    this.historyViewModel.SuccessfulDeleteRequested += UpdateBalance;

                    this.CurrentViewModel = this.historyViewModel;
                    break;

                case "statistics":
                    this.CurrentViewModel = kernel.Get<StatisticsViewModel>();
                    break;

                case "settings":
                    if(this.settingsViewModel != null)
                    {
                        this.settingsViewModel.SuccessfulUserInformationRequested -= UpdateUserInformation;
                    }
                    this.settingsViewModel = kernel.Get<SettingsViewModel>();
                    this.settingsViewModel.SuccessfulUserInformationRequested += UpdateUserInformation;

                    this.CurrentViewModel = this.settingsViewModel;
                    break;

                case "register":
                    if (this.registerViewModel != null)
                    {
                        this.registerViewModel.SuccessfulRegistrationRequested -= AuthenticationRenavigate;
                    }
                    this.registerViewModel = kernel.Get<RegisterViewModel>();
                    this.registerViewModel.SuccessfulRegistrationRequested += AuthenticationRenavigate;

                    this.CurrentViewModel = this.registerViewModel;
                    break;

                default:
                    if(this.loginViewModel != null)
                    {
                        this.loginViewModel.RegistrationNavigateRequested -= Navigate;
                        this.loginViewModel.SuccessfulLoginRequested -= AuthenticationRenavigate;
                    }
                    this.loginViewModel = kernel.Get<LoginViewModel>();
                    this.loginViewModel.RegistrationNavigateRequested += Navigate;
                    this.loginViewModel.SuccessfulLoginRequested += AuthenticationRenavigate;

                    this.CurrentViewModel = this.loginViewModel;
                    break;
            }
        }

        private void UpdateBalance()
        {
            this.Balance = Constants.Balance;
        }

        private void UpdateUserInformation()
        {
            this.CurrencyType = Constants.CurrencyType;
            this.Email = Constants.Email;
            this.Balance = Constants.Balance;
        }
    }
}
