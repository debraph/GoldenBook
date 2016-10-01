
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using GoldenBook.iOS.Services;
using GoldenBook.ServiceContract;
using UIKit;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;

namespace GoldenBook.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        private bool _initialized = false;
        
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            if (!_initialized) SetIoc();

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init(); // Initialize the Azure web services for iOS

            LoadApplication(new App());

            var result = base.FinishedLaunching(app, options);

			UIApplication.SharedApplication.KeyWindow.TintColor = UIColor.FromRGB(0.0f, 0.58f, 0.86f);

			return result;
        }

        void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            var app = new XFormsAppiOS();
            app.Init(this);

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
            .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
            .Register<IDependencyContainer>(resolverContainer).Register<IXFormsApp>(app);
            Resolver.SetResolver(resolverContainer.GetResolver());

            SimpleIoc.Default.Register<IMediaService, IOSMediaService>();

            _initialized = true;
        }
    }
}
