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

using LightBuzz_Vitruvius_Video;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.Foundation;
using System.Threading;
using Windows.UI;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// A generic Kinect stream recorder.
    /// </summary>
    public class VideoRecorder<T>
    {
        #region Members

        /// <summary>
        /// The low-level, C++ video generator.
        /// </summary>
        VideoGenerator _videoGenerator;

        /// <summary>
        /// A thread-safe frame queue.
        /// </summary>
        ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();

        /// <summary>
        /// The original width of the frame.
        /// </summary>
        protected int _originalWidth;

        /// <summary>
        /// The original height of the frame.
        /// </summary>
        protected int _originalHeight;

        #endregion

        #region Properties

        /// <summary>
        /// The width of the video.
        /// </summary>
        public int Width
        {
            get { return HD ? _originalWidth : _originalWidth / 2; }
            set { _originalWidth = value; }
        }

        /// <summary>
        /// The height of the video.
        /// </summary>
        public int Height
        {
            get { return HD ? _originalHeight : _originalHeight / 2; }
            set { _originalHeight = value; }
        }

        /// <summary>
        /// The frames per second of the video.
        /// </summary>
        public int Fps { get; set; }

        /// <summary>
        /// The delay (in milliseconds) between each video frame.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Determines whether the video is in its original form.
        /// </summary>
        public bool HD { get; set; }

        /// <summary>
        /// The file stream where the video will be saved.
        /// </summary>
        public IRandomAccessStream Stream { get; set; }

        /// <summary>
        /// Returns whether the recorder is currently recording.
        /// </summary>
        public bool IsRecording { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Start a new recording session.
        /// </summary>
        public void Start()
        {
            if (Stream == null)
            {
                throw new NullReferenceException("Stream property should not be null.");
            }

            if (!IsRecording)
            {
                IsRecording = true;
            }

            Task.Run(() =>
            {
                while (IsRecording || _queue.Count > 0)
                {
                    byte[] pixels;

                    if (_queue.TryDequeue(out pixels))
                    {
                        _videoGenerator.AppendNewFrame(pixels);
                    }

                }

                _videoGenerator.Finalize();
                _videoGenerator.Dispose();
                _videoGenerator = null;
            });
        }

        /// <summary>
        /// Stops the current recording session.
        /// </summary>
        public void Stop()
        {
            if (IsRecording)
            {
                IsRecording = false;
            }
        }

        /// <summary>
        /// Updates the video frames with the specified byte array.
        /// </summary>
        /// <param name="pixels">The new frame in binary format.</param>
        public void Update(byte[] pixels)
        {
            if (_videoGenerator == null)
            {
                _videoGenerator = new VideoGenerator((uint)Width, (uint)Height, Stream, (uint)Fps, (uint)Delay);
            }

            if (HD)
            {
                _queue.Enqueue(pixels);
            }
            else
            {
                _queue.Enqueue(Resize(pixels));
            }
        }

        /// <summary>
        /// Updates the video frames with the specified Kinect frame.
        /// </summary>
        /// <param name="frame">A Kinect color, depth or infrared frame.</param>
        public virtual async Task Update(T frame)
        {
        }
        
        /// <summary>
        /// Resizes the specified frame to half its size.
        /// </summary>
        /// <param name="pixels">The specified frame in binary format.</param>
        /// <returns>The new, half-sized, frame.</returns>
        private byte[] Resize(byte[] pixels)
        {
            byte[] tmp = new byte[pixels.Length / 4];

            int counter = 0;

            for (int index = 0; index < pixels.Length; index += 8)
            {
                if ((index / (_originalWidth << 2)) % 2 == 0)
                {
                    int frameIndex = counter << 2;

                    tmp[frameIndex + 0] = pixels[index + 0];
                    tmp[frameIndex + 1] = pixels[index + 1];
                    tmp[frameIndex + 2] = pixels[index + 2];
                    tmp[frameIndex + 3] = pixels[index + 3];

                    counter++;
                }
                else
                {
                    index += _originalWidth << 2 - 8;
                }
            }

            return tmp;
        }

        #endregion
    }
}
