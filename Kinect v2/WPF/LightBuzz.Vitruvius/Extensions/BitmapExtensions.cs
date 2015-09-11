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
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for manipulating color frames.
    /// </summary>
    public static class BitmapExtensions
    {
        #region Members

        /// <summary>
        /// The color bitmap creator.
        /// </summary>
        static ColorBitmapGenerator _colorBitmapGenerator = new ColorBitmapGenerator();

        /// <summary>
        /// The depth bitmap creator.
        /// </summary>
        static DepthBitmapGenerator _depthBitmapGenerator = new DepthBitmapGenerator();

        /// <summary>
        /// The color bitmap creator.
        /// </summary>
        static InfraredBitmapGenerator _infraredBitmapGenerator = new InfraredBitmapGenerator();

        /// <summary>
        /// The background removal generator.
        /// </summary>
        static GreenScreenBitmapGenerator _greenScreenBitmapGenerator = new GreenScreenBitmapGenerator();

        /// <summary>
        /// The bitmap capture utility.
        /// </summary>
        static FrameCapture _capture = new FrameCapture();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts the specified color frame to a bitmap image.
        /// </summary>
        /// <param name="frame">The specified <see cref="ColorFrame"/>.</param>
        /// <returns>The bitmap representation of the current frame.</returns>
        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            _colorBitmapGenerator.Update(frame);

            return _colorBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap image.
        /// </summary>
        /// <param name="frame">The specified <see cref="DepthFrame"/>.</param>
        /// <returns>The bitmap representation of the current frame.</returns>
        public static WriteableBitmap ToBitmap(this DepthFrame frame)
        {
            _depthBitmapGenerator.Update(frame);

            return _depthBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Converts the specified depth and infrared frames to a bitmap image with the players highlighted.
        /// </summary>
        /// <param name="depthFrame">The specified <see cref="DepthFrame"/>.</param>
        /// <param name="bodyIndexFrame">The specified <see cref="BodyIndexFrame"/>.</param>
        /// <returns>The bitmap representation of the current frame.</returns>
        public static WriteableBitmap ToBitmap(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

            return _depthBitmapGenerator.HighlightedBitmap;
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap image.
        /// </summary>
        /// <param name="frame">The specified <see cref="InfraredFrame"/>.</param>
        /// <returns>The bitmap representation of the current frame.</returns>
        public static WriteableBitmap ToBitmap(this InfraredFrame frame)
        {
            _infraredBitmapGenerator.Update(frame);

            return _infraredBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Removes the background of the specified frames and generates a new bitmap (green-screen effect).
        /// </summary>
        /// <param name="colorFrame">The specified <see cref="ColorFrame"/>.</param>
        /// <param name="depthFrame">The specified <see cref="DepthFrame"/>.</param>
        /// <param name="bodyIndexFrame">The specified <see cref="BodyIndexFrame"/>.</param>
        /// <returns>The bitmap representation of the generated frame.</returns>
        public static WriteableBitmap GreenScreen(this ColorFrame colorFrame, DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            _greenScreenBitmapGenerator.Update(colorFrame, depthFrame, bodyIndexFrame);

            return _greenScreenBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Removes the background of the specified frames and generates a new bitmap (green-screen effect).
        /// </summary>
        /// <param name="depthFrame">The specified <see cref="DepthFrame"/>.</param>
        /// <param name="colorFrame">The specified <see cref="ColorFrame"/>.</param>
        /// <param name="bodyIndexFrame">The specified <see cref="BodyIndexFrame"/>.</param>
        /// <returns>The bitmap representation of the generated frame.</returns>
        public static WriteableBitmap GreenScreen(this DepthFrame depthFrame, ColorFrame colorFrame, BodyIndexFrame bodyIndexFrame)
        {
            _greenScreenBitmapGenerator.Update(colorFrame, depthFrame, bodyIndexFrame);

            return _greenScreenBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Removes the background of the specified frames and generates a new bitmap (green-screen effect).
        /// </summary>
        /// <param name="bodyIndexFrame">The specified <see cref="BodyIndexFrame"/>.</param>
        /// <param name="colorFrame">The specified <see cref="ColorFrame"/>.</param>
        /// <param name="depthFrame">The specified <see cref="DepthFrame"/>.</param>
        /// <returns>The bitmap representation of the generated frame.</returns>
        public static WriteableBitmap GreenScreen(this BodyIndexFrame bodyIndexFrame, ColorFrame colorFrame, DepthFrame depthFrame)
        {
            _greenScreenBitmapGenerator.Update(colorFrame, depthFrame, bodyIndexFrame);

            return _greenScreenBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static bool Save(this WriteableBitmap source, string path)
        {
            return _capture.Save(source, path);
        }

        /// <summary>
        /// Converts the specified color frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The source color frame.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static bool Save(this ColorFrame frame, string path)
        {
            var bitmap = frame.ToBitmap();

            return _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The source depth frame.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static bool Save(this DepthFrame frame, string path)
        {
            var bitmap = frame.ToBitmap();

            return _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="frame">The source infrared frame.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static bool Save(this InfraredFrame frame, string path)
        {
            var bitmap = frame.ToBitmap();

            return _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified depth and body index frames to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="depthFrame">The source depth frame.</param>
        /// <param name="bodyIndexFrame">The source body index frame.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the image was successfully saved. False otherwise.</returns>
        public static bool Save(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame, string path)
        {
            var bitmap = depthFrame.ToBitmap(bodyIndexFrame);

            return _capture.Save(bitmap, path);
        }

        #endregion
    }
}
