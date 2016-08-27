using System;

namespace GoldenBook.ServiceContract
{
    public interface IMediaService
    {
        /// <summary>
        /// Save a picture and its thumbnail in the current app storage for the given filename
        /// A suffix _thumb will be added for the thumbnail path
        /// </summary>
        /// <param name="picture">The picture to save</param>
        /// <param name="filename">The filename without extension</param>
        /// <returns>The full file path of the image</returns>
        string SavePictureAndThumbnail(byte[] picture, string filename);

        /// <summary>
        /// Process a photo captured by the camera
        /// </summary>
        /// <param name="filePath">The path of the photo captured and saved locally</param>
        /// <param name="needXMirroring">Photos taken with the front camera may need to be mirrored</param>
        /// <returns>A tuple containing he filepath of the new image saved locally and bynary image</returns>
        Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false);
    }
}
