using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.WinForms
{
    /// <summary>
    /// Provides some common functionality for manipulating WinForms bitmap images.
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
        /// Creates a System.Drawing.Bitmap image.
        /// </summary>
        /// <param name="pixels">Image byte array representation.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="format">Image pixel format.</param>
        /// <returns>The corresponding System.Drawing.Bitmap.</returns>
        public static Bitmap ToBitmap(this byte[] pixels, int width, int height, System.Drawing.Imaging.PixelFormat format)
        {
            Bitmap bitmap = new Bitmap(width, height, format);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// Creates a System.Drawing.Bitmap image.
        /// </summary>
        /// <param name="pixels">Image byte array representation.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>The corresponding System.Drawing.Bitmap.</returns>
        public static Bitmap ToBitmap(this byte[] pixels, int width, int height)
        {
            return pixels.ToBitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        }

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The Bitmap image to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG, PNG and BMP formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Capture(this Bitmap bitmap, string path)
        {
            if (bitmap == null || path == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                ImageFormat format;

                switch (Path.GetExtension(path))
                {
                    case "jpg":
                    case "JPG":
                        format = ImageFormat.Jpeg;
                        break;
                    case "png":
                    case "PNG":
                        format = ImageFormat.Png;
                        break;
                    case "bmp":
                    case "BMP":
                        format = ImageFormat.Bmp;
                        break;
                    default:
                        format = ImageFormat.Jpeg;
                        break;
                }

                bitmap.Save(path, format);
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
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG, PNG and BMP formats are supported.</param>
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
