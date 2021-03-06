﻿using Lipwig.Desktop.Authentication;
using Lipwig.Desktop.Expense;
using Lipwig.Desktop.History;
using Lipwig.Desktop.Home;
using Lipwig.Desktop.Income;
using Lipwig.Desktop.Settings;
using Lipwig.Desktop.Statistics;
using Lipwig.Utilities;
using Ninject;
using System;

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

            this.IsNavigationVisible = false;

            this.Navigate("login");

            this.NavigationCommand = new RelayCommand<string>(Navigate);
            this.LogoutCommand = new RelayCommand(Logout);
        }

        public BindableBase CurrentViewModel
        {
            get
            {
                return this.currentViewModel;
            }
            set
            {
               this.SetProperty(ref this.currentViewModel, value);
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
               this.SetProperty(ref this.isNavigationVisible, value);
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
               this.SetProperty(ref this.balance, value);
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
               this.SetProperty(ref this.currencyType, value);
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
               this.SetProperty(ref this.email, value);
            }
        }

        public RelayCommand<string> NavigationCommand { get; private set; }

        public RelayCommand LogoutCommand { get; private set; }

        private void AuthenticationRenavigate(string destination)
        {
            this.IsNavigationVisible = true;
            this.Email = ViewBag.Email;
            this.Balance = ViewBag.Balance;
            this.CurrencyType = ViewBag.CurrencyType;
            this.Navigate(destination);
        }

        private void Logout()
        {
            this.IsNavigationVisible = false;
            ViewBag.Balance = 0M;
            ViewBag.CurrencyValue = 0M;
            ViewBag.CurrencyType = string.Empty;
            ViewBag.Email = string.Empty;

            this.Navigate("login");
        }

        private void Navigate(string destination)
        {
            switch (destination)
            {
                case "home":
                    this.homeViewModel = kernel.Get<HomeViewModel>();

                    this.CurrentViewModel = this.homeViewModel;
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
                        this.historyViewModel.SuccessfulExpenseEditRequested -= EditExpense;
                        this.historyViewModel.SuccessfulIncomeEditRequested -= EditIncome;
                    }
                    this.historyViewModel = kernel.Get<HistoryViewModel>();
                    this.historyViewModel.SuccessfulDeleteRequested += UpdateBalance;
                    this.historyViewModel.SuccessfulExpenseEditRequested += EditExpense;
                    this.historyViewModel.SuccessfulIncomeEditRequested += EditIncome;

                    this.CurrentViewModel = this.historyViewModel;
                    break;

                case "statistics":
                    this.statisticsViewModel = kernel.Get<StatisticsViewModel>();

                    this.CurrentViewModel = this.statisticsViewModel;
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

        private void EditIncome(Guid id)
        {

            if (this.incomeAddEditViewModel != null)
            {
                this.incomeAddEditViewModel.SuccessfulIncomeRequested -= UpdateBalance;
            }
            this.incomeAddEditViewModel = kernel.Get<IncomeAddEditViewModel>();
            this.incomeAddEditViewModel.SuccessfulIncomeRequested += UpdateBalance;

            this.CurrentViewModel = this.incomeAddEditViewModel;
            (this.CurrentViewModel as IncomeAddEditViewModel).PopulateEditView(id);
        }

        private void EditExpense(Guid id)
        {
            if (this.expenseAddEditViewModel != null)
            {
                this.expenseAddEditViewModel.SuccessfulExpenseRequested -= UpdateBalance;

            }
            this.expenseAddEditViewModel = kernel.Get<ExpenseAddEditViewModel>();
            this.expenseAddEditViewModel.SuccessfulExpenseRequested += UpdateBalance;

            this.CurrentViewModel = this.expenseAddEditViewModel;
            (this.CurrentViewModel as ExpenseAddEditViewModel).PopulateEditView(id);
        }

        private void UpdateBalance()
        {
            this.Balance = ViewBag.Balance;
        }

        private void UpdateUserInformation()
        {
            this.CurrencyType = ViewBag.CurrencyType;
            this.Email = ViewBag.Email;
            this.Balance = ViewBag.Balance;
        }
    }
}
