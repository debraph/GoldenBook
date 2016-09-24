using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GoldenBook.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string FirstNameKey = "FirstNameKey";
        private const string LastNameKey = "LastNameKey";

        #endregion

        public static string FirstName
        {
            get { return AppSettings.GetValueOrDefault<string>(FirstNameKey); }
            set { AppSettings.AddOrUpdateValue<string>(FirstNameKey, value); }
        }

        public static string LastName
        {
            get { return AppSettings.GetValueOrDefault<string>(LastNameKey); }
            set { AppSettings.AddOrUpdateValue<string>(LastNameKey, value); }
        }

    }
}