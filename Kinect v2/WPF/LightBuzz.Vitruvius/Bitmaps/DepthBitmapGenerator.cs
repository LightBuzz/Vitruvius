//
// Copyright (c) LightBuzz Software.
// All rights reserved.
//
// http://lightbuzz.com
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
// AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using Microsoft.Kinect;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Creates the bitmap representation of a Kinect depth frame.
    /// </summary>
    public class DepthBitmapGenerator : BitmapGenerator<DepthFrame>
    {
        #region Properties

        /// <summary>
        /// Returns the RGB pixel values with the players highlighted.
        /// </summary>
        public byte[] HighlightedPixels { get; protected set; }

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
        public override void Update(DepthFrame frame)
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
