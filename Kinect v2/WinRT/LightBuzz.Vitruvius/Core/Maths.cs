using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some commonly used mathematical utilities.
    /// </summary>
    public static class Maths
    {
        /// <summary>
        /// Calculates the Eucledean norm (or magnitude or length) of the specified 3D vector.
        /// </summary>
        /// <param name="point">The specified 3D vector.</param>
        /// <returns>The length of the 3D vector.</returns>
        public static double Length(CameraSpacePoint point)
        {
            return Math.Sqrt(
                Math.Pow(point.X, 2) +
                Math.Pow(point.Y, 2) +
                Math.Pow(point.Z, 2)
            );
        }

        /// <summary>
        /// Returns the length of the segment defined by the specified points.
        /// </summary>
        /// <param name="first">The first point (start of the segment).</param>
        /// <param name="second">The second point (end of the segment).</param>
        /// <returns>The length of the segment (in meters).</returns>
        public static double Distance(CameraSpacePoint first, CameraSpacePoint second)
        {
            return Math.Sqrt(
                Math.Pow(first.X - second.X, 2) +
                Math.Pow(first.Y - second.Y, 2) +
                Math.Pow(first.Z - second.Z, 2)
            );
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
        /// Adds two 3D vectors.
        /// </summary>
        /// <param name="first">The first 3D vector to add.</param>
        /// <param name="second">The second 3D vector to add.</param>
        /// <returns>The resulting 3D vector.</returns>
        public static CameraSpacePoint Add(CameraSpacePoint first, CameraSpacePoint second)
        {
            first.X += second.X;
            first.Y += second.Y;
            first.Z += second.Z;

            return first;
        }

        /// <summary>
        /// Adds a quaternion to a 3D vector.
        /// </summary>
        /// <param name="point">The specified 3D vector.</param>
        /// <param name="vector">The specified quaternion.</param>
        /// <returns>The resulting 3D vector.</returns>
        public static CameraSpacePoint Add(CameraSpacePoint point, Vector4 vector)
        {
            point.X += vector.X;
            point.Y += vector.Y;
            point.Z += vector.Z;

            return point;
        }

        /// <summary>
        /// Substracts a 3D vector from another one.
        /// </summary>
        /// <param name="first">The first 3D vector.</param>
        /// <param name="second">The second 3D vector.</param>
        /// <returns>The resulting quaternion.</returns>
        public static Vector4 Subtract(CameraSpacePoint first, CameraSpacePoint second)
        {
            Vector4 vector;

            vector.X = first.X - second.X;
            vector.Y = first.Y - second.Y;
            vector.Z = first.Z - second.Z;
            vector.W = 0;

            return vector;
        }

        /// <summary>
        /// Calculates the dot product of the specified quaternions.
        /// </summary>
        /// <param name="first">The first quaternion.</param>
        /// <param name="second">The second quaternion.</param>
        /// <returns>The resulting value.</returns>
        public static float DotProduct(Vector4 first, Vector4 second)
        {
            return (first.X * second.X) + (first.Y * second.Y) + (first.Z * second.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Vector4 CrossProduct(Vector4 first, Vector4 second)
        {
            Vector4 vector;

            vector.X = (first.Y * second.Z) - (first.Z * second.Y);
            vector.Y = (first.Z * second.X) - (first.X * second.Z);
            vector.Z = (first.X * second.Y) - (first.Y * second.X);
            vector.W = 0;

            return vector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector4 Normalize(Vector4 vector)
        {
            float length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);

            if (length != 0)
            {
                float inv = 1 / length;

                vector.X *= inv;
                vector.Y *= inv;
                vector.Z *= inv;
                vector.W = 0;
            }

            return vector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector4 RotationAxis(Vector4 axis, double angle)
        {
            Vector4 normalized = Normalize(axis);

            double half = angle * 0.5f;
            float sinus = (float)Math.Sin(half);
            float cosinus = (float)Math.Cos(half);

            Vector4 vector;
            vector.X = normalized.X * sinus;
            vector.Y = normalized.Y * sinus;
            vector.Z = normalized.Z * sinus;
            vector.W = cosinus;

            return vector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Vector4 Transform(Vector4 vector, Vector4 quaternion)
        {
            float x = quaternion.X + quaternion.X;
            float y = quaternion.Y + quaternion.Y;
            float z = quaternion.Z + quaternion.Z;
            float wx = quaternion.W * x;
            float wy = quaternion.W * y;
            float wz = quaternion.W * z;
            float xx = quaternion.X * x;
            float xy = quaternion.X * y;
            float xz = quaternion.X * z;
            float yy = quaternion.Y * y;
            float yz = quaternion.Y * z;
            float zz = quaternion.Z * z;

            Vector4 result;

            result.X = ((vector.X * ((1.0f - yy) - zz)) + (vector.Y * (xy - wz))) + (vector.Z * (xz + wy));
            result.Y = ((vector.X * (xy + wz)) + (vector.Y * ((1.0f - xx) - zz))) + (vector.Z * (yz - wx));
            result.Z = ((vector.X * (xz - wy)) + (vector.Y * (yz + wx))) + (vector.Z * ((1.0f - xx) - yy));
            result.W = 0;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public static CameraSpacePoint QuaternionToEuler(this Vector4 orientation)
        {
            CameraSpacePoint point;

            point.X = (float)Math.Atan2
            (
                2 * orientation.Y * orientation.W - 2 * orientation.X * orientation.Z,
                1 - 2 * Math.Pow(orientation.Y, 2) - 2 * Math.Pow(orientation.Z, 2)
            );

            point.Y = (float)Math.Asin
            (
                2 * orientation.X * orientation.Y + 2 * orientation.Z * orientation.W
            );

            point.Z = (float)Math.Atan2
            (
                2 * orientation.X * orientation.W - 2 * orientation.Y * orientation.Z,
                1 - 2 * Math.Pow(orientation.X, 2) - 2 * Math.Pow(orientation.Z, 2)
            );

            if (orientation.X * orientation.Y + orientation.Z * orientation.W == 0.5)
            {
                point.X = (float)(2 * Math.Atan2(orientation.X, orientation.W));
                point.Z = 0;
            }

            else if (orientation.X * orientation.Y + orientation.Z * orientation.W == -0.5)
            {
                point.X = (float)(-2 * Math.Atan2(orientation.X, orientation.W));
                point.Z = 0;
            }

            return point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double AngleBetween(CameraSpacePoint center, CameraSpacePoint start, CameraSpacePoint end)
        {
            Vector4 first = Normalize(Subtract(start, center));
            Vector4 second = Normalize(Subtract(end, center));

            float cosinus = DotProduct(first, second);

            double angle = Math.Acos(cosinus) * (180.0 / Math.PI);

            if (double.IsNaN(angle) || double.IsInfinity(angle))
            {
                return 0.0;
            }

            return angle;
        }
    }
}
