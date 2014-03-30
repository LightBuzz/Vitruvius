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

        /// <summary>
        /// The RGB pixel values used for the background removal (green-screen) effect.
        /// </summary>
        static byte[] _displayPixels = null;

        /// <summary>
        /// The color points used for the background removal (green-screen) effect.
        /// </summary>
        static ColorSpacePoint[] _colorPoints = null;

        /// <summary>
        /// The coordinate mapper for the background removal (green-screen) effect.
        /// </summary>
        static CoordinateMapper _coordinateMapper = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of the depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            if (_bitmap == null)
            {
                _depthData = new ushort[width * height];
                _pixels = new byte[width * height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(width, height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
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
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));

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
            int width = depthFrame.FrameDescription.Width;
            int height = depthFrame.FrameDescription.Height;

            ushort minDepth = depthFrame.DepthMinReliableDistance;
            ushort maxDepth = depthFrame.DepthMaxReliableDistance;

            if (_bodyData == null)
            {
                _depthData = new ushort[width * height];
                _bodyData = new byte[width * height];
                _pixels = new byte[width * height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(width, height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
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
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));

            _bitmap.Unlock();

            return _bitmap;
        }

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource and removes the background (green-screen effect).
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="colorFrame">The specified color frame.</param>
        /// <param name="bodyIndexFrame">The specified body index frame.</param>
        /// <param name="mapper">The coordinate mapper used for the background removal.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of image.</returns>
        public static BitmapSource ToBitmap(this DepthFrame depthFrame, ColorFrame colorFrame, BodyIndexFrame bodyIndexFrame, CoordinateMapper mapper)
        {
            int depthWidth = depthFrame.FrameDescription.Width;
            int depthHeight = depthFrame.FrameDescription.Height;

            int colorWidth = colorFrame.FrameDescription.Width;
            int colorHeight = colorFrame.FrameDescription.Height;

            int bodyIndexWidth = bodyIndexFrame.FrameDescription.Width;
            int bodyIndexHeight = bodyIndexFrame.FrameDescription.Height;

            if (_displayPixels == null)
            {
                _depthData = new ushort[depthWidth * depthHeight];
                _bodyData = new byte[depthWidth * depthHeight];
                _pixels = new byte[colorWidth * colorHeight * Constants.BYTES_PER_PIXEL];
                _displayPixels = new byte[depthWidth * depthHeight * Constants.BYTES_PER_PIXEL];
                _colorPoints = new ColorSpacePoint[depthWidth * depthHeight];
                _coordinateMapper = mapper;
                _bitmap = new WriteableBitmap(depthWidth, depthHeight, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            if (((depthWidth * depthHeight) == _depthData.Length) && ((colorWidth * colorHeight * Constants.BYTES_PER_PIXEL) == _pixels.Length) && ((bodyIndexWidth * bodyIndexHeight) == _bodyData.Length))
            {
                depthFrame.CopyFrameDataToArray(_depthData);

                if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                {
                    colorFrame.CopyRawFrameDataToArray(_pixels);
                }
                else
                {
                    colorFrame.CopyConvertedFrameDataToArray(_pixels, ColorImageFormat.Bgra);
                }

                bodyIndexFrame.CopyFrameDataToArray(_bodyData);

                _coordinateMapper.MapDepthFrameToColorSpace(_depthData, _colorPoints);

                Array.Clear(_displayPixels, 0, _displayPixels.Length);

                for (int y = 0; y < depthHeight; ++y)
                {
                    for (int x = 0; x < depthWidth; ++x)
                    {
                        int depthIndex = (y * depthWidth) + x;

                        byte player = _bodyData[depthIndex];

                        if (player != 0xff)
                        {
                            ColorSpacePoint colorPoint = _colorPoints[depthIndex];

                            int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                            int colorY = (int)Math.Floor(colorPoint.Y + 0.5);

                            if ((colorX >= 0) && (colorX < colorWidth) && (colorY >= 0) && (colorY < colorHeight))
                            {
                                int colorIndex = ((colorY * colorWidth) + colorX) * Constants.BYTES_PER_PIXEL;
                                int displayIndex = depthIndex * Constants.BYTES_PER_PIXEL;

                                _displayPixels[displayIndex] = _pixels[colorIndex];
                                _displayPixels[displayIndex + 1] = _pixels[colorIndex + 1];
                                _displayPixels[displayIndex + 2] = _pixels[colorIndex + 2];
                                _displayPixels[displayIndex + 3] = 0xff;
                            }
                        }
                    }
                }

                _bitmap.Lock();

                Marshal.Copy(_displayPixels, 0, _bitmap.BackBuffer, _displayPixels.Length);
                _bitmap.AddDirtyRect(new Int32Rect(0, 0, depthWidth, depthHeight));

                _bitmap.Unlock();
            }

            return _bitmap;
        }

        #endregion
    }
}
