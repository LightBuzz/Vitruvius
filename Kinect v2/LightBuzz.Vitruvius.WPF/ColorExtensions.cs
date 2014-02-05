using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.IO;
using System.Windows.Media.Imaging;

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
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static ImageSource ToBitmap(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            byte[] pixels = new byte[width * height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

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
