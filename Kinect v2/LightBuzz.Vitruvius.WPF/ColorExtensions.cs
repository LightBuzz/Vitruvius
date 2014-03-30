using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating color frames.
    /// </summary>
    public static class ColorExtensions
    {
        #region Members

        /// <summary>
        /// The bitmap source.
        /// </summary>
        static WriteableBitmap _bitmap = null;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        static byte[] _pixels = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a color frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The specified frame in a System.Media.Imaging.BitmapSource representation of the color frame.</returns>
        public static BitmapSource ToBitmap(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            if (_bitmap == null)
            {
                _pixels = new byte[width * height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(width, height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(_pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(_pixels, ColorImageFormat.Bgra);
            }

            _bitmap.Lock();

            Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));

            _bitmap.Unlock();

            return _bitmap;
        }

        #endregion
    }
}
