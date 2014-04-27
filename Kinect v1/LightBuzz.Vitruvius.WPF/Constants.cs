using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Holds some commonly used Kinect constant values.
    /// </summary>
    public static class Constants
    {
        #region Constants

        /// <summary>
        /// Kinect DPI.
        /// </summary>
        public static readonly double DPI = 96.0;

        /// <summary>
        /// Default format.
        /// </summary>
        public static readonly PixelFormat FORMAT = PixelFormats.Bgr32;

        /// <summary>
        /// Bytes per pixel.
        /// </summary>
        public static readonly int BYTES_PER_PIXEL = (FORMAT.BitsPerPixel + 7) / 8;

        #endregion
    }
}