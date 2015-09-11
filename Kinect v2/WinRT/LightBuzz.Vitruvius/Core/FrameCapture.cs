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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common methods for capturing and saving Kinect frames to disk.
    /// </summary>
    public class FrameCapture
    {
        #region Public methods

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The width of the image file.</param>
        /// <param name="height">The height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public async Task<bool> Save(WriteableBitmap source, StorageFile destination, int width, int height)
        {
            if (source == null || destination == null) return false;

            try
            {
                Guid encoderID = GetEncoderId(destination);

                using (IRandomAccessStream stream = await destination.OpenAsync(FileAccessMode.ReadWrite))
                {
                    Stream pixelStream = source.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderID, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)source.PixelWidth, (uint)source.PixelHeight, Constants.DPI, Constants.DPI, pixels);

                    if (source.PixelWidth != width || source.PixelHeight != height)
                    {
                        encoder.BitmapTransform.ScaledWidth = (uint)width;
                        encoder.BitmapTransform.ScaledHeight = (uint)height;
                    }

                    await encoder.FlushAsync();
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <param name="width">The desired width of the image file.</param>
        /// <param name="height">The desired height of the image file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public async Task<bool> Save(WriteableBitmap source, string path, int width, int height)
        {
            if (source == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                StorageFile destination = await StorageFile.GetFileFromPathAsync(path);

                return await Save(source, destination, width, height);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="destination">The destination storage file for the image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public async Task<bool> Save(WriteableBitmap source, StorageFile destination)
        {
            return await Save(source, destination, source.PixelWidth, source.PixelHeight);
        }

        /// <summary>
        /// Saves the specified bitmap to the specified location.
        /// </summary>
        /// <param name="source">The source bitmap image.</param>
        /// <param name="path">The destination path for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public async Task<bool> Save(WriteableBitmap source, string path)
        {
            return await Save(source, path, source.PixelWidth, source.PixelHeight);
        }

        /// <summary>
        /// Saves the specified JSON content to the specified location.
        /// </summary>
        /// <param name="source">The JSON-encoded string data.</param>
        /// <param name="destination">The destination path for the JSON file.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public async Task<bool> Save(string source, StorageFile destination)
        {
            if (source == null || destination == null) return false;

            try
            {
                await FileIO.WriteTextAsync(destination, source);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Returns the appropriate bitmap encoder ID, based on the specified file.
        /// </summary>
        /// <param name="file">The storage file for the new image. JPEG, PNG, GIF, BMP and TIFF formats are supported.</param>
        /// <returns>The GUID of the appropriate encoder (defaults to JPEG).</returns>
        private Guid GetEncoderId(StorageFile file)
        {
            Guid encoderID;

            switch (Path.GetExtension(file.FileType))
            {
                case ".jpg":
                case ".jpeg":
                case ".JPG":
                case ".JPEG":
                    encoderID = BitmapEncoder.JpegEncoderId;
                    break;
                case ".png":
                case ".PNG":
                    encoderID = BitmapEncoder.PngEncoderId;
                    break;
                case ".bmp":
                case ".BMP":
                    encoderID = BitmapEncoder.BmpEncoderId;
                    break;
                case ".tiff":
                case ".TIFF":
                    encoderID = BitmapEncoder.TiffEncoderId;
                    break;
                case ".gif":
                case ".GIF":
                    encoderID = BitmapEncoder.GifEncoderId;
                    break;
                default:
                    encoderID = BitmapEncoder.JpegEncoderId;
                    break;
            }

            return encoderID;
        }

        #endregion
    }
}
