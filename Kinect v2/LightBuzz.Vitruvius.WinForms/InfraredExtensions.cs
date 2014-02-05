using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.WinForms
{
    /// <summary>
    /// Provides some common functionality for manipulating infrared frames.
    /// </summary>
    public static class InfraredExtensions
    {
        #region Public methods

        /// <summary>
        /// Converts an infrared frame to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="frame">The specified infrared frame.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static Bitmap ToBitmap(this InfraredFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            ushort[] frameData = new ushort[width * height];
            byte[] pixels = new byte[width * height * 4];

            frame.CopyFrameDataToArray(frameData);

            // Convert the infrared to RGB.
            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < frameData.Length; ++infraredIndex)
            {
                // Get the infrared value for this pixel
                ushort ir = frameData[infraredIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                byte intensity = (byte)(ir >> 8);

                pixels[colorIndex++] = intensity; // Blue
                pixels[colorIndex++] = intensity; // Green   
                pixels[colorIndex++] = intensity; // Red

                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                // If we were outputting BGRA, we would write alpha here.
                ++colorIndex;
            }

            return pixels.ToBitmap(frame.FrameDescription.Width, frame.FrameDescription.Height);
        }

        #endregion
    }
}
