using Newtonsoft.Json;
using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmarterThanYou.Mobile.ViewModels
{
    public class ScoreboardViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Scoreboard> scoreboard;

        public event PropertyChangedEventHandler PropertyChanged;

        public ScoreboardViewModel()
        {
            this.Username = ViewBag.Username;
            this.GetScoreboard();
        }

        public string Username { get; set; }

        public IEnumerable<Scoreboard> Scoreboard
        {
            get
            {
                return this.scoreboard;
            }
            set
            {
                this.scoreboard = value;
                this.NotifyPropertyChanged();

            }
        }

        private void NotifyPropertyChanged([CallerMemberName]String propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public async void GetScoreboard()
        {
            var responseString = await this.GetScoreboardRemote();

            var response = JsonConvert.DeserializeObject<GeneralScoreboard>(responseString);

            if (response != null)
            {
                this.Scoreboard = response.Scoreboard;
            }
        }

        public async Task<string> GetScoreboardRemote()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);

                var response = await client.GetAsync(Constants.ApiScoreboard);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
