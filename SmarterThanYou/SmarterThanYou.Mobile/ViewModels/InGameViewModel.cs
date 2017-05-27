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
    public class InGameViewModel : INotifyPropertyChanged
    {
        private string username;
        private int score;
        private int timer;
        public event PropertyChangedEventHandler PropertyChanged;

        public InGameViewModel()
        {
            this.Username = ViewBag.Username;
            this.Score = ViewBag.Score;
            this.Timer = 15;
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

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
                NotifyPropertyChanged();
            }
        }

        public int Timer
        {
            get
            {
                return this.timer;
            }
            set
            {
                this.timer = value;
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

        public bool CheckTimer()
        {
            if(this.Timer <= 0)
            {
                return true;
            }

            this.Timer -= 1;

            return false;
        }
    }
}
