using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoldenBook.ViewModel.Interfaces
{
    public interface IAdvertisersFormViewModel
    {
        ImageSource ImageSource { get; set; }

        ICommand TakePictureCommand { get; set; }

        IEnumerable<string> Proposers { get; }
    }
}
