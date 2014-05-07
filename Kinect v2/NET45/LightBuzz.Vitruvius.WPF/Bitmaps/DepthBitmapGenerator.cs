using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Creates a bitmap representation of a Kinect depth frame.
    /// </summary>
    public class DepthBitmapGenerator : IBitmapGenerator<DepthFrame>
    {
        #region Properties

        /// <summary>
        /// Returns the RGB pixel values.
        /// </summary>
        public byte[] Pixels { get; protected set; }

        /// <summary>
        /// Returns the RGB pixel values with the players highlighted.
        /// </summary>
        public byte[] HighlightedPixels { get; protected set; }

        /// <summary>
        /// Returns the width of the bitmap.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Returns the height of the bitmap.
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Returns the actual bitmap.
        /// </summary>
        public WriteableBitmap Bitmap { get; protected set; }

        /// <summary>
        /// Returns the actual bitmap with the players highlighted.
        /// </summary>
        public WriteableBitmap HighlightedBitmap { get; protected set; }

        /// <summary>
        /// Returns the current depth values.
        /// </summary>
        public ushort[] DepthData { get; protected set; }

        /// <summary>
        /// Returns the body index values.
        /// </summary>
        public byte[] BodyData { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the bitmap with new frame data.
        /// </summary>
        /// <param name="frame">The specified Kinect depth frame.</param>
        public void Update(DepthFrame frame)
        {
            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            if (Bitmap == null)
            {
                Width = frame.FrameDescription.Width;
                Height = frame.FrameDescription.Height;
                DepthData = new ushort[Width * Height];
                Pixels = new byte[Width * Height * Constants.BYTES_PER_PIXEL];
                Bitmap = new WriteableBitmap(Width, Height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            frame.CopyFrameDataToArray(DepthData);

            // Convert the depth to RGB.
            int colorIndex = 0;

            for (int depthIndex = 0; depthIndex < DepthData.Length; ++depthIndex)
            {
                // Get the depth for this pixel
                ushort depth = DepthData[depthIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                // Values outside the reliable depth range are mapped to 0 (black).
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                Pixels[colorIndex++] = intensity; // Blue
                Pixels[colorIndex++] = intensity; // Green
                Pixels[colorIndex++] = intensity; // Red

                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                // If we were outputting BGRA, we would write alpha here.
                ++colorIndex;
            }

            Bitmap.Lock();

            Marshal.Copy(Pixels, 0, Bitmap.BackBuffer, Pixels.Length);
            Bitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));

            Bitmap.Unlock();
        }

        /// <summary>
        /// Updates the bitmap with new frame data and highlights the players.
        /// </summary>
        /// <param name="depthFrame">The specified Kinect depth frame.</param>
        /// <param name="bodyIndexFrame">The specified Kinect body index frame.</param>
        public void Update(DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            ushort minDepth = depthFrame.DepthMinReliableDistance;
            ushort maxDepth = depthFrame.DepthMaxReliableDistance;

            if (BodyData == null)
            {
                Width = depthFrame.FrameDescription.Width;
                Height = depthFrame.FrameDescription.Height;
                DepthData = new ushort[Width * Height];
                BodyData = new byte[Width * Height];
                HighlightedPixels = new byte[Width * Height * Constants.BYTES_PER_PIXEL];
                HighlightedBitmap = new WriteableBitmap(Width, Height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            depthFrame.CopyFrameDataToArray(DepthData);
            bodyIndexFrame.CopyFrameDataToArray(BodyData);

            // Convert the depth to RGB
            for (int depthIndex = 0, colorPixelIndex = 0; depthIndex < DepthData.Length && colorPixelIndex < HighlightedPixels.Length; depthIndex++, colorPixelIndex += 4)
            {
                // Get the depth for this pixel
                ushort depth = DepthData[depthIndex];
                byte player = BodyData[depthIndex];

                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                // Values outside the reliable depth range are mapped to 0 (black).
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                if (player != 0xff)
                {
                    // Color player gold.
                    HighlightedPixels[colorPixelIndex + 0] = Colors.Gold.B; // B
                    HighlightedPixels[colorPixelIndex + 1] = Colors.Gold.G; // G
                    HighlightedPixels[colorPixelIndex + 2] = Colors.Gold.R; // R
                }
                else
                {
                    // Color the rest of the image in grayscale.
                    HighlightedPixels[colorPixelIndex + 0] = intensity; // B
                    HighlightedPixels[colorPixelIndex + 1] = intensity; // G
                    HighlightedPixels[colorPixelIndex + 2] = intensity; // R
                }
            }

            HighlightedBitmap.Lock();

            Marshal.Copy(HighlightedPixels, 0, HighlightedBitmap.BackBuffer, HighlightedPixels.Length);
            HighlightedBitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));

            HighlightedBitmap.Unlock();
        }

        #endregion
    }
}
