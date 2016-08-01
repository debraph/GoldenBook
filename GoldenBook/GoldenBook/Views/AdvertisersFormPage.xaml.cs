using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
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

        void OnSendButton_Clicked(object sender, EventArgs args)
        {
            // TODO
        }
    }
}
