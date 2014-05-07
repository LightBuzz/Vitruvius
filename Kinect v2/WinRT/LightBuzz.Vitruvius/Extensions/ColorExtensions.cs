using System;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
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
        static ColorBitmapGenerator _bitmapGenerator = new ColorBitmapGenerator();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a color frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The bitmap representation of the color frame.</returns>
        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            _bitmapGenerator.Update(frame);

            return _bitmapGenerator.Bitmap;
        }

        #endregion
    }
}
