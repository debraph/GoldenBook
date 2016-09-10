using CoreGraphics;
using Foundation;
using GoldenBook.ServiceContract;
using System;
using System.IO;
using UIKit;

namespace GoldenBook.iOS.Services
{
    public class IOSMediaService : IMediaService
    {
        private string AppDirectoryPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GoldenBook");
        private string FilePath(string filename) => Path.Combine(AppDirectoryPath, filename);

        public string SavePictureAndThumbnail(byte[] picture, string filename)
        {
            try
            {
                var thumbnails = CreateThumbnails(picture);

                if (!Directory.Exists(AppDirectoryPath))
                {
                    var info = Directory.CreateDirectory(AppDirectoryPath);
                    if (!info.Exists) return null;
                }

                var filePath = FilePath(filename);
                var thumbnailPath = $"{filePath}_thumb";

                if (!WriteFile(picture, filePath)) return null;
                if (!WriteFile(thumbnails, thumbnailPath)) return null;

                return filePath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetFilepath(string pictureId)
        {
            if (pictureId == null) return null;
            return File.Exists(FilePath(pictureId)) ? FilePath(pictureId) : null;
        }

        public Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false)
        {
            UIImage image = UIImage.FromFile(filePath);
            var imageByteArray = UIImageToByteArray(image);

            var id = Guid.NewGuid().ToString("n");
            var filename = $"capture-{id}";
            
            var newFilePath = SavePictureAndThumbnail(imageByteArray, filename);

            return new Tuple<string, byte[]>(newFilePath, imageByteArray);
        }

        #region Private methods

        private bool WriteFile(byte[] picture, string filePath)
        {
            try
            {
                if (File.Exists(filePath)) return true;

                File.WriteAllBytes(filePath, picture);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte[] CreateThumbnails(byte[] picture)
        {
            float maxHeight = 200.0f;
            float maxWidth = 200.0f;

            var pictureData = NSData.FromArray(picture);
            var sourceImage = UIImage.LoadFromData(pictureData);

            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

            if (maxResizeFactor > 1) return UIImageToByteArray(sourceImage);

            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;

            UIGraphics.BeginImageContext(new CGSize(width, height));
            sourceImage.Draw(new CGRect(0, 0, width, height));

            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return UIImageToByteArray(resultImage);
        }

        private byte[] UIImageToByteArray(UIImage image)
        {
            using (NSData imageData = image.AsPNG())
            {
                var myByteArray = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                return myByteArray;
            }
        }

        #endregion
    }
}
