using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GoldenBook.ServiceContract;
using GoldenBook.ViewModel.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using System;
using GoldenBook.Model;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;

namespace GoldenBook.ViewModel
{
    public class AdvertisersFormViewModel : ViewModelBase, IAdvertisersFormViewModel
    {
        private string _name;
        private string _email;
        private string _amount;
        private string _message;
        private string _addedBy;
        private bool _isActivityIndicatorVisible = false;
        private ImageSource _imageSource;
        private IMediaPicker _mediaPicker = null;
        private ICommand _takePictureCommand;
        private ICommand _sendCommand;

        private Page _page;

        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public AdvertisersFormViewModel()
        {
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

        public string AddedBy
        {
            get { return _addedBy; }
            set { Set(ref _addedBy, value); }
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

        private IMediaService MediaService => ServiceLocator.Current.GetInstance<IMediaService>();

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
                    photoId = await InsertImage(image);

                    if (photoId == null) return;
                }

                Ad ad = new Ad()
                {
                    Name = Name,
                    Email = Email,
                    Message = Message,
                    CreatedAt = DateTime.Now,
                    Amount = amount,
                    AddedBy = AddedBy,
                    PhotoId = photoId,
                };

                await InsertAd(ad);

                // On success the object is updated by the service
                if (ad.Id != null) _page?.DisplayAlert("Succès de l'envoi", "Merci de votre soutien !", "Ok");
                else               _page?.DisplayAlert("Echec de l'envoi", "Réessayer et si le problème persiste contacter le comité d'organisation.", "Ok");
            }
            catch { }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        private async Task<string> InsertImage(byte[] image)
        {
            try
            {
                CloudBlobContainer container = new CloudBlobContainer(new Uri(Sas));

                var guid = Guid.NewGuid().ToString("n");
                string photoId = $"photo-{guid}";

                CloudBlockBlob blob = container.GetBlockBlobReference(photoId);

                MemoryStream msWrite = new
                MemoryStream(image);
                msWrite.Position = 0;
                using (msWrite)
                {
                    await blob.UploadFromStreamAsync(msWrite);
                }
                return photoId;
            }
            catch (Exception ex)
            {
                await _page?.DisplayAlert("Erreur lors de l'envoi de la photo", "Réessayer et si le problème persiste contacter le comité d'organisation.", "Ok");
                return null;
            }
        }

        private async Task InsertAd(Ad ad)
        {
            try
            {
                await MobileService.GetTable<Ad>().InsertAsync(ad); //TODO: Move it into a dedicated class (RestClient)
            }
            catch (Exception ex)
            {
                await _page?.DisplayAlert("Erreur lors de l'envoi", "Réessayer et si le problème persiste contacter le comité d'organisation.", "Ok");
            }
        }

        private MobileServiceClient MobileService => new MobileServiceClient("https://goldenbook.azurewebsites.net");
        private string Sas => "https://goldenbook.blob.core.windows.net/golden-book-photos?sv=2015-04-05&sr=c&sig=hnDVgepWpsAbX7Lj9o1h%2FgN7t3Va3A3meBGoMejx%2Fwc%3D&se=2017-08-18T19%3A13%3A55Z&sp=rwdl";

        private async Task TakePictureAsync()
        {
            SetupMediaPicker();

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

        private void SetupMediaPicker()
        {
            if (_mediaPicker != null) return;

            var device = Resolver.Resolve<IDevice>();

            _mediaPicker = DependencyService.Get<IMediaPicker>();
            //RM: hack for working on windows phone? 
            if (_mediaPicker == null) _mediaPicker = device.MediaPicker;
        }
    }
}
