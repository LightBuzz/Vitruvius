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

        public ColorStreamRecorder()
        {
        }

        public ColorStreamRecorder(StorageFile file)
        {
            File = file;
        }

        public override void Update(ColorFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new ColorBitmapGenerator();

                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
            }

            BitmapGenerator.Update(frame);

            Frames.Add(BitmapGenerator.Pixels);
        }
    }
}
