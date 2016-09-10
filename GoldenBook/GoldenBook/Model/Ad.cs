using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace GoldenBook.Model
{
    public class Ad
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("addedBy")]
        public string AddedBy { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("photoId")]
        public string PhotoId { get; set; }

        [JsonIgnore]
        public ImageSource Picture { get; set; }

        [JsonIgnore]
        public string DisplayedName => $"{FirstName} {LastName}";

        [JsonIgnore]
        public string DisplayedAmount => $"CHF {Amount}.-";

        [JsonIgnore]
        public string DisplayedDate => $"{CreatedAt.Day}.{CreatedAt.Month}.{CreatedAt.Year}";
    }
}
