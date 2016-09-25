using GoldenBook.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoldenBook.ServiceContract
{
    public interface IRestClient
    {
        /// <summary>
        /// Send an add to the azure web service
        /// </summary>
        /// <param name="ad">The ad to be sent</param>
        /// <returns>False if an exception was catched</returns>
        Task<bool> SendAd(Ad ad);

        /// <summary>
        /// Send a photo to the azure storage
        /// </summary>
        /// <param name="image">The byte array of the picture</param>
        /// <returns>The id of the picture saved in the cloud</returns>
        Task<string> SendPhoto(byte[] image);

        /// <summary>
        /// Retrieve a list of ads from the azure web services, sorted by date DESC.
        /// </summary>
        /// <param name="limit">The number of ads to retrieve.</param>
        /// <returns>A list of ads</returns>
        Task<List<Ad>> GetAds(int limit = int.MaxValue);

        /// <summary>
        /// Retrieve a specific photo
        /// </summary>
        /// <param name="id">The id of the picture to retrieve</param>
        /// <returns>The byte array of the picture</returns>
        Task<byte[]> GetPhoto(string id);
    }
}
