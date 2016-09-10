using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenBook.Model;
using Xamarin.Forms;
using GoldenBook.ServiceContract;
using Microsoft.Practices.ServiceLocation;

namespace GoldenBook.Views
{
    public partial class AdsDetailPage : ContentPage
    {
        public AdsDetailPage(Ad ad)
        {
            InitializeComponent();

            BindingContext = ad;
            LoadImageSource(ad);
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
        
        private IMediaService MediaService => ServiceLocator.Current.GetInstance<IMediaService>();

        private void LoadImageSource(Ad ad)
        {
            if (ad != null)
            {
                var filepath = MediaService.GetFilepath(ad.PhotoId);

                CurrentImageSource = ImageSource.FromFile(filepath);
            }
        }

        public static readonly BindableProperty CurrentImageSourceProperty = BindableProperty.Create("CurrentImageSource", typeof(string), typeof(ImageSource), null);

        public ImageSource CurrentImageSource
        {
            get { return (string)GetValue(CurrentImageSourceProperty); }
            set { SetValue(CurrentImageSourceProperty, value); }
        }
    }
}
