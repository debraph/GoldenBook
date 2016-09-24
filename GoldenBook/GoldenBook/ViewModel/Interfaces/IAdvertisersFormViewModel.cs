using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoldenBook.ViewModel.Interfaces
{
    public interface IAdvertisersFormViewModel
    {
        ImageSource ImageSource { get; set; }

        byte[] ImageByteArray { get; }

        ICommand TakePictureCommand { get; }

        ICommand SendCommand { get; }

        string Name { get; set; }

        string Email { get; set; }

        string Amount { get; set; }

        string Message { get; set; }

        string AddedBy { get; set; }

        bool IsActivityIndicatorVisible { get; }
    }
}
