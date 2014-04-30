using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Kinect;
using System.Runtime.InteropServices;

namespace LightBuzz.Vitruvius.WinForms
{
    /// <summary>
    /// Provides some common functionality for manipulating infrared frames.
    /// </summary>
    public static class InfraredExtensions
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
        /// The infrared values.
        /// </summary>
        static ushort[] _infraredData = null;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        static byte[] _pixels = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts an infrared frame to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="frame">The specified infrared frame.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static Bitmap ToBitmap(this InfraredFrame frame)
        {
            if (_bitmap == null)
            {
                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
                _infraredData = new ushort[_width * _height];
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new Bitmap(_width, _height, Constants.FORMAT);
            }

            frame.CopyFrameDataToArray(_infraredData);

            // Convert the infrared to RGB.
            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < _infraredData.Length; ++infraredIndex)
            {
                // Get the infrared value for this pixel
                ushort ir = _infraredData[infraredIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                byte intensity = (byte)(ir >> 8);

                _pixels[colorIndex++] = intensity; // Blue
                _pixels[colorIndex++] = intensity; // Green   
                _pixels[colorIndex++] = intensity; // Red

                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                // If we were outputting BGRA, we would write alpha here.
                ++colorIndex;
            }

            BitmapData bitmapData = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            Marshal.Copy(_pixels, 0, bitmapData.Scan0, _pixels.Length);

            _bitmap.UnlockBits(bitmapData);

            return _bitmap;
        }

        #endregion
    }
}
