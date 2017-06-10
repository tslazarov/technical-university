using Lipwig.Services.Contracts;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Authentication
{
    public class LoginViewModel : BindableBase
    {
        private IUsersService usersService;

        public LoginViewModel(IUsersService usersService)
        {
            this.usersService = usersService;

            this.RegistrationNavigateCommand = new RelayCommand<string>(RegistrationNavigate);
        }

        public event Action<string> RegistrationNavigateRequested = delegate { }; 

        public RelayCommand<string> RegistrationNavigateCommand { get; private set; }

        private void RegistrationNavigate(string destination)
        {
            this.RegistrationNavigateRequested(destination);
        }
    }
}
