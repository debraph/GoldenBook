using GoldenBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenBook.ViewModel.Interfaces
{
    public interface IAdvertisersViewModel
    {
        /// <summary>
        /// Get the list of ads from the Azure services
        /// </summary>
        Task RefreshAdsAsync();

        /// <summary>
        /// The list of Ads not refreshed from the web services
        /// </summary>
        List<Ad> Ads { get; }
    }
}
