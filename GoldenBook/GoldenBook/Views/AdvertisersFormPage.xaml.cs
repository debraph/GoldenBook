using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;

using Xamarin.Forms;

namespace GoldenBook.Views
{
    public partial class AdvertisersFormPage : ContentPage
    {
        public AdvertisersFormPage()
        {
            InitializeComponent();

            BindingContextChanged += (object sender, EventArgs e) => 
            {
                MessagingCenter.Send<Page>(this, "BindingContextChanged.AdvertisersFormViewModel");
            };

            var advertisersViewModel = ServiceLocator.Current.GetInstance<IAdvertisersViewModel>();

            advertisersViewModel?.RefreshAdsAsync();
        }

        private IAdvertisersFormViewModel ViewModel => ServiceLocator.Current.GetInstance<IAdvertisersFormViewModel>();

        private void OnButtonSendClicked(object sender, EventArgs e)
        {
            ViewModel?.SendCommand?.Execute(null);
        }
    }
}
