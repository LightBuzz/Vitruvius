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
    /// Provides some common functionality for recording the infrared Kinect stream.
    /// </summary>
    public class InfraredStreamRecorder : IStreamRecorder<InfraredFrame>
    {
        public InfraredBitmapGenerator BitmapGenerator { get; protected set; }

        public InfraredStreamRecorder()
        {
        }

        public InfraredStreamRecorder(StorageFile file)
        {
            File = file;
        }

        public override void Update(InfraredFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new InfraredBitmapGenerator();

                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
            }

            BitmapGenerator.Update(frame);

            Frames.Add(BitmapGenerator.Pixels);
        }
    }
}
