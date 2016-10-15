
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using GoldenBook.iOS.Services;
using GoldenBook.ServiceContract;
using UIKit;

namespace GoldenBook.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            SimpleIoc.Default.Register<IMediaService, IOSMediaService>();

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init(); // Initialize the Azure web services for iOS

            LoadApplication(new App());

            var result = base.FinishedLaunching(app, options);

			UIApplication.SharedApplication.KeyWindow.TintColor = UIColor.FromRGB(0.0f, 0.58f, 0.86f);

			return result;
        }
    }
}
