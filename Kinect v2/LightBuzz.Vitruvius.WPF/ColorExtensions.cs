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
            byte[] pixels = new byte[frame.FrameDescription.Width * frame.FrameDescription.Height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            return pixels.ToBitmap(frame.FrameDescription.Width, frame.FrameDescription.Height);
        }

        #endregion
    }
}
