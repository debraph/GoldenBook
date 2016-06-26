using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GoldenBook.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            Children.Add(new AdvertisersFormPage());
            Children.Add(new AdvertisersPage());
        }
    }
}
