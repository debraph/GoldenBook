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
        public AdvertisersViewModel()
        { }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");
        private string Sas => "https://goldenbook.blob.core.windows.net/golden-book-photos?sv=2015-04-05&sr=c&sig=hnDVgepWpsAbX7Lj9o1h%2FgN7t3Va3A3meBGoMejx%2Fwc%3D&se=2017-08-18T19%3A13%3A55Z&sp=rwdl";

        public async Task<List<Ad>> GetRefreshedAdsAsync()
        {
            try
            {
                List<Ad> ads = await MobileService.GetTable<Ad>().OrderByDescending(a => a.CreatedAt).ToListAsync();

                foreach(Ad ad in ads)
                {
                    ad.Picture = await LoadPicture(ad.PhotoId);
                }

                return ads;
            }
            catch (Exception ex)
            {
                return new List<Ad>();
            }
        }

        private IMediaService MediaService => ServiceLocator.Current.GetInstance<IMediaService>();

        private async Task<ImageSource> LoadPicture(string photoId)
        {
            if (photoId == null) return null;

            CloudBlobContainer container = new CloudBlobContainer(new Uri(Sas));

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(photoId);

            await blockBlob.FetchAttributesAsync();

            long fileByteLength = blockBlob.Properties.Length;

            var pictureByteArray = new byte[fileByteLength];

            await blockBlob.DownloadToByteArrayAsync(pictureByteArray, 0);

            var filePathCreated = MediaService.SavePictureAndThumbnail(pictureByteArray, photoId);

            if (filePathCreated == null) return null;

            var thumbnailPath = $"{filePathCreated}_thumb"; // By convention

            ImageSource imageSource = ImageSource.FromFile(thumbnailPath);

            return imageSource;
        }
    }
}
