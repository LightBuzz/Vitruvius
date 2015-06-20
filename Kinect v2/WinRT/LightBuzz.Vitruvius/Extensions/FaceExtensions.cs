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

using Microsoft.Kinect.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius.Extensions
{
    /// <summary>
    /// Provides some common functionality for manupulating body data.
    /// </summary>
    public static class FaceExtensions
    {
        #region Members

        static FaceModel _faceModel = new FaceModel();
        static FaceAlignment _faceAlignment = new FaceAlignment();

        #endregion

        #region Public methods

        public static IReadOnlyList<CameraSpacePoint> Face(this HighDefinitionFaceFrame frame)
        {
            frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);

            return _faceModel.CalculateVerticesForAlignment(_faceAlignment);
        }

        public static List<CameraSpacePoint> EyeLeft(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        public static List<CameraSpacePoint> EyeRight(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        public static List<CameraSpacePoint> CheekLeft(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }
        public static List<CameraSpacePoint> CheekRight(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        public static List<CameraSpacePoint> Forehead(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        public static List<CameraSpacePoint> Nose(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        public static List<CameraSpacePoint> Mouth(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        public static List<CameraSpacePoint> Lips(this IReadOnlyList<CameraSpacePoint> vertices)
        {
            return vertices.GetFacePoints(1, 2, 3, 4, 5);
        }

        #endregion

        #region Utilities

        internal static List<CameraSpacePoint> GetFacePoints(this IReadOnlyList<CameraSpacePoint> vertices, params int[] indexes)
        {
            List<CameraSpacePoint> points = new List<CameraSpacePoint>();

            foreach (int index in indexes)
            {
                points.Add(vertices[index]);
            }

            return points;
        }

        #endregion
    }
}
