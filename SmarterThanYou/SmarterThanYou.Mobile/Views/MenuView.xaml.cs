using SmarterThanYou.Mobile.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmarterThanYou.Mobile.Views
{
    public sealed partial class MenuView : Page
    {
        private MenuViewModel viewModel;

        public MenuView()
        {
            this.viewModel = new MenuViewModel();
            this.DataContext = viewModel;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InGameView));
        }

        private void btnScoreboard_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ScoreboardView));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
