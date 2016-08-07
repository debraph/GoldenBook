using GoldenBook.Model;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
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

        private void OnSendButton_Clicked(object sender, EventArgs args)
        {
            string addedBy;
            try   { addedBy = proposersPicker.Items[proposersPicker.SelectedIndex]; }
            catch { addedBy = string.Empty; }

            float amount;
            var result = float.TryParse(amountEntry.Text, out amount);
            if(!result) amount = 0.0f;

            //TODO: Move the section below in a dedicated class (RestClient)

            Ad ad = new Ad()
            {
                FirstName = firstNameEntry.Text,
                LastName = lastNameEntry.Text,
                Email = emailEntry.Text,
                Message = messageEditor.Text,
                CreatedAt = DateTime.Now,
                Amount = amount,
                AddedBy = addedBy
            };

            Insert(ad);

            //TODO: Navigate to the ads list page and refresh the list
        }

        private async void Insert(Ad ad)
        {
            await MobileService.GetTable<Ad>().InsertAsync(ad); //TODO: Move it into a dedicated class (RestClient)
        }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");
    }
}
