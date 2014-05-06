using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating color frames.
    /// </summary>
    public static class ColorExtensions
    {
        #region Members

        /// <summary>
        /// The color bitmap creator.
        /// </summary>
        static ColorBitmapCreator _colorBitmapCreator = new ColorBitmapCreator();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a color frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The bitmap representation of the specified color frame.</returns>
        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            _colorBitmapCreator.Update(frame);

            return _colorBitmapCreator.Bitmap;
        }

        #endregion
    }
}
