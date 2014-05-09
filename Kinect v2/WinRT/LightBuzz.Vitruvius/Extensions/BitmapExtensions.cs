using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Diagnostics;
using WindowsPreview.Kinect;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for manipulating WPF bitmap images.
    /// </summary>
    public static class BitmapExtensions
    {
        #region Public methods

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The ImageSouce to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this WriteableBitmap bitmap, StorageFile file, int width, int height)
        {
            if (bitmap == null || file == null) return false;

            try
            {
                Guid encoderID;

                switch (Path.GetExtension(file.FileType))
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".JPG":
                    case ".JPEG":
                        encoderID = BitmapEncoder.JpegEncoderId;
                        break;
                    case ".png":
                    case ".PNG":
                        encoderID = BitmapEncoder.PngEncoderId;
                        break;
                    case ".bmp":
                    case ".BMP":
                        encoderID = BitmapEncoder.BmpEncoderId;
                        break;
                    case ".tiff":
                    case ".TIFF":
                        encoderID = BitmapEncoder.TiffEncoderId;
                        break;
                    case ".gif":
                    case ".GIF":
                        encoderID = BitmapEncoder.GifEncoderId;
                        break;
                    default:
                        return false;
                }

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {                    
                    Stream pixelStream = bitmap.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderID, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, Constants.DPI, Constants.DPI, pixels);

                    if (bitmap.PixelWidth != width || bitmap.PixelHeight != height)
                    {
                        encoder.BitmapTransform.ScaledWidth = (uint)width;
                        encoder.BitmapTransform.ScaledHeight = (uint)height;
                    }

                    await encoder.FlushAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The ImageSouce to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this WriteableBitmap bitmap, StorageFile file)
        {
            return await Capture(bitmap, file, bitmap.PixelWidth, bitmap.PixelHeight);
        }

        /// <summary>
        /// Captures the specified Kinect color frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The color frame to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this ColorFrame frame, StorageFile file)
        {
            if (frame == null) return false;

            return await Capture(frame.ToBitmap(), file);
        }

        /// <summary>
        /// Captures the specified Kinect color frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The color frame to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this ColorFrame frame, StorageFile file, int width, int height)
        {
            if (frame == null) return false;

            return await Capture(frame.ToBitmap(), file, width, height);
        }

        /// <summary>
        /// Captures the specified Kinect depth frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The depth frame to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this DepthFrame frame, StorageFile file)
        {
            if (frame == null) return false;

            return await Capture(frame.ToBitmap(), file);
        }

        /// <summary>
        /// Captures the specified Kinect depth frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The depth frame to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this DepthFrame frame, StorageFile file, int width, int height)
        {
            if (frame == null) return false;

            return await Capture(frame.ToBitmap(), file, width, height);
        }

        /// <summary>
        /// Captures the specified Kinect infrared frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The infrared frame to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this InfraredFrame frame, StorageFile file)
        {
            if (frame == null) return false;

            return await Capture(frame.ToBitmap(), file);
        }

        /// <summary>
        /// Captures the specified Kinect infrared frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The infrared frame to capture.</param>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Capture(this InfraredFrame frame, StorageFile file, int width, int height)
        {
            if (frame == null) return false;

            return await Capture(frame.ToBitmap(), file, width, height);
        }

        #endregion
    }
}
