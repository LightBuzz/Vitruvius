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
        /// The depth bitmap creator.
        /// </summary>
        static DepthBitmapGenerator _bitmapGenerator = new DepthBitmapGenerator();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The bitmap representation of the specified depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthFrame frame)
        {
            _bitmapGenerator.Update(frame);

            return _bitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.Imaging.BitmapSource with the players highlighted.
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="bodyIndexFrame">The specified body index frame.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of the depth frame.</returns>
        public static BitmapSource ToBitmap(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            _bitmapGenerator.Update(depthFrame, bodyIndexFrame);

            return _bitmapGenerator.HighlightedBitmap;
        }

        #endregion
    }
}
