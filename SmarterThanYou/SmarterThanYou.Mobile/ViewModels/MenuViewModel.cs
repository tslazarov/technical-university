using SmarterThanYou.Mobile.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmarterThanYou.Mobile.ViewModels
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private string username;

        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel()
        {
            this.Username = ViewBag.Username;
        }

        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
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
    }
}
