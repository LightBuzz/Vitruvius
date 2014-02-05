using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common functionality for manipulating depth frames.
    /// </summary>
    public static class DepthExtensions
    {
        #region Constants

        static readonly int BLUE_INDEX = 0;
        static readonly int GREEN_INDEX = 1;
        static readonly int RED_INDEX = 2;

        static readonly float MAX_DEPTH_DISTANCE = 4095;
        static readonly float MIN_DEPTH_DISTANCE = 850;
        static readonly float MAX_DEPTH_DISTANCE_OFFSET = MAX_DEPTH_DISTANCE - MIN_DEPTH_DISTANCE;

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a depth frame to the corresponding System.Windows.Media.ImageSource.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The corresponding System.Windows.Media.ImageSource representation of the depth frame.</returns>
        public static ImageSource ToBitmap(this DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            ushort[] pixelData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (PixelFormats.Bgr32.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(pixelData);

            // Convert the depth to RGB.
            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < pixelData.Length; ++depthIndex)
            {
                // Get the depth for this pixel
                ushort depth = pixelData[depthIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                // Values outside the reliable depth range are mapped to 0 (black).
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixels[colorIndex++] = intensity; // Blue
                pixels[colorIndex++] = intensity; // Green
                pixels[colorIndex++] = intensity; // Red

                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                // If we were outputting BGRA, we would write alpha here.
                ++colorIndex;
            }

            return pixels.ToBitmap(width, height);
        }

        #endregion
    }
}
