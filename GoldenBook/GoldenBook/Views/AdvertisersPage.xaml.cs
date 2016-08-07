using GoldenBook.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.Collections;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GoldenBook.Views
{
    public partial class AdvertisersPage : ContentPage
    {
        public AdvertisersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(adsList.ItemsSource == null) await RefreshItems(true);
        }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");

        public void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ad = e.SelectedItem as Ad;

            //TODO: Do something with the ad!
        }

        //TODO: Move all this logic below in the ViewModel
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        private async Task RefreshItems(bool showActivityIndicator)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                adsList.ItemsSource = await GetAds();
            }
        }
        
        private async Task<IEnumerable> GetAds()
        {
            IEnumerable<Ad> ads;

            try
            {
                ads = await MobileService.GetTable<Ad>().OrderByDescending(a => a.CreatedAt).ToEnumerableAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                ads = new ObservableCollection<Ad>();
            }

            return new ObservableCollection<Ad>(ads);
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool _showIndicator;
            private ActivityIndicator _indicator;
            private Task _indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                _indicator = indicator;
                _showIndicator = showIndicator;

                if (showIndicator)
                {
                    _indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    _indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                _indicator.IsVisible = isActive;
                _indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (_showIndicator)
                {
                    _indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}
