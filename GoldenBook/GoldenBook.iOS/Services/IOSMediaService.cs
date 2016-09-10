using GoldenBook.ServiceContract;
using System;
using System.IO;

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
            return null;
        }

        public Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false)
        {
            return null;
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
            return picture;
        }

        #endregion
    }
}
