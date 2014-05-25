using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating depth frames.
    /// </summary>
    public static class DepthExtensions
    {
        #region Constants

        static readonly int BLUE_INDEX = 0;
        static readonly int GREEN_INDEX = 1;
        static readonly int RED_INDEX = 2;

        static readonly float MAX_DEPTH_DISTANCE = 4095;
        static readonly float MIN_DEPTH_DISTANCE = 850;
        static readonly float MAX_DEPTH_DISTANCE_OFFSET = MAX_DEPTH_DISTANCE - MIN_DEPTH_DISTANCE;

        #endregion

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
        static short[] _depthData = null;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        static byte[] _pixels = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.ImageSource.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The corresponding System.Windows.Media.ImageSource representation of the depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthImageFrame frame)
        {
            return ToBitmap(frame, DepthImageMode.Raw);
        }

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.ImageSource.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <param name="mode">Depth frame mode.</param>
        /// <returns>The corresponding System.Windows.Media.ImageSource representation of the depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthImageFrame frame, DepthImageMode mode)
        {
            if (_bitmap == null)
            {
                _width = frame.Width;
                _height = frame.Height;
                _depthData = new short[frame.PixelDataLength];
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(_width, _height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            frame.CopyPixelDataTo(_depthData);

            switch (mode)
            {
                case DepthImageMode.Raw:
                    GenerateRawFrame();
                    break;
                case DepthImageMode.Dark:
                    GenerateDarkFrame();
                    break;
                case DepthImageMode.Colors:
                    GenerateColoredFrame();
                    break;
                case DepthImageMode.Player:
                    GeneratePlayerFrame();
                    break;
                default:
                    GenerateRawFrame();
                    break;
            }

            _bitmap.Lock();

            Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _width, _height));

            _bitmap.Unlock();

            return _bitmap;
        }

        #endregion

        #region Helpers

        private static void GenerateRawFrame()
        {
            // Bgr32  - Blue, Green, Red, empty byte
            // Bgra32 - Blue, Green, Red, transparency 
            // You must set transparency for Bgra as .NET defaults a byte to 0 = fully transparent

            // Loop through all distances and pick a RGB color based on distance
            for (int depthIndex = 0, colorIndex = 0; depthIndex < _depthData.Length && colorIndex < _pixels.Length; depthIndex++, colorIndex += 4)
            {
                // Get the player (requires skeleton tracking enabled for values).
                int player = _depthData[depthIndex] & DepthImageFrame.PlayerIndexBitmask;

                // Get the depth value.
                int depth = _depthData[depthIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // .9M or 2.95'
                if (depth <= 900) // ---> We are very close.
                {
                    _pixels[colorIndex + BLUE_INDEX] = 255;
                    _pixels[colorIndex + GREEN_INDEX] = 0;
                    _pixels[colorIndex + RED_INDEX] = 0;

                }
                // .9M - 2M or 2.95' - 6.56'
                else if (depth > 900 && depth < 2000) // ---> We are a bit further away.
                {
                    _pixels[colorIndex + BLUE_INDEX] = 0;
                    _pixels[colorIndex + GREEN_INDEX] = 255;
                    _pixels[colorIndex + RED_INDEX] = 0;
                }
                // 2M+ or 6.56'+
                else if (depth > 2000) // ---> We are the farthest.
                {
                    _pixels[colorIndex + BLUE_INDEX] = 0;
                    _pixels[colorIndex + GREEN_INDEX] = 0;
                    _pixels[colorIndex + RED_INDEX] = 255;
                }

                // Equal coloring for monochromatic histogram.
                byte intensity = CalculateIntensityFromDepth(depth);
                _pixels[colorIndex + BLUE_INDEX] = intensity;
                _pixels[colorIndex + GREEN_INDEX] = intensity;
                _pixels[colorIndex + RED_INDEX] = intensity;

                // Color all players "gold".
                if (player > 0)
                {
                    _pixels[colorIndex + BLUE_INDEX] = Colors.Gold.B;
                    _pixels[colorIndex + GREEN_INDEX] = Colors.Gold.G;
                    _pixels[colorIndex + RED_INDEX] = Colors.Gold.R;
                }
            }
        }

        private static void GeneratePlayerFrame()
        {
            // Bgr32  - Blue, Green, Red, empty byte
            // Bgra32 - Blue, Green, Red, transparency 
            // You must set transparency for Bgra as .NET defaults a byte to 0 = fully transparent

            // Loop through all distances and pick a RGB color based on distance
            for (int depthIndex = 0, colorIndex = 0; depthIndex < _depthData.Length && colorIndex < _pixels.Length; depthIndex++, colorIndex += 4)
            {
                // Get the player (requires skeleton tracking enabled for values).
                int player = _depthData[depthIndex] & DepthImageFrame.PlayerIndexBitmask;

                // Get the depth value.
                int depth = _depthData[depthIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // Color all players "gold".
                if (player > 0)
                {
                    _pixels[colorIndex + BLUE_INDEX] = Colors.Gold.B;
                    _pixels[colorIndex + GREEN_INDEX] = Colors.Gold.G;
                    _pixels[colorIndex + RED_INDEX] = Colors.Gold.R;
                }
                else
                {
                    _pixels[colorIndex + BLUE_INDEX] = 0;
                    _pixels[colorIndex + GREEN_INDEX] = 0;
                    _pixels[colorIndex + RED_INDEX] = 0;
                }
            }
        }

        private static void GenerateDarkFrame()
        {
            int depth;
            int gray;
            int loThreshold = 1220;
            int hiThreshold = 3048;

            for (int i = 0, j = 0; i < _depthData.Length; i++, j += Constants.BYTES_PER_PIXEL)
            {
                depth = _depthData[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                if (depth < loThreshold || depth > hiThreshold)
                {
                    gray = 0xFF;
                }
                else
                {
                    gray = (255 * depth / 0xFFF);
                }

                _pixels[j] = (byte)gray;
                _pixels[j + 1] = (byte)gray;
                _pixels[j + 2] = (byte)gray;
            }
        }

        private static void GenerateColoredFrame()
        {
            int depth;
            double hue;
            int loThreshold = 1220;
            int hiThreshold = 3048;
            int bytesPerPixel = 4;
            byte[] rgb = new byte[3];

            for (int i = 0, j = 0; i < _depthData.Length; i++, j += bytesPerPixel)
            {
                depth = _depthData[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                if (depth < loThreshold || depth > hiThreshold)
                {
                    _pixels[j] = 0x00;
                    _pixels[j + 1] = 0x00;
                    _pixels[j + 2] = 0x00;
                }
                else
                {
                    hue = ((360 * depth / 0xFFF) + loThreshold);
                    ConvertHslToRgb(hue, 100, 100, rgb);

                    _pixels[j] = rgb[2];  //Blue
                    _pixels[j + 1] = rgb[1];  //Green
                    _pixels[j + 2] = rgb[0];  //Red
                }
            }
        }

        private static void ConvertHslToRgb(double hue, double saturation, double lightness, byte[] rgb)
        {
            double red = 0.0;
            double green = 0.0;
            double blue = 0.0;
            hue = hue % 360.0;
            saturation = saturation / 100.0;
            lightness = lightness / 100.0;

            if (saturation == 0.0)
            {
                red = lightness;
                green = lightness;
                blue = lightness;
            }
            else
            {
                double huePrime = hue / 60.0;
                int x = (int)huePrime;
                double xPrime = huePrime - (double)x;
                double L0 = lightness * (1.0 - saturation);
                double L1 = lightness * (1.0 - (saturation * xPrime));
                double L2 = lightness * (1.0 - (saturation * (1.0 - xPrime)));

                switch (x)
                {
                    case 0:
                        red = lightness;
                        green = L2;
                        blue = L0;
                        break;
                    case 1:
                        red = L1;
                        green = lightness;
                        blue = L0;
                        break;
                    case 2:
                        red = L0;
                        green = lightness;
                        blue = L2;
                        break;
                    case 3:
                        red = L0;
                        green = L1;
                        blue = lightness;
                        break;
                    case 4:
                        red = L2;
                        green = L0;
                        blue = lightness;
                        break;
                    case 5:
                        red = lightness;
                        green = L0;
                        blue = L1;
                        break;
                }
            }

            rgb[0] = (byte)(255.0 * red);
            rgb[1] = (byte)(255.0 * green);
            rgb[2] = (byte)(255.0 * blue);
        }

        private static byte CalculateIntensityFromDepth(int distance)
        {
            // Formula for calculating monochrome intensity for histogram.
            return (byte)(255 - (255 * Math.Max(distance - MIN_DEPTH_DISTANCE, 0) / (MAX_DEPTH_DISTANCE_OFFSET)));
        }

        #endregion
    }

    /// <summary>
    /// Represents the depth image mode (raw pixels, grayscale, colored).
    /// </summary>
    public enum DepthImageMode
    {
        /// <summary>
        /// The simplest representation of a depth image.
        /// </summary>
        Raw,

        /// <summary>
        /// Depth image representation in a grayscale format.
        /// </summary>
        Dark,

        /// <summary>
        /// Colored depth image representation.
        /// </summary>
        Colors,

        /// <summary>
        /// Player depth frame representation.
        /// </summary>
        Player
    }
}
