using SmarterThanYou.Mobile.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmarterThanYou.Mobile.Views
{
    public sealed partial class ScoreboardView : Page
    {
        private ScoreboardViewModel viewModel;

        public ScoreboardView()
        {
            this.viewModel = new ScoreboardViewModel();
            this.DataContext = this.viewModel;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuView));
        }
    }
}
