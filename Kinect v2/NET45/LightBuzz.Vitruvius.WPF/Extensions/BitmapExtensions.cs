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
    public static class BitmapExtensions
    {
        #region Public methods

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
                    case ".JPG":
                    case ".jpeg":
                    case ".JPEG":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                    case ".PNG":
                        encoder = new PngBitmapEncoder();
                        break;
                    case ".bmp":
                    case ".BMP":
                        encoder = new BmpBitmapEncoder();
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
