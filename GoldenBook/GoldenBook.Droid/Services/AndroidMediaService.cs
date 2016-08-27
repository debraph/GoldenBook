using GoldenBook.ServiceContract;
using Android.Graphics;
using Android.Media;
using Java.IO;
using System;

namespace GoldenBook.Droid.Services
{
    public class AndroidMediaService : IMediaService
    {
        public string SavePicture(byte[] picture, string filename)
        {
            try
            {
                File directory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "GoldenBook");
                if (!directory.Exists())
                {
                    // Try to create the directory if it doesn't exist
                    if (!directory.Mkdirs()) return null;
                }

                var filePath = $"{directory.AbsolutePath}{File.Separator}{filename}.jpg";

                File file = new File(filePath);

                if (file.Exists()) return filePath; // The file already exist in the directory

                FileOutputStream fos = new FileOutputStream(file);
                fos.Write(picture);
                fos.Close();

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

            Bitmap resultBitmap = BitmapFactory.DecodeFile(filePath);
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

            if(needXMirroring) mtx.PreScale(1, -1);

            resultBitmap = Bitmap.CreateBitmap(resultBitmap, 0, 0, resultBitmap.Width, resultBitmap.Height, mtx, false);
            mtx.Dispose();
            mtx = null;

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            var compressionResult = resultBitmap.Compress(Bitmap.CompressFormat.Jpeg, 80, stream);

            File directory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "GoldenBook");
            if (!directory.Exists())
            {
                if (!directory.Mkdirs()) return null;
            }

            var id = Guid.NewGuid().ToString("n");
            var newFilePath = $"{directory.AbsolutePath}{File.Separator}photo_{id}.jpg";

            File file = new File(newFilePath);
            FileOutputStream fos = new FileOutputStream(file);

            var byteArray = stream.GetBuffer();

            fos.Write(byteArray);
            fos.Close();
            
            return new Tuple<string, byte[]>(filePath, byteArray);
        }
    }
}