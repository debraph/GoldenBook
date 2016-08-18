using GoldenBook.Model;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GoldenBook.Views
{
    public partial class AdvertisersFormPage : ContentPage
    {
        public AdvertisersFormPage()
        {
            InitializeComponent();

            PopulateProposersPicker();
        }

        private IAdvertisersFormViewModel ViewModel => ServiceLocator.Current.GetInstance<IAdvertisersFormViewModel>();

        private void PopulateProposersPicker()
        {
            foreach(var proposer in ViewModel.Proposers) proposersPicker.Items.Add(proposer);
        }

        private async void OnSendButton_Clicked(object sender, EventArgs args)
        {
            string addedBy;
            try   { addedBy = proposersPicker.Items[proposersPicker.SelectedIndex]; }
            catch { addedBy = string.Empty; }

            float amount;
            var result = float.TryParse(amountEntry.Text, out amount);
            if(!result) amount = 0.0f;

            //TODO: Move the section below in a dedicated class (RestClient)
            
            var image = ViewModel.ImageByteArray;
            var photoId = await Insert(image);

            if (photoId == null) return;

            Ad ad = new Ad()
            {
                FirstName = firstNameEntry.Text,
                LastName = lastNameEntry.Text,
                Email = emailEntry.Text,
                Message = messageEditor.Text,
                CreatedAt = DateTime.Now,
                Amount = amount,
                AddedBy = addedBy,
                PhotoId = photoId,
            };

            Insert(ad);

            //TODO: Navigate to the ads list page and refresh the list
        }

        private async Task<string> Insert(byte[] image)
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
            catch(Exception ex)
            {
                return null;
            }
        }

        private async void Insert(Ad ad)
        {
            await MobileService.GetTable<Ad>().InsertAsync(ad); //TODO: Move it into a dedicated class (RestClient)
        }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");
        private string Sas => "https://goldenbook.blob.core.windows.net/golden-book-photos?sv=2015-04-05&sr=c&sig=hnDVgepWpsAbX7Lj9o1h%2FgN7t3Va3A3meBGoMejx%2Fwc%3D&se=2017-08-18T19%3A13%3A55Z&sp=rwdl";
    }
}
