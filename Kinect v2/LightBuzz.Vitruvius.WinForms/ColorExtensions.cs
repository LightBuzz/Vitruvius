using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Kinect;
using System.Runtime.InteropServices;

namespace LightBuzz.Vitruvius.WinForms
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
        static Bitmap _bitmap = null;

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
        /// Converts a color frame to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="frame">A ColorImageFrame generated from a Kinect sensor.</param>
        /// <returns>The specified frame in a System.Drawing.Bitmap format.</returns>
        public static Bitmap ToBitmap(this ColorFrame frame)
        {
            if (_bitmap == null)
            {
                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new Bitmap(_width, _height, Constants.FORMAT);
            }

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(_pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(_pixels, ColorImageFormat.Bgra);
            }

            BitmapData bitmapData = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            Marshal.Copy(_pixels, 0, bitmapData.Scan0, _pixels.Length);

            _bitmap.UnlockBits(bitmapData);

            return _bitmap;
        }

        #endregion
    }
}
