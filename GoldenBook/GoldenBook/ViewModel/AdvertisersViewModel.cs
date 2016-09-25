using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GoldenBook.Model;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Xamarin.Forms;
using GoldenBook.ServiceContract;
using Microsoft.Practices.ServiceLocation;

namespace GoldenBook.ViewModel
{
    public class AdvertisersViewModel : ViewModelBase, IAdvertisersViewModel
    {
        private List<Ad> _ads;
        private IMediaService _mediaService;
        private IRestClient _restClient;

        public AdvertisersViewModel(IRestClient restClient, IMediaService mediaService)
        {
            _restClient   = restClient;
            _mediaService = mediaService;
        }

        public List<Ad> Ads
        {
            get { return _ads; }
            private set { Set(ref _ads, value); }
        }

        public async Task RefreshAdsAsync()
        {
            try
            {
                Ads = await _restClient.GetAds();

                foreach(Ad ad in Ads)
                {
                    ad.Picture = await LoadPicture(ad.PhotoId);
                }
            }
            catch (Exception ex)
            {
                Ads = new List<Ad>();
            }
        }

        private async Task<ImageSource> LoadPicture(string photoId)
        {
            if (photoId == null) return null;

            string pictureFilePath   = _mediaService.GetFilepath(photoId);
            string thumbnailFilePath = _mediaService.GetFilepath($"{photoId}_thumb");

            if (pictureFilePath == null || thumbnailFilePath == null)
            {
                var pictureByteArray = await _restClient.GetPhoto(photoId);

                if (pictureByteArray == null || pictureByteArray.Length == 0) return null;

                pictureFilePath = _mediaService.SavePictureAndThumbnail(pictureByteArray, photoId);

                if (pictureFilePath == null) return null;

                thumbnailFilePath = $"{pictureFilePath}_thumb"; // By convention
            }

            ImageSource imageSource = ImageSource.FromFile(thumbnailFilePath);

            return imageSource;
        }
    }
}
