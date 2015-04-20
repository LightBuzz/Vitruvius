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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz_Vitruvius_Video;
using Windows.Storage;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for recording the depth Kinect stream.
    /// </summary>
    public class DepthVideoRecorder : VideoRecorder<DepthFrame>
    {
        #region Properties

        /// <summary>
        /// The bitmap pixel generator.
        /// </summary>
        public DepthBitmapGenerator BitmapGenerator { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DepthFrameVideoRecorder" />.
        /// </summary>
        public DepthVideoRecorder()
        {
            HD = true;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DepthFrameVideoRecorder" />.
        /// </summary>
        /// <param name="hd">Specifies whether the recorder will record in HD.</param>
        public DepthVideoRecorder(bool hd)
        {
            HD = hd;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current frame.
        /// </summary>
        /// <param name="frame">The specified <see cref="DepthFrame"/>.</param>
        public override async Task Update(DepthFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new DepthBitmapGenerator();

                _originalWidth = frame.FrameDescription.Width;
                _originalHeight = frame.FrameDescription.Height;

                Fps = 30;
                Delay = 66;
            }

            BitmapGenerator.Update(frame);

            Update(BitmapGenerator.Pixels);
        }

        #endregion
    }
}
