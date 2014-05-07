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
    public class DepthRecorder : IRecorder<DepthFrame>
    {
        DepthBitmapGenerator _bitmapGenerator = new DepthBitmapGenerator();

        public DepthRecorder()
        {
        }

        public DepthRecorder(StorageFile file)
        {
            File = file;
        }

        public override void Update(DepthFrame frame)
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
