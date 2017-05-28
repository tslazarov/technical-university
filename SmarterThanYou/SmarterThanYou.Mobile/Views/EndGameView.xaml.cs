using SmarterThanYou.Mobile.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmarterThanYou.Mobile.Views
{
    public sealed partial class EndGameView : Page
    {
        private EndGameViewModel viewModel;

        public EndGameView()
        {
            this.viewModel = new EndGameViewModel();
            this.DataContext = viewModel;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InGameView));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuView));
        }
    }
}
