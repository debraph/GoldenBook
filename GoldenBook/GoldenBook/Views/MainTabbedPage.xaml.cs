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

            if (GetAppSetting(UserConfigurationViewModel.AppSettingFirstNameKey) == null || GetAppSetting(UserConfigurationViewModel.AppSettingLastNameKey) == null)
            {
                await Navigation.PushModalAsync(new UserConfigurationPage(), true);
            }
        }

        private string GetAppSetting(string key)
        {
            if (Application.Current.Properties.ContainsKey(key)) return Application.Current.Properties[key] as string;
            return null;
        }
    }
}
