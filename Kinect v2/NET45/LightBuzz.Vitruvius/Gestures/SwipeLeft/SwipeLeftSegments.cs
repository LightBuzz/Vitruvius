using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    /// <summary>
    /// The first part of the swipe left gesture
    /// </summary>
    public class SwipeLeftSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {

            // right hand in front of right shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
            {
                // right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // right hand right of right shoulder
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The second part of the swipe left gesture
    /// </summary>
    public class SwipeLeftSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // right hand in front of right shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
            {
                // right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // right hand left of right shoulder & right of left shoulder
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The third part of the swipe left gesture
    /// </summary>
    public class SwipeLeftSegment3 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //Right hand in front of right Shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
            {
                // //right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // //right hand left of center hip
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }

                    return GesturePartResult.Undetermined;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }
    }
}
