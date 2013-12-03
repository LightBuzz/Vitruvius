using System;
using System.Windows.Media;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating color frames.
    /// </summary>
    public static class ColorExtensions
    {
        #region Public methods

        /// <summary>
        /// Converts a color frame to a System.Media.ImageSource.
        /// </summary>
        /// <param name="frame">A ColorImageFrame generated from a Kinect sensor.</param>
        /// <param name="format">The pixel format.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static ImageSource ToBitmap(this ColorImageFrame frame, System.Windows.Media.PixelFormat format)
        {
            byte[] pixels = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixels);

            return pixels.ToBitmap(frame.Width, frame.Height, format);
        }

        /// <summary>
        /// Converts a color frame to a System.Media.ImageSource.
        /// </summary>
        /// <param name="frame">A ColorImageFrame generated from a Kinect sensor.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static ImageSource ToBitmap(this ColorImageFrame frame)
        {
            return frame.ToBitmap(PixelFormats.Bgr32);
        }

        /// <summary>
        /// Converts a color frame to a System.Media.ImageSource with its background removed.
        /// </summary>
        /// <param name="frame">A BackgroundRemovedColorFrame generated from a Kinect sensor.</param>
        /// <param name="format">The pixel format.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static ImageSource ToBitmap(this BackgroundRemovedColorFrame frame, System.Windows.Media.PixelFormat format)
        {
            byte[] pixels = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixels);

            return pixels.ToBitmap(frame.Width, frame.Height, format);
        }

        /// <summary>
        /// Converts a color frame to a System.Media.ImageSource with its background removed.
        /// </summary>
        /// <param name="frame">A BackgroundRemovedColorFrame generated from a Kinect sensor.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static ImageSource ToBitmap(this BackgroundRemovedColorFrame frame)
        {
            return frame.ToBitmap(PixelFormats.Bgra32);
        }

        #endregion
    }
}
