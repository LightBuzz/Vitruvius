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
    public static class BitmapExtensions
    {                
        #region Public methods

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
                    case "jpeg":
                    case "JPEG":
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
        /// Captures the specified Kinect color frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The color frame to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG, PNG and BMP formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Capture(this ColorFrame frame, string path)
        {
            if (frame == null) return false;

            return Capture(frame.ToBitmap(), path);
        }

        /// <summary>
        /// Captures the specified Kinect depth frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The depth frame to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG, PNG and BMP formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Capture(this DepthFrame frame, string path)
        {
            if (frame == null) return false;

            return Capture(frame.ToBitmap(), path);
        }

        /// <summary>
        /// Captures the specified Kinect infrared frame and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The infrared frame to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG, PNG and BMP formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Capture(this InfraredFrame frame, string path)
        {
            if (frame == null) return false;

            return Capture(frame.ToBitmap(), path);
        }

        #endregion
    }
}
