using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    public class JoinedHandsSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // Right and Left Hand in front of Shoulders
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z && body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                // Hands between shoulder and hip
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y &&
                    body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Hands between shoulders
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderLeft].Position.X &&
                        body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X && body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        // Hands very close
                        if (body.Joints[JointType.HandRight].Position.X - body.Joints[JointType.HandLeft].Position.X < 0)
                        {
                            return GesturePartResult.Succeeded;
                        }

                        return GesturePartResult.Undetermined;
                    }

                    return GesturePartResult.Failed;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }

    }
}
