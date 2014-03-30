using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating depth frames.
    /// </summary>
    public static class DepthExtensions
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
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of the depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthFrame frame)
        {
            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            if (_bitmap == null)
            {
                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
                _depthData = new ushort[_width * _height];
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(_width, _height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
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

            _bitmap.Lock();

            Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _width, _height));

            _bitmap.Unlock();

            return _bitmap;
        }

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource with the players highlighted.
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="bodyIndexFrame">The specified body index frame.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of the depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            ushort minDepth = depthFrame.DepthMinReliableDistance;
            ushort maxDepth = depthFrame.DepthMaxReliableDistance;

            if (_bodyData == null)
            {
                _width = depthFrame.FrameDescription.Width;
                _height = depthFrame.FrameDescription.Height;
                _depthData = new ushort[_width * _height];
                _bodyData = new byte[_width * _height];
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(_width, _height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            depthFrame.CopyFrameDataToArray(_depthData);
            bodyIndexFrame.CopyFrameDataToArray(_bodyData);

            // Convert the depth to RGB
            for (int depthIndex = 0, colorPixelIndex = 0; depthIndex < _depthData.Length && colorPixelIndex < _pixels.Length; depthIndex++, colorPixelIndex += 4)
            {
                // Get the depth for this pixel
                ushort depth = _depthData[depthIndex];
                byte player = _bodyData[depthIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                // Values outside the reliable depth range are mapped to 0 (black).
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                if (player != 0xff)
                {
                    // Color player gold.
                    _pixels[colorPixelIndex + 0] = Colors.Gold.B; // B
                    _pixels[colorPixelIndex + 1] = Colors.Gold.G; // G
                    _pixels[colorPixelIndex + 2] = Colors.Gold.R; // R
                }
                else
                {
                    // Color the rest of the image in grayscale.
                    _pixels[colorPixelIndex + 0] = intensity; // B
                    _pixels[colorPixelIndex + 1] = intensity; // G
                    _pixels[colorPixelIndex + 2] = intensity; // R
                }
            }

            _bitmap.Lock();

            Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _width, _height));

            _bitmap.Unlock();

            return _bitmap;
        }

        #endregion
    }
}
