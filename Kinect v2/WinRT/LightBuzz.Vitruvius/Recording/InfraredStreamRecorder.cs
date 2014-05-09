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

        public override async Task Update(InfraredFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new InfraredBitmapGenerator();

                Width = frame.FrameDescription.Width;
                Height = frame.FrameDescription.Height;
                Fps = 15;
                Delay = 66;
            }

            BitmapGenerator.Update(frame);

            await Update(BitmapGenerator.Pixels);
        }
    }
}
