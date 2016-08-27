using System;

namespace GoldenBook.ServiceContract
{
    public interface IMediaService
    {
        /// <summary>
        /// Save a picture in the current app storage for the given filename (no extension is required)
        /// </summary>
        /// <param name="picture">The binaries of the picture</param>
        /// <param name="filename">The filename without extension</param>
        /// <returns>The full file path</returns>
        string SavePicture(byte[] picture, string filename);

        /// <summary>
        /// Process a photo captured by the camera
        /// </summary>
        /// <param name="filePath">The path of the photo captured and saved locally</param>
        /// <param name="needXMirroring">Photos taken with the front camera may need to be mirrored</param>
        /// <returns>A tuple containing he filepath of the new image saved locally and bynary image</returns>
        Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false);
    }
}
