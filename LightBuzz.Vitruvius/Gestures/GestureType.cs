using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents the predefined gesture types.
    /// </summary>
    public enum GestureType
    {
        /// <summary>
        /// Undefined gesture.
        /// </summary>
        None,

        /// <summary>
        /// All of the predefined gestures.
        /// </summary>
        All,

        /// <summary>
        /// Hands joined in front of chest.
        /// </summary>
        JoinedHands,

        /// <summary>
        /// Waving using the right hand.
        /// </summary>
        WaveRight,

        /// <summary>
        /// Waving using the left hand.
        /// </summary>
        WaveLeft,

        /// <summary>
        /// Hand slightly bent above hip (XBOX-like gesture).
        /// </summary>
        Menu,

        /// <summary>
        /// Hand moved horizontally from right to left.
        /// </summary>
        SwipeLeft,

        /// <summary>
        /// Hand moved horizontally from left to right.
        /// </summary>
        SwipeRight,

        /// <summary>
        /// Hand moved vertically from hip center to head.
        /// </summary>
        SwipeUp,

        /// <summary>
        /// Hand moved vertically from head to hip center.
        /// </summary>
        SwipeDown,

        /// <summary>
        /// Both hands extended closer to the chest.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// Both hands extended farther from the chest.
        /// </summary>
        ZoomOut
    }
}
