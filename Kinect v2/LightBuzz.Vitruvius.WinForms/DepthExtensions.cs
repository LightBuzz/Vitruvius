using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Kinect;
using System.Runtime.InteropServices;

namespace LightBuzz.Vitruvius.WinForms
{
    public static class DepthExtensions
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
        /// The depth values.
        /// </summary>
        static ushort[] _depthData = null;

        /// <summary>
        /// The body index values.
        /// </summary>
        static byte[] _bodyData = null;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        static byte[] _pixels = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a depth frame to the corresponding System.Drawing.Bitmap.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The corresponding System.Drawing.Bitmap representation of the depth frame.</returns>
        public static Bitmap ToBitmap(this DepthFrame frame)
        {
            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            if (_bitmap == null)
            {
                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
                _depthData = new ushort[_width * _height];
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new Bitmap(_width, _height, Constants.FORMAT);
            }

            frame.CopyFrameDataToArray(_depthData);

            // Convert the depth to RGB.
            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < _depthData.Length; ++depthIndex)
            {
                // Get the depth for this pixel
                ushort depth = _depthData[depthIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                // Values outside the reliable depth range are mapped to 0 (black).
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

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
