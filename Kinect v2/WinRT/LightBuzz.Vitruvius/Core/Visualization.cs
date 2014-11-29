using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents the visualization mode.
    /// </summary>
    public enum Visualization
    {
        /// <summary>
        /// Color mode (1920x1080).
        /// </summary>
        Color = 0,

        /// <summary>
        /// Depth mode (512x424).
        /// </summary>
        Depth = 1,

        /// <summary>
        /// Infrared mode (512x424).
        /// </summary>
        Infrared = 3
    }
}
