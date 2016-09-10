using GoldenBook.Model;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using GoldenBook.ViewModel.Interfaces;
using GoldenBook.Helpers;

namespace GoldenBook.Views
{
    public partial class AdvertisersPage : ContentPage
    {
        public AdvertisersPage()
        {
            InitializeComponent();
        }

        private IAdvertisersViewModel ViewModel   => BindingContext as IAdvertisersViewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (adsList.ItemsSource == null)
            {
                if (ViewModel.Ads != null)
                {
                    adsList.ItemsSource = ViewModel.Ads;
                }
                else
                {
                    await RefreshItems(true);
                }
            }
        }

        public async void OnRefresh(object sender, EventArgs e)
        {
            try
            {
                await RefreshItems(false);
            }
            catch { }
            finally
            {
                ((ListView)sender)?.EndRefresh();
            }
        }

        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ad = e.SelectedItem as Ad;

            await Navigation.PushModalAsync(new AdsDetailPage(ad), true);
        }

        private async Task RefreshItems(bool showActivityIndicator)
        {
            using (var scope = new ActivityIndicatorDisposable(syncIndicator, showActivityIndicator))
            {
                await ViewModel.RefreshAdsAsync();
                adsList.ItemsSource = ViewModel.Ads;
            }
        }
    }
}
