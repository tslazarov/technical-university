using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Authentication
{
    public class LoginViewModel : BindableBase
    {
        public LoginViewModel()
        {
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
