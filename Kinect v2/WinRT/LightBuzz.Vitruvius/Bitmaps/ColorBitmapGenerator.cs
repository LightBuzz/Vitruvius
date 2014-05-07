using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Creates a bitmap representation of a Kinect color frame.
    /// </summary>
    public class ColorBitmapGenerator : IBitmapGenerator<ColorFrame>
    {
        #region Properties

        /// <summary>
        /// Returns the RGB pixel values.
        /// </summary>
        public byte[] Pixels { get; protected set; }

        /// <summary>
        /// Returns the width of the bitmap.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Returns the height of the bitmap.
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Returns the stream of the bitmap.
        /// </summary>
        public Stream Stream { get; protected set; }

        /// <summary>
        /// Returns the actual bitmap.
        /// </summary>
        public WriteableBitmap Bitmap { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the bitmap with new frame data.
        /// </summary>
        /// <param name="frame">The specified Kinect color frame.</param>
        public void Update(ColorFrame frame)
        {
            if (Bitmap == null)
            {
                Width = frame.FrameDescription.Width;
                Height = frame.FrameDescription.Height;
                Pixels = new byte[Width * Height * Constants.BYTES_PER_PIXEL];
                Bitmap = new WriteableBitmap(Width, Height);
                Stream = Bitmap.PixelBuffer.AsStream();
            }
            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(Pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(Pixels, ColorImageFormat.Bgra);
            }

            Stream.Seek(0, SeekOrigin.Begin);
            Stream.Write(Pixels, 0, Pixels.Length);

            Bitmap.Invalidate();
        }

        #endregion
    }
}
