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

        IEnumerable<string> Proposers { get; }
    }
}
