using SmarterThanYou.Mobile.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SmarterThanYou.Mobile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterView : Page
    {
        private RegisterViewModel viewModel; 

        public RegisterView()
        {
            this.viewModel = new RegisterViewModel();
            this.DataContext = this.viewModel;

            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
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
