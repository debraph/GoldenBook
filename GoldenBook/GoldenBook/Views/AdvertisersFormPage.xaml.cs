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
        }
    }
}
