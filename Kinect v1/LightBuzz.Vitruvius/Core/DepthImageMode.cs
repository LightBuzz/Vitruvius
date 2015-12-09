namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents the depth image mode (raw pixels, grayscale, colored).
    /// </summary>
    public enum DepthImageMode
    {
        /// <summary>
        /// The simplest representation of a depth image.
        /// </summary>
        Raw,

        /// <summary>
        /// Depth image representation in a grayscale format.
        /// </summary>
        Dark,

        /// <summary>
        /// Colored depth image representation.
        /// </summary>
        Colors,

        /// <summary>
        /// Player depth frame representation.
        /// </summary>
        Player
    }

}
