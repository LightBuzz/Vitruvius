using LightBuzz_Vitruvius_Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Interface for a Kinect stream recorder.
    /// </summary>
    public abstract class IRecorder<T>
    {
        protected VideoGenerator _videoGenerator = null;

        protected IRandomAccessStream _stream = null;

        public StorageFile File { get; set; }

        public async void Start()
        {
            if (File != null)
            {
                _stream = await File.OpenAsync(FileAccessMode.ReadWrite);
            }
        }

        public void Stop()
        {
            if (_videoGenerator != null)
            {
                _videoGenerator.Finalize();
                _videoGenerator.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        public abstract void Update(T frame);
    }
}
