using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// The gesture event arguments.
    /// </summary>
    public class GestureEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the gesture type.
        /// </summary>
        public GestureType Type { get; private set; }

        /// <summary>
        /// Gets the name of the gesture.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the skeleton tracking ID for the gesture.
        /// </summary>
        public int TrackingID { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        public GestureEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        /// <param name="type">The gesture type.</param>
        /// <param name="trackingID">The tracking ID.</param>
        public GestureEventArgs(GestureType type, int trackingID)
        {
            Type = type;
            TrackingID = trackingID;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        /// <param name="name">The gesture name.</param>
        /// <param name="trackingID">The tracking ID.</param>
        public GestureEventArgs(string name, int trackingID)
        {
            Name = name;
            TrackingID = trackingID;
        }

        #endregion
    }
}
