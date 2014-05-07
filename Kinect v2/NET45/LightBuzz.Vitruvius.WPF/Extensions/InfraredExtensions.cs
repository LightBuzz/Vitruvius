using Microsoft.Kinect;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius.WPF
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
        static InfraredBitmapGenerator _infraredBitmapCreator = new InfraredBitmapGenerator();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts an infrared frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified infrared frame.</param>
        /// <returns>The bitmap representation of the specified infrared frame.</returns>
        public static ImageSource ToBitmap(this InfraredFrame frame)
        {
            _infraredBitmapCreator.Update(frame);

            return _infraredBitmapCreator.Bitmap;
        }

        #endregion
    }
}
