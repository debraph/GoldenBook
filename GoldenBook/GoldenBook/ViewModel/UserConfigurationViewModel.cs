using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoldenBook.Helpers;
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
        private string _errorMessage;

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

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                Set(ref _errorMessage, value);
                RaisePropertyChanged(nameof(IsErrorMessageDisplayed));
            }
        }

        public bool IsErrorMessageDisplayed => ErrorMessage != null;

        private void Continue()
        {
            if(Crypto.IsPasswordCorrect(Password))
            {
                Settings.FirstName = FirstName;
                Settings.LastName  = LastName;
                
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                ErrorMessage = "Le mot de passe saisie est incorrect. Demandez le au comité d'organisation.";
            }
        }
    }
}
