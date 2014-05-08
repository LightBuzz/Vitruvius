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

        public override async void Update(DepthFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new DepthBitmapGenerator();

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
