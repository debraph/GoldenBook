using System;

namespace GoldenBook.ServiceContract
{
    public interface IMediaService
    {
        /// <summary>
        /// Process a photo captured by the camera
        /// </summary>
        /// <param name="filePath">The path of the photo captured and saved locally</param>
        /// <param name="needXMirroring">Photos taken with the front camera may need to be mirrored</param>
        /// <returns>A tuple containing he filepath of the new image saved locally and bynary image</returns>
        Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false);
    }
}
