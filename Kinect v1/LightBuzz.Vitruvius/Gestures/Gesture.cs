using Microsoft.Kinect;
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
        /// The segments which form the current gesture.
        /// </summary>
        IGestureSegment[] _segments;

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
        bool _paused = false;

        /// <summary>
        /// The name of the current gesture.
        /// </summary>
        string _name;

        /// <summary>
        /// The type of the current gesture.
        /// </summary>
        GestureType _type;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognised.
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Gesture"/>.
        /// </summary>
        /// <param name="name">The name of gesture.</param>
        /// <param name="segments">The segments of the gesture.</param>
        public Gesture(string name, IGestureSegment[] segments)
        {
            _name = name;
            _segments = segments;
        }

        public Gesture(GestureType type, IGestureSegment[] segments)
        {
            _type = type;
            _name = type.ToString();
            _segments = segments;

            _name = type.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton data.</param>
        public void Update(Skeleton skeleton)
        {
            if (_paused)
            {
                if (_frameCount == _pausedFrameCount)
                {
                    _paused = false;
                }

                _frameCount++;
            }

            GesturePartResult result = _segments[_currentSegment].Update(skeleton);

            if (result == GesturePartResult.Succeeded)
            {
                if (_currentSegment + 1 < _segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = MAX_PAUSE_COUNT;
                    _paused = true;
                }
                else
                {
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new GestureEventArgs(_name, skeleton.TrackingId));
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
                _paused = true;
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
            _paused = true;
        }

        #endregion
    }
}
