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
// Based on: http://www.codeproject.com/Articles/17425/A-Vector-Type-for-C by R Potter
//

using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents a displacement in 3-D space.
    /// </summary>
    public struct Vector3
    {
        #region Constants

        /// <summary>
        /// A vector with the minimum double values.
        /// </summary>
        public static readonly Vector3 MinValue = new Vector3(double.MinValue, double.MinValue, double.MinValue);

        /// <summary>
        /// A vector with the maximum double values.
        /// </summary>
        public static readonly Vector3 MaxValue = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);

        /// <summary>
        /// A vector with Epsilon values.
        /// </summary>
        public static readonly Vector3 Epsilon = new Vector3(double.Epsilon, double.Epsilon, double.Epsilon);

        /// <summary>
        /// A vector with zero values.
        /// </summary>
        public static readonly Vector3 Zero = new Vector3(0.0, 0.0, 0.0);

        #endregion

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

        #region Constructors

        /// <summary>
        /// Creates a new instance of Vector3 with the specified initial values.
        /// </summary>
        /// <param name="x">Value of the X coordinate of the new vector.</param>
        /// <param name="y">Value of the Y coordinate of the new vector.</param>
        /// <param name="z">Value of the Z coordinate of the new vector.</param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion Constructors

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
        /// Negates the values of X, Y, and Z on this vector.
        /// </summary>
        public void Negate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
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

        /// <summary>
        /// Calculates the Dot Product of the specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The calculated Dot Product.</returns>
        public static double DotProduct(Vector3 vector1, Vector3 vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y) + (vector1.Z * vector2.Z);
        }

        /// <summary>
        /// Calculates the Cross Product of the specified vectors
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The calculated Cross Product.</returns>
        public static Vector3 CrossProduct(Vector3 vector1, Vector3 vector2)
        {
            Vector3 vector;

            vector.X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
            vector.Y = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
            vector.Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);

            return vector;
        }

        /// <summary>
        /// Calculates the distance of the specified vectors in 3D space.
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
        /// Calculates the distance btween the current and the specified vector in the 3D space.
        /// </summary>
        /// <param name="other">The vector to evaluate.</param>
        /// <returns>The distance between the vectors.</returns>
        public double Distance(Vector3 other)
        {
            return Distance(this, other);
        }

        /// <summary>
        /// Calculates the angle, expressed in degrees, between the two specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector to evaluate.</param>
        /// <param name="vector2">The second vector to evaluate.</param>
        /// <returns>The angle, in degrees, between vector1 and vector2.</returns>
        public static double Angle(Vector3 vector1, Vector3 vector2)
        {
            vector1.Normalize();
            vector2.Normalize();

            double cosinus = DotProduct(vector1, vector2);
            double angle = (Math.Acos(cosinus) * (180.0 / Math.PI));

            if (double.IsNaN(angle) || double.IsInfinity(angle))
            {
                return 0.0;
            }

            Vector3 normal = CrossProduct(vector1, vector2);

            if (normal.Z < 0.0)
            {
                angle = 360.0 - angle;
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
        /// Calculates the interpolated vector of the specified vectors and the specified fraction.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="fraction">The control fraction (a number between 0 and 1).</param>
        /// <returns>The interpolation vector.</returns>
        public static Vector3 Interpolate(Vector3 vector1, Vector3 vector2, double fraction)
        {
            if (fraction > 0 && fraction < 1)
            {
                Vector3 vector;
                vector.X = vector1.X * (1 - fraction) + vector2.X * fraction;
                vector.Y = vector1.Y * (1 - fraction) + vector2.Y * fraction;
                vector.Z = vector1.Z * (1 - fraction) + vector2.Z * fraction;

                return vector;
            }

            return Vector3.Zero;
        }

        /// <summary>
        /// Calculates the interpolated vector of the current vector, the specified vector and the specified fraction.
        /// </summary>
        /// <param name="vector">The specified vector.</param>
        /// <param name="fraction">The control fraction (a number between 0 and 1).</param>
        /// <returns>The interpolation vector.</returns>
        public Vector3 Interpolate(Vector3 vector, double fraction)
        {
            return Interpolate(this, vector, fraction);
        }

        /// <summary>
        /// Compares two vectors and returns the one with the maximum length.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>The vector with the maximum length.</returns>
        public static Vector3 Max(Vector3 vector1, Vector3 vector2)
        {
            if (vector1 >= vector2)
            {
                return vector1;
            }

            return vector2;
        }

        /// <summary>
        /// Compares this vector with the the specified one and returns the one with the maximum length.
        /// </summary>
        /// <param name="value">The vector to compare.</param>
        /// <returns>The vector with the maximum length.</returns>
        public Vector3 Max(Vector3 value)
        {
            return Max(this, value);
        }

        /// <summary>
        /// Compares two vectors and returns the one with the minimum length.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>The vector with the minimum length.</returns>
        public static Vector3 Min(Vector3 vector1, Vector3 vector2)
        {
            if (vector1 <= vector2)
            {
                return vector1;
            }

            return vector2;
        }

        /// <summary>
        /// Compares this vector with the the specified one and returns the one with the minimum length.
        /// </summary>
        /// <param name="value">The vector to compare.</param>
        /// <returns>The vector with the minimum length.</returns>
        public Vector3 Min(Vector3 value)
        {
            return Min(this, value);
        }

        /// <summary>
        /// Rotates the specified vector around the X axis by the given degrees (Euler rotation around X).
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="degree">The number of the degrees to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 Pitch(Vector3 value, double degree)
        {
            Vector3 vector;
            vector.X = value.X;
            vector.Y = (value.Y * Math.Cos(degree)) - (value.Z * Math.Sin(degree));
            vector.Z = (value.Y * Math.Sin(degree)) + (value.Z * Math.Cos(degree));

            return vector;
        }

        /// <summary>
        /// Rotates this vector around the X axis by the given degrees (Euler rotation around X).
        /// </summary>
        /// <param name="degree">The number of the degrees to rotate.</param>
        public void Pitch(double degree)
        {
            this = Pitch(this, degree);
        }

        /// <summary>
        /// Rotates the specified vector around the Y axis by the given degrees (Euler rotation around Y).
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="degree">The number of the degrees to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 Yaw(Vector3 value, double degree)
        {
            Vector3 vector;
            vector.X = (value.Z * Math.Sin(degree)) + (value.X * Math.Cos(degree));
            vector.Y = value.Y;
            vector.Z = (value.Z * Math.Cos(degree)) - (value.X * Math.Sin(degree));

            return vector;
        }

        /// <summary>
        /// Rotates this vector around the Y axis by the given degrees (Euler rotation around Y).
        /// </summary>
        /// <param name="degree">The number of the degrees to rotate.</param>
        public void Yaw(double degree)
        {
            this = Yaw(this, degree);
        }

        /// <summary>
        /// Rotates the specified vector around the Z axis by the given degrees (Euler rotation around Z).
        /// </summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="degree">The number of the degrees to rotate.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 Roll(Vector3 value, double degree)
        {
            Vector3 vector;
            vector.X = (value.X * Math.Cos(degree)) - (value.Y * Math.Sin(degree));
            vector.Y = (value.X * Math.Sin(degree)) + (value.Y * Math.Cos(degree));
            vector.Z = value.Z;

            return vector;
        }

        /// <summary>
        /// Rotates this vector around the Z axis by the given degrees (Euler rotation around Z).
        /// </summary>
        /// <param name="degree">The number of the degrees to rotate.</param>
        public void Roll(double degree)
        {
            this = Roll(this, degree);
        }

        /// <summary>
        /// Compares the length of this vector with the length of the specified object.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns>A positive number if the length of this vector is greater than the other's. A negative number if it's smaller. Zero otherwise.</returns>
        public int CompareTo(object other)
        {
            return this.CompareTo((Vector3)other);
        }

        /// <summary>
        /// Compares the length of this vector with the length of the specified one.
        /// </summary>
        /// <param name="other">The vector to compare.</param>
        /// <returns>A positive number if the length of this vector is greater than the other's. A negative number if it's smaller. Zero otherwise.</returns>
        public int CompareTo(Vector3 other)
        {
            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }

            return 0;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Compares two vectors for equality.
        /// </summary>
        /// <param name="obj">The cast vector to compare with this vector.</param>
        /// <returns>True if value has the same X and Y values as this vector; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                return this.Equals((Vector3)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Creates a string representation of this vector based on the current culture.
        /// </summary>
        /// <returns>A string representation of this vector.</returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", X, Y, Z);
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
        /// Operator -Vector (unary negation).
        /// </summary>
        /// <param name="vector">Vector being negated.</param>
        /// <returns>Negation of the given vector.</returns>
        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3(-vector.X, -vector.Y, -vector.Z);
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

        /// <summary>
        /// Compares the lengths of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is smaller than the length of vector2; otherwise, false.</returns>
        public static bool operator <(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length < vector2.Length;
        }

        /// <summary>
        /// Compares the lengths of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is smaller or equal than the length of vector2; otherwise, false.</returns>
        public static bool operator <=(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length <= vector2.Length;
        }

        /// <summary>
        /// Compares the lengths of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is greater than the length of vector2; otherwise, false</returns>
        public static bool operator >(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length > vector2.Length;
        }

        /// <summary>
        /// Compares the lengths of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>True if the length of vector1 is greater or equal than the length of vector2; otherwise, false</returns>
        public static bool operator >=(Vector3 vector1, Vector3 vector2)
        {
            return vector1.Length >= vector2.Length;
        }

        #endregion
    }
}
