
using Android.App;
using Android.Content.PM;
using Android.OS;
using GalaSoft.MvvmLight.Ioc;
using GoldenBook.ServiceContract;
using GoldenBook.Droid.Services;

namespace GoldenBook.Droid
{
    [Activity(Label = "Livre d'or", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SimpleIoc.Default.Register<IMediaService, AndroidMediaService>();

            RequestedOrientation = ScreenOrientation.Portrait;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init(); // Initialize the Azure web services for Android

            LoadApplication(new App());
        }
    }
}

