using WindowsPreview.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius
{
    public static class JointExtensions
    {
        #region Methods

        /// <summary>
        /// Returns the length of the segment defined by the specified joints.
        /// </summary>
        /// <param name="p1">The first joint (start of the segment).</param>
        /// <param name="p2">The second joint (end of the segment).</param>
        /// <returns>The length of the segment in meters.</returns>
        public static double Distance(Joint p1, Joint p2)
        {
            return CameraExtensions.Distance(p1.Position, p2.Position);
        }

        /// <summary>
        /// Returns the length of the segments defined by the specified joints.
        /// </summary>
        /// <param name="joints">A collection of two or more joints.</param>
        /// <returns>The length of all the segments in meters.</returns>
        public static double Distance(params Joint[] joints)
        {
            double length = 0;

            for (int index = 0; index < joints.Length - 1; index++)
            {
                length += Distance(joints[index], joints[index + 1]);
            }

            return length;
        }

        /// <summary>
        /// Returns the distance of the specified joints.
        /// </summary>
        /// <param name="p1">The first joint (start of the segment).</param>
        /// <param name="p2">The second joint (end of the segment).</param>
        /// <returns>The length of the segment in meters.</returns>
        public static double DistanceFrom(this Joint p1, Joint p2)
        {
            return Distance(p1, p2);
        }

        /// <summary>
        /// Given a collection of joints, calculates the number of the joints that are tracked accurately.
        /// </summary>
        /// <param name="joints">A collection of joints.</param>
        /// <returns>The number of the accurately tracked joints.</returns>
        public static int NumberOfTrackedJoints(params Joint[] joints)
        {
            int trackedJoints = 0;

            foreach (var joint in joints)
            {
                if (joint.TrackingState == TrackingState.Tracked)
                {
                    trackedJoints++;
                }
            }

            return trackedJoints;
        }

        /// <summary>
        /// Given a collection of joints, calculates the number of the joints that are tracked accurately.
        /// </summary>
        /// <param name="joints">A collection of joints.</param>
        /// <returns>The number of the accurately tracked joints.</returns>
        public static int NumberOfTrackedJoints(IEnumerable<Joint> joints)
        {
            int trackedJoints = 0;

            foreach (var joint in joints)
            {
                if (joint.TrackingState == TrackingState.Tracked)
                {
                    trackedJoints++;
                }
            }

            return trackedJoints;
        }        
        
        /// <summary>
        /// Scales the specified joint according to the specified dimensions.
        /// </summary>
        /// <param name="joint">The joint to scale.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="skeletonMaxX">Maximum X.</param>
        /// <param name="skeletonMaxY">Maximum Y.</param>
        /// <returns>The scaled version of the joint.</returns>
        public static Joint ScaleTo(this Joint joint, double width, double height, float skeletonMaxX, float skeletonMaxY)
        {
            joint.Position = new CameraSpacePoint
            {
                X = Scale(width, skeletonMaxX, joint.Position.X),
                Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z
            };

            return joint;
        }

        /// <summary>
        /// Scales the specified joint according to the specified dimensions.
        /// </summary>
        /// <param name="joint">The joint to scale.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <returns>The scaled version of the joint.</returns>
        public static Joint ScaleTo(this Joint joint, double width, double height)
        {
            return ScaleTo(joint, width, height, 1.0f, 1.0f);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the scaled value of the specified position.
        /// </summary>
        /// <param name="maxPixel">Width or height.</param>
        /// <param name="maxSkeleton">Border (X or Y).</param>
        /// <param name="position">Original position (X or Y).</param>
        /// <returns>The scaled value of the specified position.</returns>
        private static float Scale(double maxPixel, double maxSkeleton, float position)
        {
            float value = (float)((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return (float)maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        #endregion
    }
}
