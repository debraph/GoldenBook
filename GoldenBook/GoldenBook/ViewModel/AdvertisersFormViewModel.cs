using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoldenBook.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace GoldenBook.ViewModel
{
    public class AdvertisersFormViewModel : ViewModelBase, IAdvertisersFormViewModel
    {
        private IMediaPicker _mediaPicker = null;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public AdvertisersFormViewModel()
        {
            TakePictureCommand = new RelayCommand(() => TakePicture());
            SelectPictureCommand = new RelayCommand(() => SelectPicture());
        }

        private ImageSource _imageSource = null;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                Set(ref _imageSource, value);
            }
        }

        private ICommand _takePictureCommand = null;
        public ICommand TakePictureCommand
        {
            get { return _takePictureCommand; }

            set
            {
                Set(ref _takePictureCommand, value);
            }
        }

        private ICommand _selectPictureCommand = null;
        public ICommand SelectPictureCommand
        {
            get { return _selectPictureCommand; }
            set
            {
                Set(ref _selectPictureCommand, value);
            }
        }

        private void Setup()
        {
            if (_mediaPicker != null) return;

            var device = Resolver.Resolve<IDevice>();

            _mediaPicker = DependencyService.Get<IMediaPicker>();
            //RM: hack for working on windows phone? 
            if (_mediaPicker == null) _mediaPicker = device.MediaPicker;
        }

        private async void TakePicture()
        {
            await TakePictureAsync();
        }

        private async Task TakePictureAsync()
        {
            Setup();

            ImageSource = null;

            await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
            {
                if (t.IsFaulted) { var s = t.Exception.InnerException.ToString(); }

                else if (t.IsCanceled) { }

                else
                {
                    var mediaFile = t.Result;
                    ImageSource = ImageSource.FromStream(() => mediaFile.Source);

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        private async void SelectPicture()
        {
            await SelectPictureAsync();
        }

        private async Task SelectPictureAsync()
        {
            Setup();

            ImageSource = null;
            try
            {
                var mediaFile = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                {
                    DefaultCamera = CameraDevice.Front,
                    MaxPixelDimension = 400
                });
                ImageSource = ImageSource.FromStream(() => mediaFile.Source);
            }

            catch (System.Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
