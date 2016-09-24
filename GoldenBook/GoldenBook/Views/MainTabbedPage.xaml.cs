using GoldenBook.Helpers;
using GoldenBook.ViewModel;

using Xamarin.Forms;

namespace GoldenBook.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            Children.Add(new AdvertisersFormPage());
            Children.Add(new AdvertisersPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (string.IsNullOrEmpty(Settings.FirstName) || string.IsNullOrEmpty(Settings.LastName))
            {
                await Navigation.PushModalAsync(new UserConfigurationPage(), true);
            }
        }
    }
}
