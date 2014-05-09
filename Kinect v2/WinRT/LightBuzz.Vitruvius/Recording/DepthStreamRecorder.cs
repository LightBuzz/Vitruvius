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
    public class DepthStreamRecorder : StreamRecorder<DepthFrame>
    {
        #region Properties

        /// <summary>
        /// The bitmap pixel generator.
        /// </summary>
        public DepthBitmapGenerator BitmapGenerator { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DepthStreamRecorder" />.
        /// </summary>
        public DepthStreamRecorder()
        {
            HD = true;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DepthStreamRecorder" />.
        /// </summary>
        /// <param name="hd">Specifies whether the recorder will record in HD.</param>
        public DepthStreamRecorder(bool hd)
        {
            HD = hd;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current frame.
        /// </summary>
        /// <param name="frame">The specified <see cref="DepthFrame"/>.</param>
        public override async void Update(DepthFrame frame)
        {
            if (BitmapGenerator == null)
            {
                BitmapGenerator = new DepthBitmapGenerator();

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
