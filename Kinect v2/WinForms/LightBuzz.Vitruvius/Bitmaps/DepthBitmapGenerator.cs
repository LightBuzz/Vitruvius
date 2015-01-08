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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LightBuzz.Vitruvius
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
        public Bitmap Bitmap { get; protected set; }

        /// <summary>
        /// Returns the actual bitmap with the players highlighted.
        /// </summary>
        public Bitmap HighlightedBitmap { get; protected set; }

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
                Bitmap = new Bitmap(Width, Height, Constants.FORMAT);
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

            BitmapData bitmapData = Bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, Bitmap.PixelFormat);
            Marshal.Copy(Pixels, 0, bitmapData.Scan0, Pixels.Length);

            Bitmap.UnlockBits(bitmapData);
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
                HighlightedBitmap = new Bitmap(Width, Height, Constants.FORMAT);
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
                    HighlightedPixels[colorPixelIndex + 0] = Color.Gold.B; // B
                    HighlightedPixels[colorPixelIndex + 1] = Color.Gold.G; // G
                    HighlightedPixels[colorPixelIndex + 2] = Color.Gold.R; // R
                }
                else
                {
                    // Color the rest of the image in grayscale.
                    HighlightedPixels[colorPixelIndex + 0] = intensity; // B
                    HighlightedPixels[colorPixelIndex + 1] = intensity; // G
                    HighlightedPixels[colorPixelIndex + 2] = intensity; // R
                }
            }

            BitmapData bitmapData = HighlightedBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, HighlightedBitmap.PixelFormat);
            Marshal.Copy(HighlightedPixels, 0, bitmapData.Scan0, HighlightedPixels.Length);

            HighlightedBitmap.UnlockBits(bitmapData);
        }

        #endregion
    }
}
