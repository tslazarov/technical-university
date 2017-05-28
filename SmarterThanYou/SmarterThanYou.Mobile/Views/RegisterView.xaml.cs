using SmarterThanYou.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmarterThanYou.Mobile.Views
{
    public sealed partial class RegisterView : Page
    {
        private RegisterViewModel viewModel; 

        public RegisterView()
        {
            this.viewModel = new RegisterViewModel();
            this.DataContext = this.viewModel;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnRegister_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.RegisterUser();
        }

        private async void RegisterUser()
        {
            if (await this.viewModel.RegisterUser())
            {
                this.Frame.Navigate(typeof(LoginView));
            }
        }
    }
}
