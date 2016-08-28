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

        public AdvertisersViewModel()
        { }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");
        private string Sas => "https://goldenbook.blob.core.windows.net/golden-book-photos?sv=2015-04-05&sr=c&sig=hnDVgepWpsAbX7Lj9o1h%2FgN7t3Va3A3meBGoMejx%2Fwc%3D&se=2017-08-18T19%3A13%3A55Z&sp=rwdl";

        public List<Ad> Ads
        {
            get { return _ads; }
            private set { Set(ref _ads, value); }
        }

        public async Task RefreshAdsAsync()
        {
            try
            {
                Ads = await MobileService.GetTable<Ad>().OrderByDescending(a => a.CreatedAt).ToListAsync();

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

        private IMediaService MediaService => ServiceLocator.Current.GetInstance<IMediaService>();

        private async Task<ImageSource> LoadPicture(string photoId)
        {
            if (photoId == null) return null;

            string pictureFilePath   = MediaService.GetFilepath(photoId);
            string thumbnailFilePath = MediaService.GetFilepath($"{photoId}_thumb");

            if (pictureFilePath == null || thumbnailFilePath == null)
            {
                CloudBlobContainer container = new CloudBlobContainer(new Uri(Sas));

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(photoId);

                await blockBlob.FetchAttributesAsync();

                long fileByteLength = blockBlob.Properties.Length;

                var pictureByteArray = new byte[fileByteLength];

                await blockBlob.DownloadToByteArrayAsync(pictureByteArray, 0);

                pictureFilePath = MediaService.SavePictureAndThumbnail(pictureByteArray, photoId);

                if (pictureFilePath == null) return null;

                thumbnailFilePath = $"{pictureFilePath}_thumb"; // By convention
            }

            ImageSource imageSource = ImageSource.FromFile(thumbnailFilePath);

            return imageSource;
        }
    }
}
