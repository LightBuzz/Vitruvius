using WindowsPreview.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius
{
    public static class CameraExtensions
    {
        #region Methods

        /// <summary>
        /// Returns the length of the segment defined by the specified points.
        /// </summary>
        /// <param name="p1">The first point (start of the segment).</param>
        /// <param name="p2">The second point (end of the segment).</param>
        /// <returns>The length of the segment in meters.</returns>
        public static double Distance(CameraSpacePoint p1, CameraSpacePoint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.X - p2.X, 2) +
                Math.Pow(p1.Y - p2.Y, 2) +
                Math.Pow(p1.Z - p2.Z, 2));
        }

        /// <summary>
        /// Returns the length of the segments defined by the specified points.
        /// </summary>
        /// <param name="joints">A collection of two or more points.</param>
        /// <returns>The length of all the segments in meters.</returns>
        public static double Distance(params CameraSpacePoint[] points)
        {
            double length = 0;

            for (int index = 0; index < points.Length - 1; index++)
            {
                length += Distance(points[index], points[index + 1]);
            }

            return length;
        }

        /// <summary>
        /// Returns the distance of the specified points.
        /// </summary>
        /// <param name="p1">The first point (start of the segment).</param>
        /// <param name="p2">The second point (end of the segment).</param>
        /// <returns>The length of the segment in meters.</returns>
        public static double DistanceFrom(this CameraSpacePoint p1, CameraSpacePoint p2)
        {
            return Distance(p1, p2);
        }

        #endregion
    }
}
