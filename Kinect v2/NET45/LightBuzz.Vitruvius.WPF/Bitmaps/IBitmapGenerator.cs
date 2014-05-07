using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Interface describing a bitmap creation tool.
    /// </summary>
    /// <typeparam name="T">The type of frame (Color, Depth, Infrared).</typeparam>
    public interface IBitmapGenerator<T>
    {
        /// <summary>
        /// Returns the RGB pixel values.
        /// </summary>
        byte[] Pixels { get; }

        /// <summary>
        /// Returns the width of the bitmap.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Returns the height of the bitmap.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Returns the actual bitmap.
        /// </summary>
        WriteableBitmap Bitmap { get; }

        /// <summary>
        /// Updates the bitmap with new frame data.
        /// </summary>
        /// <param name="frame">The specified Kinect frame.</param>
        void Update(T frame);
    }
}
