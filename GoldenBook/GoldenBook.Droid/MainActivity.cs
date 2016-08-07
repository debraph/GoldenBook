
using Android.App;
using Android.Content.PM;
using Android.OS;
using XLabs.Ioc;
using XLabs.Forms;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using GalaSoft.MvvmLight.Ioc;
using GoldenBook.ServiceContract;
using GoldenBook.Droid.Services;

namespace GoldenBook.Droid
{
    [Activity(Label = "GoldenBook", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : XFormsApplicationDroid
    {
        private bool _initialized = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (!_initialized) SetIoc();

            RequestedOrientation = ScreenOrientation.Portrait;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init(); // Initialize the Azure web services for Android
            
            LoadApplication(new App());
        }
        
        void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            var app = new XFormsAppDroid();
            app.Init(this);

            resolverContainer.Register <IDevice > (t => AndroidDevice.CurrentDevice)
            .Register <IDisplay> (t => t.Resolve < IDevice > ().Display)
            .Register <IDependencyContainer> (resolverContainer).Register<IXFormsApp> (app);
            Resolver.SetResolver(resolverContainer.GetResolver());

            SimpleIoc.Default.Register<IMediaService, AndroidMediaService>();

            _initialized = true;
        }
    }
}

