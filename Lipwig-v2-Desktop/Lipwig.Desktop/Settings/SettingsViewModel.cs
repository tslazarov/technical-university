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

namespace Lipwig.Desktop.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private string message;
        private string messagePassword;
        private string messageColor;
        private Currency currency;
        private SimpleEditUser user;
        private IEnumerable<Currency> currencies;

        private IModelFactory modelFactory;

        public SettingsViewModel(IModelFactory modelFactory)
        {
            this.modelFactory = modelFactory;

            this.User = this.modelFactory.CreateSimpleEditUser();

            this.GetCurrencies();
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

        private async void SaveUserInformation()
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                user.Email = this.User.Email;
                user.FirstName = this.User.FirstName;
                user.LastName = this.User.LastName;
                user.Currency = this.Currency;
                user.CurrencyId = this.Currency.Id;

                var responseUser = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateUserRemote(user));

                if (responseUser.Status == "0")
                {
                    throw new Exception();
                }

                ViewBag.Email = user.Email;
                ViewBag.Balance = user.LocalizedBalance;
                ViewBag.CurrencyType = user.Currency.Name;
                ViewBag.CurrencyValue = user.Currency.Value;

                this.Message = Constants.SuccessfulUserDetailsUpdate;
                this.MessageColor = Constants.MessagePositiveColor;

                this.SuccessfulUserInformationRequested();
            }
            catch
            {
                this.Message = Constants.UnsuccessfulUserDetailsUpdate;
                this.MessageColor = Constants.MessagePositiveColor;
            }
            finally
            {
                await Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }

        private async void SaveUserPassword(object passwordBoxes)
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

                var responseUserPassword = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateUserPassword(ViewBag.Email, oldPassword, newPassword));

                if (responseUserPassword.Status == "0")
                {
                    result = false;
                    throw new Exception();
                }

                result = JsonConvert.DeserializeObject<bool>(responseUserPassword.ComplexObject);

                if (result)
                {
                    this.MessagePassword = Constants.SuccessfulUserPasswordUpdate;
                    this.MessageColor = Constants.MessagePositiveColor;
                }
            }
            catch
            {
                this.MessagePassword = Constants.UnsuccessfulUserPasswordUpdate;
                this.MessageColor = Constants.MessageNegativeColor;
            }
            finally
            {
                if (!result)
                {
                    this.MessagePassword = Constants.IncorrectOldPassword;
                    this.MessageColor = Constants.MessageNegativeColor;
                }

                await Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.MessagePassword = string.Empty;
                    });
            }
        }

        private async void PopulateFields()
        {
            var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

            if (user != null)
            {
                this.User.Email = user.Email;
                this.User.FirstName = user.FirstName;
                this.User.LastName = user.LastName;
                this.Currency = user.Currency;
            }
        }

        private async void GetCurrencies()
        {
            var currenciesResponse = await HttpRequestHelper.GetCurrenciesRemote();

            try
            {
                this.Currencies = JsonConvert.DeserializeObject<IEnumerable<Currency>>(await HttpRequestHelper.GetCurrenciesRemote());
            }
            catch
            {
            }
        }
    }
}
