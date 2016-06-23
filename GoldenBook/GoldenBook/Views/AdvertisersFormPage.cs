
using Xamarin.Forms;

namespace GoldenBook.Views
{
    public class AdvertisersFormPage : ContentPage
    {
        public AdvertisersFormPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Entry { Placeholder = "[Advertiser]", },
                    new Entry { Placeholder = "[E-mail]", Keyboard = Keyboard.Email },
                    new Entry { Placeholder = "[Sponsor]", },
                    new Editor { Text = "[Comment...]",  },
                    new Button { Text = "Send" },
                },

                VerticalOptions = LayoutOptions.Center,
            };

            Title = "Form";
        }
    }
}
