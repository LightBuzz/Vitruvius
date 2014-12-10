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
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;
using System.Threading.Tasks;
using Windows.Storage;

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
        /// The bitmap capture utility.
        /// </summary>
        static BitmapCapture _capture = new BitmapCapture();

        #endregion

        #region Public methods

        /// <summary>
        /// Converts the specified color frame to a bitmap image.
        /// </summary>
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The bitmap representation of the color frame.</returns>
        public static WriteableBitmap ToBitmap(this ColorFrame frame)
        {
            _colorBitmapGenerator.Update(frame);

            return _colorBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap image.
        /// </summary>
        /// <param name="frame">The specified depth frame.</param>
        /// <returns>The bitmap representation of the specified depth frame.</returns>
        public static WriteableBitmap ToBitmap(this DepthFrame frame)
        {
            _depthBitmapGenerator.Update(frame);

            return _depthBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Converts the specified depth and infrared frames to a bitmap image with the players highlighted.
        /// </summary>
        /// <param name="depthFrame">The specified depth frame.</param>
        /// <param name="bodyIndexFrame">The specified body index frame.</param>
        /// <returns>The corresponding System.Windows.Media.Imaging.BitmapSource representation of the depth frame.</returns>
        public static WriteableBitmap ToBitmap(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame)
        {
            _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

            return _depthBitmapGenerator.HighlightedBitmap;
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap image.
        /// </summary>
        /// <param name="frame">The specified infrared frame.</param>
        /// <returns>The bitmap representation of the specified infrared frame.</returns>
        public static WriteableBitmap ToBitmap(this InfraredFrame frame)
        {
            _infraredBitmapGenerator.Update(frame);

            return _infraredBitmapGenerator.Bitmap;
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this WriteableBitmap source, StorageFile destination)
        {
            return await _capture.Save(source, destination);
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this WriteableBitmap source, StorageFile destination, int width, int height)
        {
            return await _capture.Save(source, destination, width, height);
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="destination">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this WriteableBitmap source, string path)
        {
            return await _capture.Save(source, path);
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="destination">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The desired width of the image file.</param>
        /// <param name="height">The desired height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this WriteableBitmap source, string path, int width, int height)
        {
            return await _capture.Save(source, path, width, height);
        }

        /// <summary>
        /// Converts the specified color frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source color frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this ColorFrame frame, StorageFile destination)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, destination);
        }

        /// <summary>
        /// Converts the specified color frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source color frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this ColorFrame frame, StorageFile destination, int width, int height)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, destination, width, height);
        }

        /// <summary>
        /// Converts the specified color frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source color frame.</param>
        /// <param name="destination">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this ColorFrame frame, string path)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified color frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source color frame.</param>
        /// <param name="destination">The destination path for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this ColorFrame frame, string path, int width, int height)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, path, width, height);
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source depth frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame frame, StorageFile destination)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, destination);
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source depth frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame frame, StorageFile destination, int width, int height)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, destination, width, height);
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source depth frame.</param>
        /// <param name="destination">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame frame, string path)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified depth frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source depth frame.</param>
        /// <param name="destination">The destination path for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame frame, string path, int width, int height)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, path, width, height);
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source infrared frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this InfraredFrame frame, StorageFile destination)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, destination);
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source infrared frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this InfraredFrame frame, StorageFile destination, int width, int height)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, destination, width, height);
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source infrared frame.</param>
        /// <param name="destination">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this InfraredFrame frame, string path)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified infrared frame to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="source">The source infrared frame.</param>
        /// <param name="destination">The destination path for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the frame was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this InfraredFrame frame, string path, int width, int height)
        {
            var bitmap = frame.ToBitmap();

            return await _capture.Save(bitmap, path, width, height);
        }

        /// <summary>
        /// Converts the specified depth and body index frames to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="depthFrame">The source depth frame.</param>
        /// <param name="bodyIndexFrame">The source body index frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the image was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame, StorageFile destination)
        {
            var bitmap = depthFrame.ToBitmap(bodyIndexFrame);

            return await _capture.Save(bitmap, destination);
        }

        /// <summary>
        /// Converts the specified depth and body index frames to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="depthFrame">The source depth frame.</param>
        /// <param name="bodyIndexFrame">The source body index frame.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the image was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame, StorageFile destination, int width, int height)
        {
            var bitmap = depthFrame.ToBitmap(bodyIndexFrame);

            return await _capture.Save(bitmap, destination, width, height);
        }

        /// <summary>
        /// Converts the specified depth and body index frames to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="depthFrame">The source depth frame.</param>
        /// <param name="bodyIndexFrame">The source body index frame.</param>
        /// <param name="destination">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the image was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame, string path)
        {
            var bitmap = depthFrame.ToBitmap(bodyIndexFrame);

            return await _capture.Save(bitmap, path);
        }

        /// <summary>
        /// Converts the specified depth and body index frames to a bitmap and saves it to the specified location.
        /// </summary>
        /// <param name="depthFrame">The source depth frame.</param>
        /// <param name="bodyIndexFrame">The source body index frame.</param>
        /// <param name="destination">The destination path for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the image was successfully saved. False otherwise.</returns>
        public static async Task<bool> Save(this DepthFrame depthFrame, BodyIndexFrame bodyIndexFrame, string path, int width, int height)
        {
            var bitmap = depthFrame.ToBitmap(bodyIndexFrame);

            return await _capture.Save(bitmap, path, width, height);
        }

        #endregion
    }
}
