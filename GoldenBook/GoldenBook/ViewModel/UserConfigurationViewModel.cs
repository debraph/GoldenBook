using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoldenBook.ViewModel.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoldenBook.ViewModel
{
    public class UserConfigurationViewModel : ViewModelBase, IUserConfigurationViewModel
    {
        private ICommand _continueCommand;
        private string _firstName;
        private string _lastName;
        private string _password;

        public static string AppSettingFirstNameKey = "GB_FirstName_Key";
        public static string AppSettingLastNameKey = "GB_LastName_Key";

        public UserConfigurationViewModel()
        {
            ContinueCommand = new RelayCommand(() => Continue());
        }

        public ICommand ContinueCommand
        {
            get { return _continueCommand; }
            private set { Set(ref _continueCommand, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private void Continue()
        {
            if(IsPasswordCorrect())
            {
                SaveAppSetting(key: AppSettingFirstNameKey, value: FirstName);
                SaveAppSetting(key: AppSettingLastNameKey,  value: LastName);
                
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        //TODO: Fix, it doesn't work on Android
        private void SaveAppSetting(string key, string value) => Application.Current.Properties[key] = value;

        private bool IsPasswordCorrect()
        {
            return true;
        }
    }
}
