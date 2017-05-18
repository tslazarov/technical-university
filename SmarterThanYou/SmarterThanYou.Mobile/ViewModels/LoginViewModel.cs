using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using SmarterThanYou.Mobile.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace SmarterThanYou.Mobile.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private User currentUser;
        private Frame frame;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel(Frame frame)
        {
            this.currentUser = new User();
            this.frame = frame;
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

        public void LoginUser()
        {
            //Create a call to login a user
        }
    }
}