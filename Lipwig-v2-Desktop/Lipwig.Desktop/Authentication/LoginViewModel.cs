using Lipwig.Desktop.Models;
using Lipwig.HttpUtilities;
using Lipwig.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Lipwig.Desktop.Authentication
{
    public class LoginViewModel : BindableBase
    {
        private string message;

        public LoginViewModel()
        {
            this.RegistrationNavigateCommand = new RelayCommand<string>(RegistrationNavigate);
            this.LoginCommand = new RelayCommand<object>(Login);
        }

        public string Email { get; set; }

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
        public RelayCommand<object> LoginCommand { get; private set; }

        public RelayCommand<string> RegistrationNavigateCommand { get; private set; }


        public event Action<string> RegistrationNavigateRequested = delegate { };

        public event Action<string> SuccessfulLoginRequested = delegate { };

        private async void Login(object passwordBox)
        {
            try
            {
                var password = (passwordBox as RadPasswordBox).Password;

                var responseString = await HttpRequestHelper.LoginUserRemote(this.Email, password);

                var response = JsonConvert.DeserializeObject<GeneralResponse>(responseString);


                if (response.Status == "0")
                {
                    throw new NullReferenceException();
                }

                if (response.Status == "1")
                {
                    var user = JsonConvert.DeserializeObject<UserPostLogin>(response.ComplexObject);

                    ViewBag.CurrencyType = user.Currency.Name;
                    ViewBag.CurrencyValue = user.Currency.Value;
                    ViewBag.Balance = user.LocalizedBalance;
                    ViewBag.Email = user.Email;

                    this.SuccessfulLoginRequested("home");
                }
            }
            catch
            {

                this.Message = Constants.InvalidEmailOrPasswordMessage;

                await Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }

        private void RegistrationNavigate(string destination)
        {
            this.RegistrationNavigateRequested(destination);
        }
    }
}
