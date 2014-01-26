using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.WinForms
{
    /// <summary>
    /// Provides some common functionality for manipulating color frames.
    /// </summary>
    public static class ColorExtensions
    {
        #region Public methods

        /// <summary>
        /// Converts a color frame to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="frame">A ColorImageFrame generated from a Kinect sensor.</param>
        /// <param name="format">Image format.</param>
        /// <returns>The specified frame in a System.Drawing.Bitmap format.</returns>
        public static Bitmap ToBitmap(this ColorImageFrame frame, PixelFormat format)
        {
            byte[] pixels = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixels);

            return pixels.ToBitmap(frame.Width, frame.Height, format);
        }

        /// <summary>
        /// Converts a color frame to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="frame">A ColorImageFrame generated from a Kinect sensor.</param>
        /// <returns>The specified frame in a System.Drawing.Bitmap format.</returns>
        public static Bitmap ToBitmap(this ColorImageFrame frame)
        {
            return frame.ToBitmap(PixelFormat.Format32bppRgb);
        }

        #endregion
    }
}
