using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents a displacement in 3-D space.
    /// </summary>
    public struct Vector3
    {
        #region Members

        /// <summary>
        /// Gets or sets the X component of this vector.
        /// </summary>
        public double X;

        /// <summary>
        /// Gets or sets the Y component of this vector.
        /// </summary>
        public double Y;

        /// <summary>
        /// Gets or sets the Z component of this vector.
        /// </summary>
        public double Z;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the length (or magnitude) of this vector.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        /// <summary>
        /// Gets the square of the length of this vector.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds two vectors and returns the result as a Vector structure.
        /// </summary>
        /// <param name="vector1">The first vector to add.</param>
        /// <param name="vector2">The second vector to add.</param>
        /// <returns>The sum of vector1 and vector2.</returns>
        public static Vector3 Add(Vector3 vector1, Vector3 vector2)
        {
            vector1.X += vector2.X;
            vector1.Y += vector2.Y;
            vector1.Z += vector2.Z;

            return vector1;
        }

        /// <summary>
        /// Subtracts the specified vector from another specified vector.
        /// </summary>
        /// <param name="vector1">The vector from which vector2 is subtracted.</param>
        /// <param name="vector2">The vector to subtract from vector1.</param>
        /// <returns>The difference between vector1 and vector2.</returns>
        public static Vector3 Subtract(Vector3 vector1, Vector3 vector2)
        {
            Vector3 vector;

            vector.X = vector1.X - vector2.X;
            vector.Y = vector1.Y - vector2.Y;
            vector.Z = vector1.Z - vector2.Z;

            return vector;
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector and returns the resulting vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying scalar and vector.</returns>
        public static Vector3 Multiply(double scalar, Vector3 vector)
        {
            vector.X *= scalar;
            vector.Y *= scalar;
            vector.Z *= scalar;

            return vector;
        }

        /// <summary>
        /// Divides the specified vector by the specified scalar and returns the result as a Vector.
        /// </summary>
        /// <param name="vector">The vector structure to divide.</param>
        /// <param name="scalar">The amount by which vector is divided.</param>
        /// <returns>The result of dividing vector by scalar.</returns>
        public static Vector3 Divide(Vector3 vector, double scalar)
        {
            vector.X /= scalar;
            vector.Y /= scalar;
            vector.Z /= scalar;

            return vector;
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="value">The vector to compare with this vector.</param>
        /// <returns>True if value has the same X and Y values as this vector; otherwise, false.</returns>
        public bool Equals(Vector3 value)
        {
            return X == value.X && Y == value.Y && Z == value.Z;
        }

        /// <summary>
        /// Compares the two specified vectors for equality.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if t he X and Y components of vector1 and vector2 are equal; otherwise, false.</returns>
        public static bool Equals(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Equals(vector2);
        }

        public static double DotProduct(Vector3 vector1, Vector3 vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y) + (vector1.Z * vector2.Z);
        }

        /// <summary>
        /// Retrieves the cross product of the specified vectors
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Vector3 CrossProduct(Vector3 first, Vector3 second)
        {
            Vector3 vector;

            vector.X = (first.Y * second.Z) - (first.Z * second.Y);
            vector.Y = (first.Z * second.X) - (first.X * second.Z);
            vector.Z = (first.X * second.Y) - (first.Y * second.X);

            return vector;
        }

        /// <summary>
        /// Retrieves the distance of the specified vectors in 3-D space.
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The distance between vector1 and vector2.</returns>
        public static double Distance(Vector3 vector1, Vector3 vector2)
        {
            return Math.Sqrt(
                (vector1.X - vector2.X) * (vector1.X - vector2.X) +
                (vector1.Y - vector2.Y) * (vector1.Y - vector2.Y) +
                (vector1.Z - vector2.Z) * (vector1.Z - vector2.Z)
            );
        }

        /// <summary>
        /// Retrieves the angle, expressed in degrees, between the two specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The angle, in degrees, between vector1 and vector2.</returns>
        public static double AngleBetween(Vector3 vector1, Vector3 vector2)
        {
            vector1.Normalize();
            vector2.Normalize();

            double cosinus = DotProduct(vector1, vector2);

            double angle = (Math.Acos(cosinus) * (180.0 / Math.PI));

            if (double.IsNaN(angle) || double.IsInfinity(angle))
            {
                return 0.0;
            }

            return angle;
        }

        /// <summary>
        /// Normalizes this vector (a normalized vector maintains its direction but its Length becomes 1).
        /// </summary>
        public void Normalize()
        {
            double length = Length;

            if (length != 0)
            {
                double inv = 1 / length;

                X *= inv;
                Y *= inv;
                Z *= inv;
            }
        }

        /// <summary>
        /// Transforms this vector according to the specified quaternion.
        /// </summary>
        /// <param name="quaternion">The quaternion used to transform this vector.</param>
        public void Transform(Vector4 quaternion)
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

            X = ((X * ((1.0f - yy) - zz)) + (Y * (xy - wz))) + (Z * (xz + wy));
            Y = ((X * (xy + wz)) + (Y * ((1.0f - xx) - zz))) + (Z * (yz - wx));
            Z = ((X * (xz - wy)) + (Y * (yz + wx))) + (Z * ((1.0f - xx) - yy));
        }

        #endregion

        #region Operator overloading

        /// <summary>
        /// Adds two vectors and returns the result as a vector.
        /// </summary>
        /// <param name="vector1">The first vector to add.</param>
        /// <param name="vector2">The second vector to add.</param>
        /// <returns>The sum of vector1 and vector2.</returns>
        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            return Add(vector1, vector2);
        }

        /// <summary>
        /// Subtracts one specified vector from another.
        /// </summary>
        /// <param name="vector1">The vector from which vector2 is subtracted.</param>
        /// <param name="vector2">The vector to subtract from vector1.</param>
        /// <returns>The difference between vector1 and vector2.</returns>
        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            return Subtract(vector1, vector2);
        }

        /// <summary>
        /// Multiplies the specified scalar by the specified vector and returns the resulting vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>The result of multiplying scalar and vector.</returns>
        public static Vector3 operator *(double scalar, Vector3 vector)
        {
            return Multiply(scalar, vector);
        }
        
        /// <summary>
        /// Divides the specified vector by the specified scalar and returns the resulting vector.
        /// </summary>
        /// <param name="vector">The vector to divide.</param>
        /// <param name="scalar">The scalar by which vector will be divided.</param>
        /// <returns>The result of dividing vector by scalar.</returns>
        public static Vector3 operator /(Vector3 vector, double scalar)
        {
            return Divide(vector, scalar);
        }

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the X and Y components of vector1 and vector2 are equal; otherwise, false.</returns>
        public static bool operator ==(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Equals(vector2);
        }

        /// <summary>
        /// Compares two vectors for inequality.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the X and Y components of vector1 and vector2 are different; otherwise, false.</returns>
        public static bool operator !=(Vector3 vector1, Vector3 vector2)
        {
            return !vector1.Equals(vector2);
        }

        #endregion
    }
}
