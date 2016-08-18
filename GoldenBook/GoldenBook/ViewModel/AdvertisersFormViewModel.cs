using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoldenBook.ServiceContract;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using System;
using System.Collections.Generic;

namespace GoldenBook.ViewModel
{
    public class AdvertisersFormViewModel : ViewModelBase, IAdvertisersFormViewModel
    {
        private IMediaPicker _mediaPicker = null;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public AdvertisersFormViewModel()
        {
            TakePictureCommand = new RelayCommand(() => TakePicture());
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

        public byte[] ImageByteArray
        {
            get; private set;
        }

        private IMediaService MediaService => ServiceLocator.Current.GetInstance<IMediaService>();

        public IEnumerable<string> Proposers
        {
            get
            {
                return new List<string>()
                {
                    "Bruce Wayne",
                    "Clark Kent",
                    "Tony Stark",
                    "Peter Parker",
                };
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
                    var result = t.Result;
                    var needXMirroring = false; //TODO: Determine if the photo is a selfie

                    var mediaResult = MediaService.ProcessCapturedPhoto(result.Path, needXMirroring);

                    var filePath = mediaResult.Item1;
                    ImageByteArray = mediaResult.Item2;

                    ImageSource = ImageSource.FromFile(filePath);

                    return result.Source;
                }

                return null;
            }, _scheduler);
        }
    }
}
