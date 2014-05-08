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

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Interface for a Kinect stream recorder.
    /// </summary>
    public class IStreamRecorder<T>
    {
        protected VideoGenerator _videoGenerator;

        public int Width { get; set; }

        public int Height { get; set; }

        public int Fps { get; set; }

        public int Delay { get; set; }

        public IRandomAccessStream Stream { get; set; }
        
        public bool IsRecording { get; protected set; }

        public void Start()
        {
            if (Stream == null)
            {
                throw new NullReferenceException("Stream property cannot be null.");
            }

            if (!IsRecording)
            {
                IsRecording = true;
            }
        }

        public void Stop()
        {
            if (IsRecording)
            {
                IsRecording = false;

                _videoGenerator.Finalize();
                _videoGenerator.Dispose();

                if (Stream != null)
                {
                    Stream.Dispose();
                }
            }
        }

        public async Task Update(byte[] source)
        {
            if (_videoGenerator == null)
            {
                if (Width == 0 || Height == 0 || Fps == 0 || Delay == 0)
                {
                    throw new Exception("Width, height, FramesPerSecond and Delay cannot be zero.");
                }

                _videoGenerator = new VideoGenerator((uint)Width, (uint)Height, Stream, (uint)Fps, (uint)Delay);
            }

            await Task.Factory.StartNew(() =>
            {
                if (IsRecording)
                {
                    try
                    {
                        _videoGenerator.AppendNewFrame(source);
                    }
                    catch
                    {
                        // Exception thrown on Stop(). Just ignore it ;-)
                    }
                }
            });
        }

        public virtual void Update(T frame)
        {
        }
    }
}
