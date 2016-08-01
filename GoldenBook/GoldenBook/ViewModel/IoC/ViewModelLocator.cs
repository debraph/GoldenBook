using GalaSoft.MvvmLight.Ioc;
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
        }

        public IAdvertisersViewModel Advertisers         => ServiceLocator.Current.GetInstance<IAdvertisersViewModel>();
        public IAdvertisersFormViewModel AdvertisersForm => ServiceLocator.Current.GetInstance<IAdvertisersFormViewModel>();
        public IMediaService MediaService                => ServiceLocator.Current.GetInstance<IMediaService>();
    }
}
