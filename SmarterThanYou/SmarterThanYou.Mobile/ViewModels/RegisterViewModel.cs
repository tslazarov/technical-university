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
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private User currentUser;
        private string errorMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterViewModel()
        {
            this.currentUser = new User();
            this.ErrorMessage = string.Empty;
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                this.errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public User CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
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

            var response = JsonConvert.DeserializeObject<GeneralResponse>(responseString);

            if (response.Status == "1")
            {
                return true;
            }
            else
            {
                this.ErrorMessage = Constants.RegisterErrorMessage;
                return false;
            }
        }

        public async Task<string> RegisterUserRemote()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var user = JsonConvert.SerializeObject(this.CurrentUser);

                var response = await client.PostAsync(Constants.ApiRegister, new StringContent(user.ToString(), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
