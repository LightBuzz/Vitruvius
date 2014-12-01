using WindowsPreview.Kinect;
using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents a Kinect <see cref="Gesture"/>.
    /// </summary>
    class Gesture
    {
        #region Constants

        /// <summary>
        /// The window size.
        /// </summary>
        readonly int WINDOW_SIZE = 50;

        /// <summary>
        /// The maximum number of frames allowed for a paused gesture.
        /// </summary>
        readonly int MAX_PAUSE_COUNT = 10;

        #endregion

        #region Members

        /// <summary>
        /// The current gesture segment we are matching against.
        /// </summary>
        int _currentSegment = 0;

        /// <summary>
        /// The number of frames to pause for when a pause is initiated.
        /// </summary>
        int _pausedFrameCount = 10;

        /// <summary>
        /// The current frame.
        /// </summary>
        int _frameCount = 0;

        /// <summary>
        /// Are we paused?
        /// </summary>
        bool _isPaused = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Gesture"/>.
        /// </summary>
        public Gesture()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Gesture"/> with the specified type and segments.
        /// </summary>
        /// <param name="type">The type of gesture.</param>
        /// <param name="segments">The segments of the gesture.</param>
        public Gesture(GestureType type, IGestureSegment[] segments)
        {
            GestureType = type;
            Segments = segments;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognised.
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Properties

        /// <summary>
        /// The type of the current gesture.
        /// </summary>
        public GestureType GestureType { get; set; }

        /// <summary>
        /// The segments which form the current gesture.
        /// </summary>
        public IGestureSegment[] Segments { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body data.</param>
        public void Update(Body body)
        {
            if (_isPaused)
            {
                if (_frameCount == _pausedFrameCount)
                {
                    _isPaused = false;
                }

                _frameCount++;
            }

            GesturePartResult result = Segments[_currentSegment].Update(body);

            if (result == GesturePartResult.Succeeded)
            {
                if (_currentSegment + 1 < Segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = MAX_PAUSE_COUNT;
                    _isPaused = true;
                }
                else
                {
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new GestureEventArgs(GestureType, body.TrackingId));
                        Reset();
                    }
                }
            }
            else if (result == GesturePartResult.Failed || _frameCount == WINDOW_SIZE)
            {
                Reset();
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = MAX_PAUSE_COUNT / 2;
                _isPaused = true;
            }
        }

        /// <summary>
        /// Resets the current gesture.
        /// </summary>
        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
            _pausedFrameCount = MAX_PAUSE_COUNT / 2;
            _isPaused = true;
        }

        #endregion
    }
}
