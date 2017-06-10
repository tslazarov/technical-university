using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Lipwig.Desktop.Authentication
{
    public class LoginViewModel : BindableBase
    {
        private string message;

        private IUsersService usersService;

        public LoginViewModel(IUsersService usersService)
        {
            this.usersService = usersService;

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
                SetProperty(ref this.message, value);
            }
        }
        public RelayCommand<object> LoginCommand { get; private set; }

        public RelayCommand<string> RegistrationNavigateCommand { get; private set; }


        public event Action<string> RegistrationNavigateRequested = delegate { };

        public event Action<string> SuccessfulLoginRequested = delegate { };

        private void Login(object passwordBox)
        {
            try
            {
                var password = (passwordBox as RadPasswordBox).Password;

                var user = this.usersService.Login(this.Email, password);

                if (user == null)
                {
                    throw new NullReferenceException();
                }

                Constants.CurrencyType = user.Currency.Name;
                Constants.CurrencyValue = user.Currency.Value;
                Constants.Balance = user.LocalizedBalance;
                Constants.Email = user.Email;

                this.SuccessfulLoginRequested("home");
            }
            catch
            {

                this.Message = "Invalid email or password";

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
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
