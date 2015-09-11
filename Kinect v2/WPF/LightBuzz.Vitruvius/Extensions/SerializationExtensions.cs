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
using System.Collections.Generic;
using System.Text;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality for serializing and deserializing body data to JSON.
    /// </summary>
    public static class SerializationExtensions
    {
        #region Serialization

        /// <summary>
        /// Serializes the current <see cref="BodyFrame"/>.
        /// </summary>
        /// <param name="frame">The frame to serialize.</param>
        /// <returns>A JSON representation of the current frame.</returns>
        public static string Serialize(this BodyFrame frame)
        {
            StringBuilder json = new StringBuilder();

            if (frame != null)
            {
                json.Append("{");
                json.Append("\"Bodies\":");

                json.Append(frame.Bodies().Serialize());
                json.Append("}");
            }

            return json.ToString();
        }

        /// <summary>
        /// Serializes the current collection of <see cref="Body"/>.
        /// </summary>
        /// <param name="bodies">The body collection to serialize.</param>
        /// <returns>A JSON representation of the current body collection.</returns>
        public static string Serialize(this IEnumerable<Body> bodies)
        {
            StringBuilder json = new StringBuilder();

            if (bodies != null)
            {
                json.Append("[");

                foreach (Body body in bodies)
                {
                    json.Append(body.Serialize() + ",");
                }

                json.Append("]");
            }

            return json.ToString();
        }

        /// <summary>
        /// Serializes the current <see cref="Body"/>.
        /// </summary>
        /// <param name="body">The body to serialize.</param>
        /// <returns>A JSON representation of the current body.</returns>
        public static string Serialize(this Body body)
        {
            StringBuilder json = new StringBuilder();

            if (body != null)
            {
                json.Append("{");
                json.Append("\"TrackingId\":\"" + body.TrackingId + "\",");
                json.Append("\"IsTracked\":\"" + body.IsTracked + "\",");
                json.Append("\"LeanTrackingState\":\"" + body.LeanTrackingState + "\",");
                json.Append("\"HandLeftConfidence\":\"" + body.HandLeftConfidence + "\",");
                json.Append("\"HandLeftState\":\"" + body.HandLeftState + "\",");
                json.Append("\"HandRightConfidence\":\"" + body.HandRightConfidence + "\",");
                json.Append("\"HandRightState\":\"" + body.HandRightState + "\",");
                json.Append("\"Joints\":[");

                foreach (Joint joint in body.Joints.Values)
                {
                    json.Append(joint.Serialize() + ",");
                }

                json.Append("]");
                json.Append("}");
            }

            return json.ToString();
        }

        /// <summary>
        /// Serializes the current <see cref="Joint"/>.
        /// </summary>
        /// <param name="joint">The joint to serialize.</param>
        /// <returns>A JSON representation of the current joint.</returns>
        public static string Serialize(this Joint joint)
        {
            StringBuilder json = new StringBuilder();

            json.Append("{");
            json.Append("\"JointType\":\"" + joint.JointType + "\",");
            json.Append("\"TrackingState\":\"" + joint.TrackingState + "\",");
            json.Append("\"Position\":{");
            json.Append("\"X\":\"" + joint.Position.X + "\",");
            json.Append("\"Y\":\"" + joint.Position.Y + "\",");
            json.Append("\"Z\":\"" + joint.Position.Z + "\"");
            json.Append("}");
            json.Append("}");

            return json.ToString();
        }

        #endregion
    }
}
