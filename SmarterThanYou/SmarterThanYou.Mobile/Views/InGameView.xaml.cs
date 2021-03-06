﻿using SmarterThanYou.Mobile.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmarterThanYou.Mobile.Views
{
    public sealed partial class InGameView : Page
    {
        private InGameViewModel viewModel;
        private DispatcherTimer dispatcherTimer;
        private Random random;

        public InGameView()
        {
            this.viewModel = new InGameViewModel();
            this.DataContext = viewModel;

            this.InitializeComponent();

            this.random = new Random();

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += new EventHandler<object>(this.dispatcherTimer_Tick);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            this.dispatcherTimer.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            if (this.viewModel.CheckTimer())
            {
                this.dispatcherTimer.Stop();
                this.Frame.Navigate(typeof(EndGameView));
            }

            if (!this.choiceA.IsEnabled && !this.choiceB.IsEnabled && !this.choiceC.IsEnabled && !this.choiceD.IsEnabled)
            {
                this.ChangeButtonsState(true);
            }
        }

        private void choice_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeButtonsState(false);

            if (this.viewModel.CheckAnswer((sender as Button).Content.ToString()))
            {
                this.dispatcherTimer.Stop();
                this.Frame.Navigate(typeof(EndGameView));
            }
        }

        private void ChangeButtonsState(bool state)
        {
            this.choiceA.IsEnabled = state;
            this.choiceB.IsEnabled = state;
            this.choiceC.IsEnabled = state;
            this.choiceD.IsEnabled = state;
        }

        private void btn5050_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;

            while(count < 2)
            {
                if (this.DisableOption(random.Next(0, 4)))
                {
                    count += 1;
                };
            }
            this.btn5050.IsEnabled = false;
            this.btn5050.Content = string.Empty;
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.GetQuestion();
            this.viewModel.ResetTimer();
            this.btnSkip.IsEnabled = false;
            this.btnSkip.Content = string.Empty;
        }

        private void btnFriend_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.FriendAnswer();
            this.btnFriend.IsEnabled = false;
            this.btnFriend.Content = string.Empty;
        }

        private bool DisableOption(int option)
        {
            switch (option)
            {
                case 0:
                    if(this.choiceA.IsEnabled && this.choiceA.Content.ToString() != this.viewModel.Question.Answer.Member)
                    {
                        this.choiceA.IsEnabled = false;
                        return true;
                    }
                    break;
                case 1:
                    if (this.choiceB.IsEnabled && this.choiceB.Content.ToString() != this.viewModel.Question.Answer.Member)
                    {
                        this.choiceB.IsEnabled = false;
                        return true;
                    }
                    break;
                case 2:
                    if (this.choiceC.IsEnabled && this.choiceC.Content.ToString() != this.viewModel.Question.Answer.Member)
                    {
                        this.choiceC.IsEnabled = false;
                        return true;
                    }
                    break;
                case 3:
                    if (this.choiceD.IsEnabled && this.choiceD.Content.ToString() != this.viewModel.Question.Answer.Member)
                    {
                        this.choiceD.IsEnabled = false;
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}
