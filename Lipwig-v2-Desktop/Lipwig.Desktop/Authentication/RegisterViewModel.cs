using Lipwig.Data.Factories;
using Lipwig.Desktop.Factories;
using Lipwig.Desktop.Models;
using Lipwig.HttpUtilities;
using Lipwig.Models;
using Lipwig.Utilities;
using Newtonsoft.Json;
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
        private IEnumerable<Currency> currencies;

        private IUsersFactory usersFactory;
        private IModelFactory modelFactory;

        public RegisterViewModel(IUsersFactory usersFactory,
            IModelFactory modelFactory)
        {
            this.usersFactory = usersFactory;
            this.modelFactory = modelFactory;

            this.User = this.modelFactory.CreateSimpleRegistrationUser();
            this.RegisterCommand = new RelayCommand<object>(Register);

            this.GetCurrencies();
        }

        public IEnumerable<Currency> Currencies
        {
            get
            {
                return this.currencies;
            }
            set
            {
                this.SetProperty(ref this.currencies, value);
            }
        }

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

        private async void Register(object passwordBox)
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

                var user = this.usersFactory.Create(Guid.NewGuid(), this.User.Email, this.User.FirstName, this.User.LastName, balance, this.Currency);

                var responseString = await HttpRequestHelper.RegisterUserRemote(user, password);

                var response = JsonConvert.DeserializeObject<GeneralResponse>(responseString);


                if (response.Status == "0")
                {
                    throw new NullReferenceException();
                }

                if(response.Status == "1")
                {
                    ViewBag.Email = this.User.Email;
                    ViewBag.Balance = balance;

                    this.SuccessfulRegistrationRequested("home");
                }

                if(response.Status == "2")
                {
                    existingUser = true;
                }
            }
            catch
            {
                if (existingUser)
                {
                    this.Message = Constants.AlreadyExistingUserMessage;
                }
                else
                {
                    this.Message = Constants.UnsuccessfulRegistration;
                }

                await Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }

        private async void GetCurrencies()
        {
            var currenciesResponse = await HttpRequestHelper.GetCurrenciesRemote();

            try
            {
                this.Currencies = JsonConvert.DeserializeObject<IEnumerable<Currency>>(currenciesResponse);
            }
            catch
            {
            }
        }
    }
}
