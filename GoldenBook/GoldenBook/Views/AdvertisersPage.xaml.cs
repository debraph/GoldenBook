using GoldenBook.Model;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
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

            if(adsList.ItemsSource == null) await RefreshItems(true);
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

        public void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ad = e.SelectedItem as Ad;

            //TODO: Do something with the ad!
        }

        private async Task RefreshItems(bool showActivityIndicator)
        {
            using (var scope = new ActivityIndicatorDisposable(syncIndicator, showActivityIndicator))
            {
                adsList.ItemsSource = await ViewModel.GetRefreshedAdsAsync();
            }
        }
    }
}
