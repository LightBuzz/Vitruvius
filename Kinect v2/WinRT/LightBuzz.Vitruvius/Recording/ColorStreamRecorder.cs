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
    /// Provides some common functionality for recording the color Kinect stream.
    /// </summary>
    public class ColorStreamRecorder : IStreamRecorder<ColorFrame>
    {
        public ColorBitmapGenerator BitmapGenerator { get; protected set; }

        public override async Task Update(ColorFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new ColorBitmapGenerator();

                Width = frame.FrameDescription.Width;
                Height = frame.FrameDescription.Height;
                Fps = 15;
                Delay = 66;
            }

            BitmapGenerator.Update(frame, ColorImageFormat.Rgba);

            await Update(BitmapGenerator.Pixels);
        }
    }
}
