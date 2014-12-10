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
    /// Creates the bitmap representation of a Kinect color frame.
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
        /// <param name="format">The specified color format.</param>
        public void Update(ColorFrame frame, ColorImageFormat format)
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
                frame.CopyConvertedFrameDataToArray(Pixels, format);
            }

            Stream.Seek(0, SeekOrigin.Begin);
            Stream.Write(Pixels, 0, Pixels.Length);

            Bitmap.Invalidate();
        }

        /// <summary>
        /// Updates the bitmap with new frame data.
        /// </summary>
        /// <param name="frame">The specified Kinect color frame.</param>
        public void Update(ColorFrame frame)
        {
            Update(frame, ColorImageFormat.Bgra);
        }

        #endregion
    }
}
