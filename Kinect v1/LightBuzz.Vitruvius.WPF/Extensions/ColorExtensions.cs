using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

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
        /// Frame width.
        /// </summary>
        static int _width;

        /// <summary>
        /// Frame height.
        /// </summary>
        static int _height;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        static byte[] _pixels = null;
        
        #endregion

        #region Public methods

        /// <summary>
        /// Converts a color frame to a System.Media.ImageSource.
        /// </summary>
        /// <param name="frame">A ColorImageFrame generated from a Kinect sensor.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static BitmapSource ToBitmap(this ColorImageFrame frame)
        {
            if (_bitmap == null)
            {
                _width = frame.Width;
                _height = frame.Height;
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(_width, _height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            frame.CopyPixelDataTo(_pixels);

            _bitmap.Lock();

            Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _width, _height));

            _bitmap.Unlock();

            return _bitmap;
        }

        #endregion
    }
}
