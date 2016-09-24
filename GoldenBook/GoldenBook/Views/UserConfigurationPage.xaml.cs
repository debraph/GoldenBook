
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using Xamarin.Forms;

namespace GoldenBook.Views
{
    public partial class UserConfigurationPage : ContentPage
    {
        public UserConfigurationPage()
        {
            InitializeComponent();
        }

        private IUserConfigurationViewModel ViewModel => ServiceLocator.Current.GetInstance<IUserConfigurationViewModel>();

        private void OnButtonContinueClicked(object sender, EventArgs e)
        {
            ViewModel?.ContinueCommand?.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
