using GoldenBook.ServiceContract;
using System;

namespace GoldenBook.iOS.Services
{
    public class IOSMediaService : IMediaService
    {
        public string GetFilepath(string pictureId)
        {
            return null;
        }

        public Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false)
        {
            return null;
        }

        public string SavePictureAndThumbnail(byte[] picture, string filename)
        {
            return null;
        }
    }
}
