using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz_Vitruvius_Video;
using Windows.Storage;
using Windows.Storage.Streams;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for recording the infrared Kinect stream.
    /// </summary>
    public class InfraredRecorder : IRecorder<InfraredFrame>
    {
        InfraredBitmapGenerator _bitmapGenerator = new InfraredBitmapGenerator();

        public InfraredRecorder()
        {
        }

        public InfraredRecorder(StorageFile file)
        {
            File = file;
        }

        public override void Update(InfraredFrame frame)
        {
            if (_videoGenerator == null)
            {
                _videoGenerator = new VideoGenerator((uint)frame.FrameDescription.Width, (uint)frame.FrameDescription.Height, _stream, 16);
            }

            _bitmapGenerator.Update(frame);

            _videoGenerator.AppendNewFrame(_bitmapGenerator.Pixels);
        }
    }
}
