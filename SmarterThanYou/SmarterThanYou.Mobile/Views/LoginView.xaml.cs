using SmarterThanYou.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmarterThanYou.Mobile.Views
{
    public sealed partial class LoginView : Page
    {
        private LoginViewModel viewModel;

        public LoginView()
        {
            this.viewModel = new LoginViewModel();
            this.DataContext = viewModel;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnLogin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.LoginUser();
        }

        private async void LoginUser()
        {
            if (await this.viewModel.LoginUser())
            {
                this.Frame.Navigate(typeof(MenuView));
            }
        }

        private void btnNavigateRegisterUser_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterView));
        }
    }
}
