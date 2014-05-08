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
    public class DepthStreamRecorder : IStreamRecorder<DepthFrame>
    {
        public DepthBitmapGenerator BitmapGenerator { get; protected set; }

        public DepthStreamRecorder()
        {
        }

        public DepthStreamRecorder(StorageFile file)
        {
            File = file;
        }

        public override void Update(DepthFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new DepthBitmapGenerator();

                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
            }

            BitmapGenerator.Update(frame);

            Update(BitmapGenerator.Pixels);
        }
    }
}
