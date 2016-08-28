using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoldenBook.ViewModel.Interfaces
{
    public interface IAdvertisersFormViewModel
    {
        ImageSource ImageSource { get; set; }

        byte[] ImageByteArray { get; }

        ICommand TakePictureCommand { get; set; }

        ICommand SendCommand { get; set; }

        string Firstname { get; set; }

        string Lastname { get; set; }

        string Email { get; set; }

        string Amount { get; set; }

        string Message { get; set; }

        string AddedBy { get; set; }
    }
}
