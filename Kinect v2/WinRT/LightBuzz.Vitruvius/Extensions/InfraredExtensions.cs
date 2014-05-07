using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for manipulating infrared frames.
    /// </summary>
    public static class InfraredExtensions
    {
        #region Members

        /// <summary>
        /// The color bitmap creator.
        /// </summary>
        static InfraredBitmapGenerator _bitmapGenerator = new InfraredBitmapGenerator();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts an infrared frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified infrared frame.</param>
        /// <returns>The bitmap representation of the specified infrared frame.</returns>
        public static WriteableBitmap ToBitmap(this InfraredFrame frame)
        {
            _bitmapGenerator.Update(frame);

            return _bitmapGenerator.Bitmap;
        }

        #endregion
    }
}
