using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace SmarterThanYou.Mobile.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private User currentUser;
        private ICommand registerUserCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterViewModel()
        {
            this.currentUser = new User();
        }

        public User CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName]String propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void RegisterUser()
        {
            // Create a call to register
        }
    }
}
