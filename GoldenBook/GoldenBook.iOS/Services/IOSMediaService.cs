using CoreGraphics;
using Foundation;
using GoldenBook.ServiceContract;
using System;
using System.IO;
using UIKit;
using System.Drawing;

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

			var orientation = image.Orientation;
			UIImage imageTransformed;
			switch (orientation)
			{
				case UIImageOrientation.Right:
					imageTransformed = ScaleAndRotateImage(image, UIImageOrientation.Right);
					break;
				case UIImageOrientation.Down:
					imageTransformed = ScaleAndRotateImage(image, UIImageOrientation.Down);
					break;
				case UIImageOrientation.Left:
					imageTransformed = ScaleAndRotateImage(image, UIImageOrientation.Left);
					break;
				default:
					imageTransformed = image;
					break;
			}

            var imageByteArray = UIImageToByteArray(imageTransformed);

            var id = Guid.NewGuid().ToString("n");
            var filename = $"capture-{id}";
            
            var newFilePath = SavePictureAndThumbnail(imageByteArray, filename);

            return new Tuple<string, byte[]>(newFilePath, imageByteArray);
        }

        #region Private methods

		UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
		{
			CGImage imgRef = imageIn.CGImage;
			float width = imgRef.Width;
			float height = imgRef.Height;
			CGAffineTransform transform = CGAffineTransform.MakeIdentity();
			RectangleF bounds = new RectangleF(0, 0, width, height);

			switch (orIn)
			{
				case UIImageOrientation.Up:                                        //EXIF = 1
					transform = CGAffineTransform.MakeIdentity();
					break;

				case UIImageOrientation.UpMirrored:                                //EXIF = 2
					transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
					break;

				case UIImageOrientation.Down:                                      //EXIF = 3
					transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
					break;

				case UIImageOrientation.DownMirrored:                              //EXIF = 4
					transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
					transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
					break;

				case UIImageOrientation.LeftMirrored:                              //EXIF = 5
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
					transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
					break;

				case UIImageOrientation.Left:                                      //EXIF = 6
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
					transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
					break;

				case UIImageOrientation.RightMirrored:                             //EXIF = 7
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
					break;

				case UIImageOrientation.Right:                                     //EXIF = 8
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
					break;

				default:
					throw new Exception("Invalid image orientation");
					break;
			}

			UIGraphics.BeginImageContext(bounds.Size);

			CGContext context = UIGraphics.GetCurrentContext();

			if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
			{
				context.ScaleCTM(-scaleRatio, scaleRatio);
				context.TranslateCTM(-height, 0);
			}
			else
			{
				context.ScaleCTM(scaleRatio, -scaleRatio);
				context.TranslateCTM(0, -height);
			}

			context.ConcatCTM(transform);
			context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

			UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return imageCopy;
		}

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
