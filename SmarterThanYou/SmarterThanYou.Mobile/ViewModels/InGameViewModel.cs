using Newtonsoft.Json;
using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
        private string answer;
        private GeneralQuestion question;
        public event PropertyChangedEventHandler PropertyChanged;

        public InGameViewModel()
        {
            this.Username = ViewBag.Username;
            this.Score = ViewBag.Score;
            this.Timer = 15;

            this.GetQuestion();
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

        public GeneralQuestion Question
        {
            get
            {
                return this.question;
            }
            set
            {
                this.question = value;
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

        public string Answer
        {
            get
            {
                return this.answer;
            }
            set
            {
                this.answer = value;
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
            if(this.Timer > 0)
            {
                this.Timer -= 1;
                return false;
            }

            return true;
        }

        public bool CheckAnswer(string answer)
        {
            if(this.Question.Answer.Member == answer)
            {
                ViewBag.Score += 100;
                this.Score += 100;
                this.ResetTimer();
                this.GetQuestion();

                return false;
            }

            return true;
        }

        public void ResetTimer()
        {
            this.Timer = 15;
        }

        public async void GetQuestion()
        {
            if (!string.IsNullOrEmpty(this.Answer))
            {
                this.Answer = string.Empty;
            }

            var responseString = await this.GetQuestionRemote();

            var response = JsonConvert.DeserializeObject<GeneralQuestion>(responseString);

            if (response != null)
            {
                response.Answers = response.Answers.OrderBy(a => Guid.NewGuid()).ToList();
                this.Question = response;
            }
        }

        public async Task<string> GetQuestionRemote()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);

                var response = await client.GetAsync(Constants.ApiQuestion);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public void FriendAnswer()
        {
            this.Answer = $"The Answer is '{this.Question.Answer.Member}'";
        }
    }
}
