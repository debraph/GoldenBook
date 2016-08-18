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
    }
}
