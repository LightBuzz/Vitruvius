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
        public GestureType GestureType { get; private set; }

        /// <summary>
        /// Gets the skeleton tracking ID for the gesture.
        /// </summary>
        public ulong TrackingID { get; private set; }

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
        public GestureEventArgs(GestureType type, ulong trackingID)
        {
            GestureType = type;
            TrackingID = trackingID;
        }

        #endregion
    }
}
