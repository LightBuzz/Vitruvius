using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitruvius_Video;
using Windows.Storage;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for recording the depth Kinect stream.
    /// </summary>
    public class DepthRecorder : IRecorder<DepthFrame>
    {
        VideoGenerator _videoGenerator;

        public StorageFile File { get; set; }

        public void Start()
        {
            ColorRecorder c;
        }

        public void Stop()
        {
        }

        public void Update(DepthFrame frame)
        {
        }
    }
}
