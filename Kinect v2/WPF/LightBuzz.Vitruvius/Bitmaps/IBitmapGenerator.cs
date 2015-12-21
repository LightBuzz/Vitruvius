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

using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius
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
