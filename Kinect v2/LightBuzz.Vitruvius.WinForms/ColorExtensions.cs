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
        /// <returns>The specified frame in a System.Drawing.Bitmap format.</returns>
        public static Bitmap ToBitmap(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            byte[] pixels = new byte[frame.FrameDescription.LengthInPixels];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            return pixels.ToBitmap(width, height);
        }

        #endregion
    }
}
