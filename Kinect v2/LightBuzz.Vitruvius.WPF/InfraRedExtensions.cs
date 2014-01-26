using Microsoft.Kinect;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating infrared frames.
    /// </summary>
    public static class InfraredExtensions
    {
        #region Public methods

        /// <summary>
        /// Converts an infrared frame to a System.Media.ImageSource.
        /// </summary>
        /// <param name="frame">The specified infrared frame.</param>
        /// <returns>The specified frame in a System.media.ImageSource format.</returns>
        public static ImageSource ToBitmap(this InfraredFrame frame)
        {
            ushort[] frameData = new ushort[frame.FrameDescription.Width * frame.FrameDescription.Height];
            byte[] pixels = new byte[frame.FrameDescription.Width * frame.FrameDescription.Height * (PixelFormats.Bgr32.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(frameData);

            // Convert the infrared to RGB.
            int colorPixelIndex = 0;
            for (int i = 0; i < frameData.Length; ++i)
            {
                // Get the infrared value for this pixel
                ushort ir = frameData[i];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."

                // To convert to a byte, we're discarding the least-significant bits.
                byte intensity = (byte)(ir >> 8);

                // Write out blue byte
                pixels[colorPixelIndex++] = intensity;

                // Write out green byte
                pixels[colorPixelIndex++] = intensity;

                // Write out red byte                        
                pixels[colorPixelIndex++] = intensity;

                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                // If we were outputting BGRA, we would write alpha here.
                ++colorPixelIndex;
            }

            return pixels.ToBitmap(frame.FrameDescription.Width, frame.FrameDescription.Height);
        }

        #endregion
    }
}
