using WindowsPreview.Kinect;
using System.Diagnostics;

namespace LightBuzz.Vitruvius.Gestures
{
    /// <summary>
    /// The first part of the swipe down gesture with the right hand
    /// </summary>
    public class SwipeDownSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {

            // right hand in front of right shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
            {
                // right hand below head height and hand higher than elbow
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
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
    /// The second part of the swipe down gesture for the right hand
    /// </summary>
    public class SwipeDownSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // right hand in front of right shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
            {
                // right hand below right elbow
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.ElbowRight].Position.Y)
                {
                    // right hand right of right shoulder
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.HipRight].Position.X)
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
    /// The third part of the swipe down gesture for the right hand
    /// </summary>
    public class SwipeDownSegment3 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //Right hand in front of right Shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineShoulder].Position.Y)
            {
                // right hand below hip
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.HipRight].Position.Y)
                {
                    // right hand right of right shoulder
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.HipRight].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }

                // Debug.WriteLine("GesturePart 2 - right hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.WriteLine("GesturePart 2 - Right hand in front of right Shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }
}