using GalaSoft.MvvmLight.Ioc;
using GoldenBook.CommonServices;
using GoldenBook.ServiceContract;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace GoldenBook.ViewModel.IoC
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IAdvertisersViewModel, AdvertisersViewModel>();
            SimpleIoc.Default.Register<IAdvertisersFormViewModel, AdvertisersFormViewModel>();
            SimpleIoc.Default.Register<IUserConfigurationViewModel, UserConfigurationViewModel>();
            SimpleIoc.Default.Register<IRestClient, RestClient>();
        }

        public IAdvertisersViewModel Advertisers             => ServiceLocator.Current.GetInstance<IAdvertisersViewModel>();
        public IAdvertisersFormViewModel AdvertisersForm     => ServiceLocator.Current.GetInstance<IAdvertisersFormViewModel>();
        public IUserConfigurationViewModel UserConfiguration => ServiceLocator.Current.GetInstance<IUserConfigurationViewModel>();
        public IMediaService MediaService                    => ServiceLocator.Current.GetInstance<IMediaService>();
    }
}
