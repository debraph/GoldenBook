using GalaSoft.MvvmLight.Ioc;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IAdvertisersViewModel Advertisers => ServiceLocator.Current.GetInstance<IAdvertisersViewModel>();
        public IAdvertisersFormViewModel AdvertisersForm => ServiceLocator.Current.GetInstance<IAdvertisersFormViewModel>();
    }
}
