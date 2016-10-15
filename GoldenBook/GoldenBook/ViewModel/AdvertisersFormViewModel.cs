using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoldenBook.ServiceContract;
using GoldenBook.ViewModel.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using GoldenBook.Model;
using GoldenBook.Helpers;
using Plugin.Media;

namespace GoldenBook.ViewModel
{
    public class AdvertisersFormViewModel : ViewModelBase, IAdvertisersFormViewModel
    {
        private string _name;
        private string _email;
        private string _amount;
        private string _message;
        private bool _isActivityIndicatorVisible = false;
        private ImageSource _imageSource;
        private ICommand _takePictureCommand;
        private ICommand _sendCommand;

        private Page _page;

        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IRestClient _restClient;
        private IMediaService _mediaService;

        public AdvertisersFormViewModel(IRestClient restClient, IMediaService mediaService)
        {
            _restClient  = restClient;
            _mediaService = mediaService;

            TakePictureCommand = new RelayCommand(() => TakePicture());
            SendCommand = new RelayCommand(() => Send());

            MessagingCenter.Subscribe<Page>(this, "BindingContextChanged.AdvertisersFormViewModel", (sender) => 
            {
                _page = sender;
            });
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public string Amount
        {
            get { return _amount; }
            set { Set(ref _amount, value); }
        }

        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }

        private async void TakePicture()
        {
            await TakePictureAsync();
        }

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { Set(ref _imageSource, value); }
        }

        public ICommand TakePictureCommand
        {
            get { return _takePictureCommand; }
            private set { Set(ref _takePictureCommand, value); }
        }

        public ICommand SendCommand
        {
            get { return _sendCommand; }
            private set { Set(ref _sendCommand, value); }
        }

        public byte[] ImageByteArray { get; private set; }

        private async void Send()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                float amount;
                var result = float.TryParse(Amount, out amount);
                if (!result) amount = 0.0f;

                string photoId = null;
                if(ImageByteArray != null)
                {
                    var image = ImageByteArray;
                    photoId = await _restClient.SendPhoto(image);

                    if (photoId == null)
                    {
                        await _page?.DisplayAlert("Erreur lors de l'envoi de la photo", "Réessayer et si le problème persiste contacter le comité d'organisation.", "Ok");
                        return;
                    }
                }

                Ad ad = new Ad()
                {
                    Name = Name,
                    Email = Email,
                    Message = Message,
                    CreatedAt = DateTime.Now,
                    Amount = amount,
                    AddedBy = $"{Settings.FirstName} {Settings.LastName}",
                    PhotoId = photoId,
                };

                var sendResult = await _restClient.SendAd(ad);

                if(sendResult && ad.Id != null)
                {
                    _page?.DisplayAlert("Succès de l'envoi", "Merci de votre soutien !", "Ok");
                    ResetForm();
                }
                else
                {
                    await _page?.DisplayAlert("Erreur lors de l'envoi", "Réessayer et si le problème persiste contacter le comité d'organisation.", "Ok");
                }
            }
            catch { }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        private void ResetForm()
        {
            Name           = null;
            Email          = null;
            Amount         = null;
            Message        = null;
            ImageSource    = null;
			ImageByteArray = null;
        }

        private async Task TakePictureAsync()
        {
            ImageSource = null;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) return;

            ImageSource = null;

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front });

            var mediaResult = _mediaService.ProcessCapturedPhoto(file.Path);

            var filePath = mediaResult.Item1;
            ImageByteArray = mediaResult.Item2;

            ImageSource = ImageSource.FromFile(filePath);
        }
    }
}
