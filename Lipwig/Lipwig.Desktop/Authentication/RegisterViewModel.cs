﻿using Lipwig.Data.Factories;
using Lipwig.Desktop.Factories;
using Lipwig.Desktop.Models;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Lipwig.Desktop.Authentication
{
    public class RegisterViewModel : BindableBase
    {
        private SimpleRegistrationUser user;
        private string message;

        private ICurrenciesService currenciesService;
        private IUsersService usersService;
        private IUsersFactory usersFactory;
        private IModelFactory modelFactory;

        public RegisterViewModel(ICurrenciesService currenciesService,
            IUsersService usersService,
            IUsersFactory usersFactory,
            IModelFactory modelFactory)
        {
            this.currenciesService = currenciesService;
            this.usersService = usersService;
            this.usersFactory = usersFactory;
            this.modelFactory = modelFactory;

            this.Currencies = this.currenciesService.GetCurrencies();
            this.User = this.modelFactory.CreateSimpleRegistrationUser();

            this.RegisterCommand = new RelayCommand<object>(Register);
        }

        public IEnumerable<Currency> Currencies { get; set; }

        public SimpleRegistrationUser User
        {
            get
            {
                return this.user;
            }
            set
            {
               this.SetProperty(ref this.user, value);
            }
        }

        public Currency Currency { get; set; }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
               this.SetProperty(ref this.message, value);
            }
        }

        public RelayCommand<object> RegisterCommand { get; private set; }

        public event Action<string> SuccessfulRegistrationRequested = delegate { };

        private void Register(object passwordBox)
        {
            var existingUser = false;

            try
            {
                var password = (passwordBox as RadPasswordBox).Password;
                
                if(this.Currency != null)
                {
                    ViewBag.CurrencyValue = this.Currency.Value;
                    ViewBag.CurrencyType = this.Currency.Name;
                }
                else
                {
                    throw new NullReferenceException();
                }

                var balance = decimal.Parse(this.User.Balance);

                if(this.usersService.GetUserByEmail(this.User.Email) != null)
                {
                    existingUser = true;
                }

                var user = this.usersFactory.Create(Guid.NewGuid(), this.User.Email, this.User.FirstName, this.User.LastName, balance, this.Currency);

                this.usersService.Register(user, password);

                ViewBag.Email = this.user.Email;
                ViewBag.Balance = balance;

                this.SuccessfulRegistrationRequested("home");
            }
            catch
            {
                if (existingUser)
                {
                    this.Message = "The email is registered";
                }
                else
                {
                    this.Message = "Registration was unsuccessful";
                }

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }
    }
}
