using GoldenBook.ServiceContract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenBook.Model;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace GoldenBook.CommonServices
{
    public class RestClient : IRestClient
    {
        public RestClient()
        { }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");
        private string Sas                        => "https://goldenbook.blob.core.windows.net/golden-book-photos?sv=2015-04-05&sr=c&sig=hnDVgepWpsAbX7Lj9o1h%2FgN7t3Va3A3meBGoMejx%2Fwc%3D&se=2017-08-18T19%3A13%3A55Z&sp=rwdl";
        private int MaxLimit                      => 50;

        public async Task<List<Ad>> GetAds(int limit = 50)
        {
            try
            {
                if (limit > MaxLimit) limit = MaxLimit;
                var ads = await MobileService.GetTable<Ad>().Take(limit).OrderByDescending(a => a.CreatedAt).ToListAsync();
                return ads;
            }
            catch
            {
                return new List<Ad>();
            }
        }

        public async Task<byte[]> GetPhoto(string id)
        {
            try
            {
                if (id == null) return null;

                CloudBlobContainer container = new CloudBlobContainer(new Uri(Sas));

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);

                await blockBlob.FetchAttributesAsync();

                long fileByteLength = blockBlob.Properties.Length;

                var pictureByteArray = new byte[fileByteLength];

                await blockBlob.DownloadToByteArrayAsync(pictureByteArray, 0);

                return pictureByteArray;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SendAd(Ad ad)
        {
            try
            {
                await MobileService.GetTable<Ad>().InsertAsync(ad);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> SendPhoto(byte[] image)
        {
            try
            {
                CloudBlobContainer container = new CloudBlobContainer(new Uri(Sas));

                var guid = Guid.NewGuid().ToString("n");
                string photoId = $"photo-{guid}";

                CloudBlockBlob blob = container.GetBlockBlobReference(photoId);

                MemoryStream msWrite = new
                MemoryStream(image);
                msWrite.Position = 0;

                using (msWrite)
                {
                    await blob.UploadFromStreamAsync(msWrite);
                }

                return photoId;
            }
            catch
            {
                return null;
            }
        }
    }
}
