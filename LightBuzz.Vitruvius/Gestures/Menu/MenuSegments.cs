using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    /// <summary>
    /// The menu gesture segment
    /// </summary>
    public class MenuSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Skeleton skeleton)
        {
            // Left and right hands below hip
            if (skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipCenter].Position.Y)
            {
                // left hand 0.3 to left of center hip
                if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.HipCenter].Position.X - 0.3)
                {
                    // left hand 0.2 to left of left elbow
                    if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ElbowLeft].Position.X - 0.2)
                    {
                        return GesturePartResult.Succeeded;
                    }
                }

                return GesturePartResult.Undetermined;
            }

            return GesturePartResult.Failed;
        }
    }
}
