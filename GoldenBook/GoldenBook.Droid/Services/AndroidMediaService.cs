using GoldenBook.ServiceContract;
using Android.Graphics;
using Android.Media;
using Java.IO;
using System;

namespace GoldenBook.Droid.Services
{
    public class AndroidMediaService : IMediaService
    {
        private File AppDirectory                => new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "GoldenBook");
        private string FilePath(string filename) => $"{AppDirectory.AbsolutePath}{File.Separator}{filename}";

        public string SavePictureAndThumbnail(byte[] picture, string filename)
        {
            try
            {
                var thumbnails = CreateThumbnails(picture);

                if (!AppDirectory.Exists())
                {
                    // Try to create the directory if it doesn't exist
                    if (!AppDirectory.Mkdirs()) return null;
                }

                var filePath      = FilePath(filename);
                var thumbnailPath = $"{filePath}_thumb";

                if (!WriteFile(picture, filePath)) return null;
                if (!WriteFile(thumbnails, thumbnailPath)) return null;

                return filePath;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Tuple<string, byte[]> ProcessCapturedPhoto(string filePath, bool needXMirroring = false)
        {
            ExifInterface exif = new ExifInterface(filePath);
            var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);

            Bitmap bitmap = BitmapFactory.DecodeFile(filePath);
            Matrix mtx = new Matrix();
            
            switch (orientation)
            {
                case Orientation.Rotate180:
                    mtx.PreRotate(180);
                    break;
                case Orientation.Rotate90: // portrait
                    mtx.PreRotate(90);
                    break;
                case Orientation.Rotate270: // might need to flip horizontally too...
                    mtx.PreRotate(270);
                    break;
                default:
                    mtx.PreRotate(90);
                    break;
                case Orientation.Undefined: // Nexus 7 landscape...
                case Orientation.Normal: // landscape
                case Orientation.FlipHorizontal:
                case Orientation.FlipVertical:
                case Orientation.Transpose:
                case Orientation.Transverse:
                    break;
            }

            if (needXMirroring) mtx.PreScale(1, -1);

            bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
            mtx.Dispose();
            mtx = null;

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            var compressionResult = bitmap.Compress(Bitmap.CompressFormat.Jpeg, 80, stream);

            var id = Guid.NewGuid().ToString("n");
            var filename = $"capture-{id}";

            var imageByteArray = stream.GetBuffer();

            var newFilePath = SavePictureAndThumbnail(imageByteArray, filename);
            
            return new Tuple<string, byte[]>(newFilePath, imageByteArray);
        }

        public string GetFilepath(string pictureId)
        {
            if (pictureId == null) return null;

            File file = new File(FilePath(pictureId));

            if (file.Exists()) return FilePath(pictureId);

            return null;
        }

        private bool WriteFile(byte[] picture, string filePath)
        {
            try
            {
                File file = new File(filePath);

                if (file.Exists()) return true;

                FileOutputStream fos = new FileOutputStream(file);
                fos.Write(picture);
                fos.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte[] CreateThumbnails(byte[] picture)
        {
            Bitmap bitmap = BitmapFactory.DecodeByteArray(picture, 0, picture.Length);

            int maxHeight = 200;
            int maxWidth = 200;
            float scale = Math.Min(((float)maxHeight / bitmap.Width), ((float)maxWidth / bitmap.Height));

            Matrix matrix = new Matrix();
            matrix.PostScale(scale, scale);

            var thumbnail = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            var compressionResult = thumbnail.Compress(Bitmap.CompressFormat.Jpeg, 80, stream);

            return stream.GetBuffer();
        }
    }
}