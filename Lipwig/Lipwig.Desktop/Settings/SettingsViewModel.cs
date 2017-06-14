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

namespace Lipwig.Desktop.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private string message;
        private string messagePassword;
        private string messageColor;
        private Currency currency;
        private SimpleEditUser user;

        private IUsersService usersService;
        private ICurrenciesService currenciesService;
        private IModelFactory modelFactory;

        public SettingsViewModel(IUsersService usersService,
            ICurrenciesService currenciesService,
            IModelFactory modelFactory)
        {
            this.usersService = usersService;
            this.currenciesService = currenciesService;
            this.modelFactory = modelFactory;

            this.User = this.modelFactory.CreateSimpleEditUser();

            this.Currencies = this.currenciesService.GetCurrencies();
            this.PopulateFields();

            this.SaveUserInformationCommand = new RelayCommand(SaveUserInformation);
            this.SaveUserPasswordCommand = new RelayCommand<object>(SaveUserPassword);
        }

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

        public string MessagePassword
        {
            get
            {
                return this.messagePassword;
            }
            set
            {
               this.SetProperty(ref this.messagePassword, value);
            }
        }

        public string MessageColor
        {
            get
            {
                return this.messageColor;
            }
            set
            {
               this.SetProperty(ref this.messageColor, value);
            }
        }

        public IEnumerable<Currency> Currencies { get; set; }

        public Currency Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
               this.SetProperty(ref this.currency, value);
            }
        }


        public SimpleEditUser User
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
        public RelayCommand SaveUserInformationCommand { get; private set; }

        public RelayCommand<object> SaveUserPasswordCommand { get; private set; }

        public event Action SuccessfulUserInformationRequested = delegate { };

        private void SaveUserInformation()
        {
            try
            {
                var user = this.usersService.GetUserByEmail(ViewBag.Email);

                user.Email = this.User.Email;
                user.FirstName = this.User.FirstName;
                user.LastName = this.User.LastName;
                user.Currency = this.Currency;
                user.CurrencyId = this.Currency.Id;

                this.usersService.UpdateUser(user);

                ViewBag.Email = user.Email;
                ViewBag.Balance = user.LocalizedBalance;
                ViewBag.CurrencyType = user.Currency.Name;
                ViewBag.CurrencyValue = user.Currency.Value;

                this.Message = "User details save was successful";
                this.MessageColor = "#2CB144";

                this.SuccessfulUserInformationRequested();
            }
            catch
            {
                this.Message = "User details save was unsuccessful";
                this.MessageColor = "#FFD50000";
            }
            finally
            {
                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }

        private void SaveUserPassword(object passwordBoxes)
        {
            bool result = true;

            try
            {
                var passwords = (object[])passwordBoxes;
                var oldPassword = (passwords[0] as RadPasswordBox).Password;
                var newPassword = (passwords[1] as RadPasswordBox).Password;

                if (string.IsNullOrEmpty(newPassword))
                {
                    throw new NullReferenceException();
                }

                result = this.usersService.UpdateUserPassword(ViewBag.Email, oldPassword, newPassword);

                if (result)
                {
                    this.MessagePassword = "User password save was successful";
                    this.MessageColor = "#2CB144";
                }
            }
            catch
            {
                this.MessagePassword = "User password save was unsuccessful";
                this.MessageColor = "#FFD50000";
            }
            finally
            {
                if (!result)
                {
                    this.MessagePassword = "Old password was incorrect";
                    this.MessageColor = "#FFD50000";
                }

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.MessagePassword = string.Empty;
                    });
            }
        }

        private void PopulateFields()
        {
            var user = this.usersService.GetUserByEmail(ViewBag.Email);

            if(user != null)
            {
                this.User.Email = user.Email;
                this.User.FirstName = user.FirstName;
                this.User.LastName = user.LastName;
                this.Currency = user.Currency;
            }
        }
    }
}
