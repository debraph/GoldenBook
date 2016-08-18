using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GoldenBook.Model;
using GoldenBook.ViewModel.Interfaces;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace GoldenBook.ViewModel
{
    public class AdvertisersViewModel : ViewModelBase, IAdvertisersViewModel
    {
        public AdvertisersViewModel()
        { }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");

        public async Task<List<Ad>> GetRefreshedAdsAsync()
        {
            try
            {
                return await MobileService.GetTable<Ad>().OrderByDescending(a => a.CreatedAt).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Ad>();
            }
        }
    }
}
