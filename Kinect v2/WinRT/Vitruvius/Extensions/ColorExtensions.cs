using System;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
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
        /// The stream for the bitmap.
        /// </summary>
        static Stream _stream = null;

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
        /// Converts a color frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The bitmap representation of the color frame.</returns>
        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            if (_bitmap == null)
            {
                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(_width, _height);
                _stream = _bitmap.PixelBuffer.AsStream();
            }
            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(_pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(_pixels, ColorImageFormat.Bgra);
            }

            _stream.Seek(0, SeekOrigin.Begin);
            _stream.Write(_pixels, 0, _pixels.Length);

            _bitmap.Invalidate();

            return _bitmap;
        }

        #endregion
    }
}
