using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    public class WaveRightSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Hand right of elbow
                if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ElbowRight].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Undetermined;
            }

            // Hand dropped - no gesture fails
            return GesturePartResult.Failed;
        }
    }

    public class WaveRightSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // Hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // Hand left of elbow
                if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ElbowRight].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }

                // Hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Undetermined;
            }

            // Hand dropped - no gesture fails
            return GesturePartResult.Failed;
        }
    }
}
