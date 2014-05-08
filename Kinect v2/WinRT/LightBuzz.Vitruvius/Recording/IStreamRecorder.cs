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
    public class IStreamRecorder<T>
    {
        DateTime _startTime;
        DateTime _stopTime;

        protected int _width;

        protected int _height;

        protected IRandomAccessStream _stream = null;

        public StorageFile File { get; set; }

        public bool IsRecording { get; protected set; }

        public List<byte[]> Frames { get; set; }

        public async void Start()
        {
            if (!IsRecording)
            {
                if (Frames == null)
                {
                    Frames = new List<byte[]>();
                }

                if (File != null)
                {
                    _stream = await File.OpenAsync(FileAccessMode.ReadWrite);

                    _startTime = DateTime.Now;

                    IsRecording = true;
                }
            }
        }

        public void Stop()
        {
            if (IsRecording)
            {
                IsRecording = false;

                _stopTime = DateTime.Now;

                TimeSpan span = _stopTime - _startTime;
                int seconds = span.Seconds;
                int fps = Frames.Count / seconds;
                int delay = 1000 / fps;

                VideoGenerator videoGenerator = new VideoGenerator((uint)_width, (uint)_height, _stream, (uint)fps, (uint)delay);

                foreach (byte[] frame in Frames)
                {
                    videoGenerator.AppendNewFrame(frame);
                }

                videoGenerator.Finalize();
                videoGenerator.Dispose();

                Frames.Clear();

                if (_stream != null)
                {
                    _stream.Dispose();
                }
            }
        }

        public void Update(byte[] source)
        {
            byte[] destination = new byte[source.Length];
            Array.Copy(source, destination, destination.Length);

            Frames.Add(destination);
        }

        public virtual void Update(T frame)
        {
        }
    }
}
