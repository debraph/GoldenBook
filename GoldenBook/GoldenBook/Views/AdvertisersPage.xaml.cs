using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GoldenBook.Views
{
    public partial class AdvertisersPage : ContentPage
    {
        public AdvertisersPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.Advertisers;
        }
    }
}
