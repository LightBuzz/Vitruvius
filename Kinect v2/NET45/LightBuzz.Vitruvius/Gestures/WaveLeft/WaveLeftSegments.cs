using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    public class WaveLeftSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // hand above elbow
            if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
            {
                // hand right of elbow
                if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }

                // hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Undetermined;
            }

            // hand dropped - no gesture fails
            return GesturePartResult.Failed;
        }
    }

    public class WaveLeftSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // hand above elbow
            if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
            {
                // hand right of elbow
                if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }

                // hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Undetermined;
            }

            // hand dropped - no gesture fails
            return GesturePartResult.Failed;
        }
    }

}
