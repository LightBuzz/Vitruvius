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
    public class ColorStreamRecorder : BaseStreamRecorder<ColorFrame>
    {
        #region Properties

        /// <summary>
        /// The bitmap pixel generator.
        /// </summary>
        public ColorBitmapGenerator BitmapGenerator { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ColorStreamRecorder" />.
        /// </summary>
        public ColorStreamRecorder()
        {
            HD = false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ColorStreamRecorder" />.
        /// </summary>
        /// <param name="hd">Specifies whether the recorder will record in HD.</param>
        public ColorStreamRecorder(bool hd)
        {
            HD = hd;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current frame.
        /// </summary>
        /// <param name="frame">The specified <see cref="ColorFrame"/>.</param>
        public override async Task Update(ColorFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new ColorBitmapGenerator();

                _originalWidth = frame.FrameDescription.Width;
                _originalHeight = frame.FrameDescription.Height;

                Fps = 15;
                Delay = 66;
            }

            BitmapGenerator.Update(frame, ColorImageFormat.Rgba);

            Update(BitmapGenerator.Pixels);
        }

        #endregion
    }
}
