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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common methods for capturing and saving bitmap images to storage files.
    /// </summary>
    public class BitmapCapture
    {
        #region Public methods

        /// <summary>
        /// Captures the specified image source and saves it to the specified location.
        /// </summary>
        /// <param name="bitmap">The ImageSouce to capture.</param>
        /// <param name="path">The desired file path, including file name and extension, for the new image. Currently, JPEG and PNG formats are supported.</param>
        /// <returns>True if the bitmap file was successfully saved. False otherwise.</returns>
        public bool Save(ImageSource bitmap, string path)
        {
            if (bitmap == null || path == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                BitmapEncoder encoder;

                switch (Path.GetExtension(path))
                {
                    case ".jpg":
                    case ".JPG":
                    case ".jpeg":
                    case ".JPEG":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                    case ".PNG":
                        encoder = new PngBitmapEncoder();
                        break;
                    case ".bmp":
                    case ".BMP":
                        encoder = new BmpBitmapEncoder();
                        break;
                    default:
                        return false;
                }

                encoder.Frames.Add(BitmapFrame.Create(bitmap as BitmapSource));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(stream);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        #endregion
    }
}
