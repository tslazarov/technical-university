using Newtonsoft.Json;
using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> RegisterUser()
        {
            var responseString = await this.RegisterUserRemote();
            Debug.WriteLine(responseString);

            var response = JsonConvert.DeserializeObject<GeneralResponse>(responseString);

            if(response.Status == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> RegisterUserRemote()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri("http://cd5cfc6f.ngrok.io/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json_object = JsonConvert.SerializeObject(this.CurrentUser);

                var response = await client.PostAsync("api/account/register", new StringContent(json_object.ToString(), Encoding.UTF8, "application/json"));
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
