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
