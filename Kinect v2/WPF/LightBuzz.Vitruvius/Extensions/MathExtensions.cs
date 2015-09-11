//
// Copyright (c) LightBuzz Software.
// All rights reserved.
//
// http://lightbuzz.com
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
// AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common mathematical extensions for Kinect data.
    /// </summary>
    public static class MathExtensions
    {
        #region Degrees / Radians

        /// <summary>
        /// Converts the specified angle from degrees to radians.
        /// </summary>
        /// <param name="angle">The angle, in degrees.</param>
        /// <returns>The same angle in radians.</returns>
        public static double ToRadians(this double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Converts the specified angle from radians to degrees.
        /// </summary>
        /// <param name="angle">The angle, in radians.</param>
        /// <returns>The same angle in degrees.</returns>
        public static double ToDegrees(this double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        #endregion

        #region Points

        /// <summary>
        /// Converts the specified vector into a 2-D point.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>The corresponding 2-D point.</returns>
        public static Point ToPoint(this Vector3D vector)
        {
            return new Point
            {
                X = vector.X,
                Y = vector.Y
            };
        }

        /// <summary>
        /// Converts the specified CameraSpacePoint into a 2-D point.
        /// </summary>
        /// <param name="position">The CameraSpacePoint to convert.</param>
        /// <param name="visualization">The type of the conversion (color, depth, or infrared).</param>
        /// <param name="coordinateMapper">The CoordinateMapper to make the conversion.</param>
        /// <returns>The corresponding 2-D point.</returns>
        public static Point ToPoint(this CameraSpacePoint position, Visualization visualization, CoordinateMapper coordinateMapper)
        {
            Point point = new Point();

            switch (visualization)
            {
                case Visualization.Color:
                    {
                        ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(position);
                        point.X = float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
                        point.Y = float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;
                    }
                    break;
                case Visualization.Depth:
                case Visualization.Infrared:
                    {
                        DepthSpacePoint depthPoint = coordinateMapper.MapCameraPointToDepthSpace(position);
                        point.X = float.IsInfinity(depthPoint.X) ? 0.0 : depthPoint.X;
                        point.Y = float.IsInfinity(depthPoint.Y) ? 0.0 : depthPoint.Y;
                    }
                    break;
                default:
                    break;
            }

            return point;
        }

        /// <summary>
        /// Converts the specified CameraSpacePoint into a 2-D point.
        /// </summary>
        /// <param name="position">The CameraSpacePoint to convert.</param>
        /// <param name="visualization">The type of the conversion (color, depth, or infrared).</param>
        /// <returns>The corresponding 2-D point.</returns>
        public static Point ToPoint(this CameraSpacePoint position, Visualization visualization)
        {
            return position.ToPoint(visualization, KinectSensor.GetDefault().CoordinateMapper);
        }

        /// <summary>
        /// Converts the specified ColorSpacePoint into a 2-D point.
        /// </summary>
        /// <param name="position">The ColorSpacePoint to convert.</param>
        /// <returns>The corresponding 2-D point.</returns>
        public static Point ToPoint(this ColorSpacePoint position)
        {
            return new Point
            {
                X = position.X,
                Y = position.Y
            };            
        }

        /// <summary>
        /// Converts the specified DepthSpacePoint into a 2-D point.
        /// </summary>
        /// <param name="position">The DepthSpacePoint to convert.</param>
        /// <returns>The corresponding 2-D point.</returns>
        public static Point ToPoint(this DepthSpacePoint position)
        {
            return new Point
            {
                X = position.X,
                Y = position.Y
            };
        }

        #endregion

        #region Vectors

        /// <summary>
        /// Converts the specified CameraSpacePoint into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The CameraSpacePoint to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static Vector3D ToVector3(this CameraSpacePoint point)
        {
            return new Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = point.Z
            };
        }

        /// <summary>
        /// Converts the specified ColorSpacePoint into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The ColorSpacePoint to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static Vector3D ToVector3(this ColorSpacePoint point)
        {
            return new Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }

        /// <summary>
        /// Converts the specified DepthSpacePoint into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The DepthSpacePoint to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static Vector3D ToVector3(this DepthSpacePoint point)
        {
            return new Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }

        /// <summary>
        /// Converts the specified 2-D point into a 3-D vector structure.
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <returns>The corresponding 3-D vector.</returns>
        public static Vector3D ToVector3(this Point point)
        {
            return new Vector3D
            {
                X = point.X,
                Y = point.Y,
                Z = 0.0
            };
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Converts the specified quaternion into the corresponding Euler matrix.
        /// </summary>
        /// <param name="orientation">The quaternion to convert, expressed as a rotation 4-D vector.</param>
        /// <returns>The corresponding Euler matrix, expressed as a 4-D vector.</returns>
        public static Vector4 ToEuler(this Vector4 orientation)
        {
            Vector4 vector = new Vector4();

            vector.X = (float)Math.Atan2
            (
                2 * orientation.Y * orientation.W - 2 * orientation.X * orientation.Z,
                1 - 2 * Math.Pow(orientation.Y, 2) - 2 * Math.Pow(orientation.Z, 2)
            );

            vector.Y = (float)Math.Asin
            (
                2 * orientation.X * orientation.Y + 2 * orientation.Z * orientation.W
            );

            vector.Z = (float)Math.Atan2
            (
                2 * orientation.X * orientation.W - 2 * orientation.Y * orientation.Z,
                1 - 2 * Math.Pow(orientation.X, 2) - 2 * Math.Pow(orientation.Z, 2)
            );

            if (orientation.X * orientation.Y + orientation.Z * orientation.W == 0.5)
            {
                vector.X = (float)(2 * Math.Atan2(orientation.X, orientation.W));
                vector.Z = 0;
            }

            else if (orientation.X * orientation.Y + orientation.Z * orientation.W == -0.5)
            {
                vector.X = (float)(-2 * Math.Atan2(orientation.X, orientation.W));
                vector.Z = 0;
            }

            return vector;
        }

        /// <summary>
        /// Transforms this vector according to the specified quaternion.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <param name="quaternion">The quaternion used to transform this vector.</param>
        public static void Transform(this Vector3D vector, Vector4 quaternion)
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

            vector.X = ((vector.X * ((1.0f - yy) - zz)) + (vector.Y * (xy - wz))) + (vector.Z * (xz + wy));
            vector.Y = ((vector.X * (xy + wz)) + (vector.Y * ((1.0f - xx) - zz))) + (vector.Z * (yz - wx));
            vector.Z = ((vector.X * (xz - wy)) + (vector.Y * (yz + wx))) + (vector.Z * ((1.0f - xx) - yy));
        }

        #endregion

        #region Angles

        /// <summary>
        /// Calculates the angle between the specified points.
        /// </summary>
        /// <param name="center">The center of the angle.</param>
        /// <param name="start">The start of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <returns>The angle, in degrees.</returns>
        public static double Angle(this CameraSpacePoint center, CameraSpacePoint start, CameraSpacePoint end)
        {
            Vector3D first = start.ToVector3() - center.ToVector3();
            Vector3D second = end.ToVector3() - center.ToVector3();

            return Vector3D.AngleBetween(first, second);
        }

        /// <summary>
        /// Calculates the angle between the specified points around the specified axis.
        /// </summary>
        /// <param name="center">The center of the angle.</param>
        /// <param name="start">The start of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <param name="axis">The axis around which the angle is calculated.</param>
        /// <returns>The angle, in degrees.</returns>
        public static double Angle(this CameraSpacePoint center, CameraSpacePoint start, CameraSpacePoint end, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    start.X = 0f;
                    center.X = 0f;
                    end.X = 0f;
                    break;
                case Axis.Y:
                    start.Y = 0f;
                    center.Y = 0f;
                    end.Y = 0f;
                    break;
                case Axis.Z:
                    start.Z = 0f;
                    center.Z = 0f;
                    end.Z = 0f;
                    break;
            }

            Vector3D first = start.ToVector3() - center.ToVector3();
            Vector3D second = end.ToVector3() - center.ToVector3();

            return Vector3D.AngleBetween(first, second);
        }

        /// <summary>
        /// Calculates the angle between the specified points.
        /// </summary>
        /// <param name="center">The center of the angle.</param>
        /// <param name="start">The start of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <returns>The angle, in degrees.</returns>
        public static double Angle(this ColorSpacePoint center, ColorSpacePoint start, ColorSpacePoint end)
        {
            Vector3D first = start.ToVector3() - center.ToVector3();
            Vector3D second = end.ToVector3() - center.ToVector3();

            return Vector3D.AngleBetween(first, second);
        }

        /// <summary>
        /// Calculates the angle between the specified points.
        /// </summary>
        /// <param name="center">The center of the angle.</param>
        /// <param name="start">The start of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <returns>The angle, in degrees.</returns>
        public static double Angle(this DepthSpacePoint center, DepthSpacePoint start, DepthSpacePoint end)
        {
            Vector3D first = start.ToVector3() - center.ToVector3();
            Vector3D second = end.ToVector3() - center.ToVector3();

            return Vector3D.AngleBetween(first, second);
        }

        /// <summary>
        /// Calculates the angle between the specified body joints.
        /// </summary>
        /// <param name="center">The center of the angle.</param>
        /// <param name="start">The start of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <returns>The angle, in degrees.</returns>
        public static double Angle(this Joint center, Joint start, Joint end)
        {
            return Angle(center.Position, start.Position, end.Position);
        }

        /// <summary>
        /// Calculates the angle between the specified body joints aroudn the specified axis.
        /// </summary>
        /// <param name="center">The center of the angle.</param>
        /// <param name="start">The start of the angle.</param>
        /// <param name="end">The end of the angle.</param>
        /// <param name="axis">The axis around which the angle is calculated.</param>
        /// <returns>The angle, in degrees.</returns>
        public static double Angle(this Joint center, Joint start, Joint end, Axis axis)
        {
            return Angle(center.Position, start.Position, end.Position, axis);
        }

        #endregion

        #region Lenghts

        /// <summary>
        /// Calculates the length of the specified 3-D point.
        /// </summary>
        /// <param name="point">The specified 3-D point.</param>
        /// <returns>The corresponding length, in meters.</returns>
        public static double Length(this CameraSpacePoint point)
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
        /// <param name="point1">The first point (start of the segment).</param>
        /// <param name="point2">The second point (end of the segment).</param>
        /// <returns>The length of the segment (in meters).</returns>
        public static double Length(this CameraSpacePoint point1, CameraSpacePoint point2)
        {
            return Math.Sqrt(
                Math.Pow(point1.X - point2.X, 2) +
                Math.Pow(point1.Y - point2.Y, 2) +
                Math.Pow(point1.Z - point2.Z, 2)
            );
        }

        /// <summary>
        /// Returns the length of the segments defined by the specified points.
        /// </summary>
        /// <param name="points">A collection of two or more points.</param>
        /// <returns>The length of all the segments in meters.</returns>
        public static double Length(params CameraSpacePoint[] points)
        {
            double length = 0;

            for (int index = 0; index < points.Length - 1; index++)
            {
                length += Length(points[index], points[index + 1]);
            }

            return length;
        }

        #endregion
    }
}
