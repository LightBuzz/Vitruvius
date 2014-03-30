using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace LightBuzz.Vitruvius.WinForms
{
    /// <summary>
    /// Provides extension methods for removing the background of a Kinect frame.
    /// </summary>
    public class BackgroundRemovalTool
    {
        #region Members

        /// <summary>
        /// The bitmap source.
        /// </summary>
        Bitmap _bitmap = null;

        /// <summary>
        /// The depth values.
        /// </summary>
        ushort[] _depthData = null;

        /// <summary>
        /// The body index values.
        /// </summary>
        byte[] _bodyData = null;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        byte[] _pixels = null;

        /// <summary>
        /// The RGB pixel values used for the background removal (green-screen) effect.
        /// </summary>
        byte[] _displayPixels = null;

        /// <summary>
        /// The color points used for the background removal (green-screen) effect.
        /// </summary>
        ColorSpacePoint[] _colorPoints = null;

        /// <summary>
        /// The coordinate mapper for the background removal (green-screen) effect.
        /// </summary>
        CoordinateMapper _coordinateMapper = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of BackgroundRemovalTool.
        /// </summary>
        /// <param name="mapper">The coordinate mapper used for the background removal.</param>
        public BackgroundRemovalTool(CoordinateMapper mapper)
        {
            _coordinateMapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource and removes the background (green-screen effect).
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="colorFrame">The specified color frame.</param>
        /// <param name="bodyIndexFrame">The specified body index frame.</param>
        /// <param name="backGroundColor">The specified background color.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of image.</returns>
        public Bitmap GreenScreen(ColorFrame colorFrame, DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame, Color backgroundColor)
        {
            int colorWidth = colorFrame.FrameDescription.Width;
            int colorHeight = colorFrame.FrameDescription.Height;

            int depthWidth = depthFrame.FrameDescription.Width;
            int depthHeight = depthFrame.FrameDescription.Height;

            int bodyIndexWidth = bodyIndexFrame.FrameDescription.Width;
            int bodyIndexHeight = bodyIndexFrame.FrameDescription.Height;

            if (_displayPixels == null)
            {
                _depthData = new ushort[depthWidth * depthHeight];
                _bodyData = new byte[depthWidth * depthHeight];
                _pixels = new byte[colorWidth * colorHeight * Constants.BYTES_PER_PIXEL];
                _displayPixels = new byte[depthWidth * depthHeight * Constants.BYTES_PER_PIXEL];
                _colorPoints = new ColorSpacePoint[depthWidth * depthHeight];
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
                        int displayIndex = depthIndex * Constants.BYTES_PER_PIXEL;

                        byte player = _bodyData[depthIndex];

                        if (player != 0xff)
                        {
                            ColorSpacePoint colorPoint = _colorPoints[depthIndex];

                            int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                            int colorY = (int)Math.Floor(colorPoint.Y + 0.5);

                            if ((colorX >= 0) && (colorX < colorWidth) && (colorY >= 0) && (colorY < colorHeight))
                            {
                                int colorIndex = ((colorY * colorWidth) + colorX) * Constants.BYTES_PER_PIXEL;

                                _displayPixels[displayIndex + 0] = _pixels[colorIndex];
                                _displayPixels[displayIndex + 1] = _pixels[colorIndex + 1];
                                _displayPixels[displayIndex + 2] = _pixels[colorIndex + 2];
                                _displayPixels[displayIndex + 3] = 0xff;
                            }
                        }
                        else
                        {
                            _displayPixels[displayIndex + 0] = backgroundColor.R;
                            _displayPixels[displayIndex + 1] = backgroundColor.G;
                            _displayPixels[displayIndex + 2] = backgroundColor.B;
                            _displayPixels[displayIndex + 3] = backgroundColor.A;
                        }
                    }
                }
            }

            BitmapData bitmapData = _bitmap.LockBits(new Rectangle(0, 0, depthWidth, depthHeight), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            Marshal.Copy(_displayPixels, 0, bitmapData.Scan0, _displayPixels.Length);

            _bitmap.UnlockBits(bitmapData);

            return _bitmap;
        }

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource and removes the background (green-screen effect).
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="colorFrame">The specified color frame.</param>
        /// <param name="bodyIndexFrame">The specified body index frame.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of image.</returns>
        public Bitmap GreenScreen(ColorFrame colorFrame, DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            return GreenScreen(colorFrame, depthFrame, bodyIndexFrame, Color.Transparent);
        }

        #endregion
    }
}
