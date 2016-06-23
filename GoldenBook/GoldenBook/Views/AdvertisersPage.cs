
using Xamarin.Forms;

namespace GoldenBook.Views
{
    public class AdvertisersPage : ContentPage
    {
        public AdvertisersPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "The list of the advertisers" }
                },

                VerticalOptions   = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            Title = "Advertisers";
        }
    }
}
