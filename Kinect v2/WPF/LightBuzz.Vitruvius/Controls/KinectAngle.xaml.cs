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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LightBuzz.Vitruvius.Controls
{
    /// <summary>
    /// Represents a XAML arc based on a given angle.
    /// </summary>
    public sealed partial class KinectAngle : UserControl
    {
        #region Members

        /// <summary>
        /// The first vector.
        /// </summary>
        Vector3D _vector1;

        /// <summary>
        /// The second vector.
        /// </summary>
        Vector3D _vector2;

        /// <summary>
        /// The angle, expressed in degrees.
        /// </summary>
        double _angle;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of KinectAngle.
        /// </summary>
        public KinectAngle()
        {
            InitializeComponent();

            DataContext = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the angle of the arc, in degrees.
        /// </summary>
        public double Angle
        {
            get
            {
                return _angle;
            }
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// Gets or sets coordinate mapper used to convert between the 3D world and the 2D screen coordinates.
        /// </summary>
        public CoordinateMapper CoordinateMapper
        {
            get { return (CoordinateMapper)GetValue(CoordinateMapperProperty); }
            set { SetValue(CoordinateMapperProperty, value); }
        }
        /// <summary>
        /// The <see cref="Microsoft.Kinect.CoordinateMapper"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CoordinateMapperProperty =
            DependencyProperty.Register("CoordinateMapper", typeof(CoordinateMapper), typeof(KinectAngle), new FrameworkPropertyMetadata(KinectSensor.GetDefault().CoordinateMapper));

        /// <summary>
        /// Gets or sets the brush that specifies how to paint the interior of the shape. 
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        /// <summary>
        /// The <see cref="Fill"/> property.
        /// </summary>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(KinectAngle), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Gets or sets the brush that specifies how to paint the border of the shape. 
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        /// <summary>
        /// The <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(KinectAngle), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Gets or sets the brush that specifies how to paint the border of the shape. 
        /// </summary>
        public Thickness StrokeThickness
        {
            get { return (Thickness)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// The <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(Thickness), typeof(KinectAngle), new FrameworkPropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Gets or sets a value that specifies whether the arc is drawn in the Clockwise or Counterclockwise direction.
        /// </summary>
        public SweepDirection SweepDirection
        {
            get { return (SweepDirection)GetValue(SweepDirectionProperty); }
            set { SetValue(SweepDirectionProperty, value); }
        }
        /// <summary>
        /// The <see cref="SweepDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SweepDirectionProperty =
            DependencyProperty.Register("SweepDirection", typeof(SweepDirection), typeof(KinectAngle), new FrameworkPropertyMetadata(SweepDirection.Clockwise));

        #endregion

        #region Public methods

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed vectors.
        /// </summary>
        /// <param name="start">The vector of the starting point.</param>
        /// <param name="middle">The vector of the middle point.</param>
        /// <param name="end">The vector of the end point.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(Vector3D start, Vector3D middle, Vector3D end, double desiredRadius = 0)
        {
            _vector1 = middle - start;
            _vector2 = middle - end;

            if (desiredRadius == 0)
            {
                desiredRadius = Math.Min(_vector1.Length, _vector2.Length);
            }
            
            _vector1.Normalize();
            _vector2.Normalize();

            _angle = Vector3D.AngleBetween(_vector1, _vector2);

            start = middle - desiredRadius * _vector1;
            end = middle - desiredRadius * _vector2;

            line1.Point = middle.ToPoint();
            line2.Point = start.ToPoint();
            angleFigure.StartPoint = end.ToPoint();

            arc.IsLargeArc = _angle > 180.0;
            arc.Point = end.ToPoint();
            arc.Size = new Size(desiredRadius, desiredRadius);
        }

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed points.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="middle">The middle point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(CameraSpacePoint start, CameraSpacePoint middle, CameraSpacePoint end, double desiredRadius = 0)
        {
            Update(start.ToPoint(Visualization.Color), middle.ToPoint(Visualization.Color), end.ToPoint(Visualization.Color), desiredRadius);
        }

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed points.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="middle">The middle point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(ColorSpacePoint start, ColorSpacePoint middle, ColorSpacePoint end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed points.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="middle">The middle point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(DepthSpacePoint start, DepthSpacePoint middle, DepthSpacePoint end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed points.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="middle">The middle point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(Point start, Point middle, Point end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed joints.
        /// </summary>
        /// <param name="start">The starting joint.</param>
        /// <param name="middle">The middle joint.</param>
        /// <param name="end">The end joint.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(Joint start, Joint middle, Joint end, double desiredRadius = 0)
        {
            Update(start.Position, middle.Position, end.Position, desiredRadius);
        }

        /// <summary>
        /// Clears the current angle.
        /// </summary>
        public void Clear()
        {
            Update(new Vector3D(0, 0, 0), new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
        }

        #endregion
    }
}
