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
    /// Creates a bitmap representation of a Kinect infrared frame.
    /// </summary>
    public class InfraredBitmapGenerator : IBitmapGenerator<InfraredFrame>
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

        /// <summary>
        /// Returns the current infrared values.
        /// </summary>
        public ushort[] InfraredData { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the bitmap with new frame data.
        /// </summary>
        /// <param name="frame">The specified Kinect infrared frame.</param>
        public void Update(InfraredFrame frame)
        {
            if (Bitmap == null)
            {
                Width = frame.FrameDescription.Width;
                Height = frame.FrameDescription.Height;
                InfraredData = new ushort[Width * Height];
                Pixels = new byte[Width * Height * Constants.BYTES_PER_PIXEL];
                Bitmap = new WriteableBitmap(Width, Height);
                Stream = Bitmap.PixelBuffer.AsStream();
            }

            frame.CopyFrameDataToArray(InfraredData);

            // Convert the infrared to RGB.
            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < InfraredData.Length; infraredIndex++)
            {
                // Get the infrared value for this pixel
                ushort ir = InfraredData[infraredIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                byte intensity = (byte)(ir >> 6);

                Pixels[colorIndex++] = intensity; // Blue
                Pixels[colorIndex++] = intensity; // Green   
                Pixels[colorIndex++] = intensity; // Red

                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                // If we were outputting BGRA, we would write alpha here.
                colorIndex++;
            }

            Stream.Seek(0, SeekOrigin.Begin);
            Stream.Write(Pixels, 0, Pixels.Length);

            Bitmap.Invalidate();
        }

        #endregion
    }
}
