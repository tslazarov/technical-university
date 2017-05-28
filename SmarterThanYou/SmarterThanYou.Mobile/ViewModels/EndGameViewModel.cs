using Newtonsoft.Json;
using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmarterThanYou.Mobile.ViewModels
{
    public class EndGameViewModel : INotifyPropertyChanged
    {
        private Score score;
        private string highscoreMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public EndGameViewModel()
        {
            this.score = new Score();
            this.score.Username = ViewBag.Username;
            this.score.Points = ViewBag.Score;
            this.HighscoreMessage = string.Empty;

            this.SubmitScore();
        }

        public string HighscoreMessage
        {
            get
            {
                return this.highscoreMessage;
            }
            set
            {
                this.highscoreMessage = value;
                NotifyPropertyChanged();
            }
        }

        public Score Score
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

        private void NotifyPropertyChanged([CallerMemberName]String propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public async void SubmitScore()
        {
            var responseString = await this.SubmitScoreRemote();

            var response = JsonConvert.DeserializeObject<GeneralResponse>(responseString);

            if (response.Status == "1")
            {
                this.HighscoreMessage = Constants.HighScoreMessage;
            }

            ViewBag.Score = 0;
        }

        public async Task<string> SubmitScoreRemote()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var score = JsonConvert.SerializeObject(this.Score);

                var response = await client.PostAsync(Constants.ApiSubmitScore, new StringContent(score.ToString(), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
