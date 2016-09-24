using System.Windows.Input;

namespace GoldenBook.ViewModel.Interfaces
{
    public interface IUserConfigurationViewModel
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Password { get; set; }

        string ErrorMessage { get; set; }
        bool IsErrorMessageDisplayed { get; }

        ICommand ContinueCommand { get; }
    }
}
