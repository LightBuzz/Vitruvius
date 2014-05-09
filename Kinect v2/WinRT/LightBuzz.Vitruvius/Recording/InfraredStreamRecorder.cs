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
    public class InfraredStreamRecorder : StreamRecorder<InfraredFrame>
    {
        #region Properties

        /// <summary>
        /// The bitmap pixel generator.
        /// </summary>
        public InfraredBitmapGenerator BitmapGenerator { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="InfraredStreamRecorder" />.
        /// </summary>
        public InfraredStreamRecorder()
        {
            HD = true;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InfraredStreamRecorder" />.
        /// </summary>
        /// <param name="hd">Specifies whether the recorder will record in HD.</param>
        public InfraredStreamRecorder(bool hd)
        {
            HD = hd;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current frame.
        /// </summary>
        /// <param name="frame">The specified <see cref="InfraredFrame"/>.</param>
        public override async void Update(InfraredFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new InfraredBitmapGenerator();

                _originalWidth = frame.FrameDescription.Width;
                _originalHeight = frame.FrameDescription.Height;

                Fps = 30;
                Delay = 66;
            }

            BitmapGenerator.Update(frame);

            Update(BitmapGenerator.Pixels);
        }

        #endregion
    }
}
