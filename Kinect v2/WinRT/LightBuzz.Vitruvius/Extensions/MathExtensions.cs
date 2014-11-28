using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    public static class MathExtensions
    {
        public static double ToRadians(this double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double ToDegrees(this double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static Point ToPoint(this Vector3 vector)
        {
            Point point;
            point.X = vector.X;
            point.Y = vector.Y;

            return point;
        }

        public static Point ToPoint(this CameraSpacePoint position, Visualization visualization, CoordinateMapper coordinateMapper)
        {
            Point point;

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

        public static Point ToPoint(this CameraSpacePoint position, Visualization visualization)
        {
            return position.ToPoint(visualization, KinectSensor.GetDefault().CoordinateMapper);
        }

        public static Point ToPoint(this ColorSpacePoint position)
        {
            Point point;
            point.X = position.X;
            point.Y = position.Y;

            return point;
        }

        public static Point ToPoint(this DepthSpacePoint position)
        {
            Point point;
            point.X = position.X;
            point.Y = position.Y;

            return point;
        }

        public static Vector3 ToVector3(this CameraSpacePoint position)
        {
            Vector3 vector;
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = position.Z;

            return vector;
        }

        public static Vector3 ToVector3(this ColorSpacePoint position)
        {
            Vector3 vector;
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = 0.0;

            return vector;
        }

        public static Vector3 ToVector3(this DepthSpacePoint position)
        {
            Vector3 vector;
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = 0.0;

            return vector;
        }

        public static Vector3 ToVector3(this Point position)
        {
            Vector3 vector;
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = 0.0;

            return vector;
        }

        public static CameraSpacePoint Add(this CameraSpacePoint position, Vector3 vector)
        {
            position.X += (float)vector.X;
            position.Y += (float)vector.Y;
            position.Z += (float)vector.Z;

            return position;
        }

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

        public static double AngleBetween(this CameraSpacePoint center, CameraSpacePoint start, CameraSpacePoint end)
        {
            Vector3 first = start.Subtract(center);
            Vector3 second = end.Subtract(center);

            return Vector3.AngleBetween(first, second);
        }

        public static double AngleBetween(this Joint center, Joint start, Joint end)
        {
            return AngleBetween(center.Position, start.Position, end.Position);
        }

        public static Vector3 Subtract(this CameraSpacePoint source, CameraSpacePoint position)
        {
            Vector3 vector;
            vector.X = source.X - position.X;
            vector.Y = source.Y - position.Y;
            vector.Z = source.Z - position.Z;

            return vector;
        }

        public static Vector3 Subtract(this Point source, Point position)
        {
            Vector3 vector;

            vector.X = source.X - position.X;
            vector.Y = source.Y - position.Y;
            vector.Z = 0.0;

            return vector;
        }
    }
}
