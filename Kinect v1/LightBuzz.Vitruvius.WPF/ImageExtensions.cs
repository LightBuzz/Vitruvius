using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Diagnostics;
using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating WPF bitmap images.
    /// </summary>
    public static class ImageExtensions
    {
        #region Constants

        /// <summary>
        /// Kinect DPI.
        /// </summary>
        static readonly double DPI = 96;

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a System.Windows.Media.ImageSource image.
        /// </summary>
        /// <param name="pixels">Image byte array representation.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="format">Image pixel format.</param>
        /// <returns>The corresponding System.Windows.Media.ImageSource.</returns>
        public static ImageSource ToBitmap(this byte[] pixels, int width, int height, System.Windows.Media.PixelFormat format)
        {
            //int stride = (width * format.BitsPerPixel + 7) / 8;
            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, DPI, DPI, format, null, pixels, stride);
        }

        /// <summary>
        /// Creates a System.Windows.Media.ImageSource image.
        /// </summary>
        /// <param name="pixels">Image byte array representation.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>The corresponding System.Windows.Media.ImageSource.</returns>
        public static ImageSource ToBitmap(this byte[] pixels, int width, int height)
        {
            return pixels.ToBitmap(width, height, PixelFormats.Bgr32);
        }

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The ImageSouce to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG and PNG formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Capture(this ImageSource bitmap, string path)
        {
            if (bitmap == null || path == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                BitmapEncoder encoder;

                switch (Path.GetExtension(path))
                {
                    case ".jpg":
                    case ".jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    default:
                        return false;
                }

                encoder.Frames.Add(BitmapFrame.Create(bitmap as BitmapSource));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(stream);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Captures the specified Kinect frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The color or depth frame to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG and PNG formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Capture(this ImageFrame frame, string path)
        {
            if (frame == null) return false;

            if (frame is ColorImageFrame)
            {
                return Capture((frame as ColorImageFrame).ToBitmap(), path);
            }
            else if (frame is DepthImageFrame)
            {
                return Capture((frame as DepthImageFrame).ToBitmap(), path);
            }

            return false;
        }

        #endregion    
    }
}
