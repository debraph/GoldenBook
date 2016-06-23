
using Xamarin.Forms;

namespace GoldenBook.Views
{
    public class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            this.Title = "TabbedPage";
            this.Children.Add(new AdvertisersFormPage());
            this.Children.Add(new AdvertisersPage());
        }
    }
}
