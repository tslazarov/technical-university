﻿using Newtonsoft.Json;
using SmarterThanYou.Mobile.Common;
using SmarterThanYou.Mobile.Models;
using SmarterThanYou.Mobile.Views;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace SmarterThanYou.Mobile.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private User currentUser;
        private string errorMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
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

        public async Task<bool> LoginUser()
        {
            var responseString = await this.LoginUserRemote();

            var response = JsonConvert.DeserializeObject<GeneralResponse>(responseString);

            if (response.Status == "1")
            {
                ViewBag.Username = response.Username;
                return true;
            }
            else
            {
                this.ErrorMessage = Constants.LoginErrorMessage;
                return false;
            }
        }

        public async Task<string> LoginUserRemote()
        {
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var user = JsonConvert.SerializeObject(this.CurrentUser);

                var response = await client.PostAsync(Constants.ApiLogin, new StringContent(user.ToString(), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}