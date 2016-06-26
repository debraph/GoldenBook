using GalaSoft.MvvmLight;
using GoldenBook.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenBook.ViewModel
{
    public class AdvertisersViewModel : ViewModelBase, IAdvertisersViewModel
    {
        public AdvertisersViewModel()
        {

        }

        public string PageDescriptionMessage => "[The list of the advertisers will be printed here ...]";
    }
}
