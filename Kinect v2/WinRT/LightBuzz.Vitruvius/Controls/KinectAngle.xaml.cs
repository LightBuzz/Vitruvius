using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsPreview.Kinect;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LightBuzz.Vitruvius.Controls
{
    public sealed partial class KinectAngle : UserControl
    {
        #region Members

        /// <summary>
        /// The first vector.
        /// </summary>
        Vector3 _vector1;

        /// <summary>
        /// The second vector.
        /// </summary>
        Vector3 _vector2;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of KinectAngle.
        /// </summary>
        public KinectAngle()
        {
            InitializeComponent();
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
                return Vector3.AngleBetween(_vector1, _vector2);
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
        public static readonly DependencyProperty CoordinateMapperProperty =
            DependencyProperty.Register("CoordinateMapper", typeof(CoordinateMapper), typeof(KinectAngle), new PropertyMetadata(KinectSensor.GetDefault().CoordinateMapper));

        /// <summary>
        /// Gets or sets visualization mode of the control (Color, Depth, Infrared). Defaults to Color.
        /// </summary>
        public Visualization Visualization
        {
            get { return (Visualization)GetValue(VisualizationProperty); }
            set { SetValue(VisualizationProperty, value); }
        }
        public static readonly DependencyProperty VisualizationProperty =
            DependencyProperty.Register("Visualization", typeof(Visualization), typeof(KinectAngle), new PropertyMetadata(Visualization.Color));

        /// <summary>
        /// Gets or sets the brush that specifies how to paint the interior of the shape. 
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(KinectAngle), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Gets or sets the brush that specifies how to paint the border of the shape. 
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(KinectAngle), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Gets or sets the brush that specifies how to paint the border of the shape. 
        /// </summary>
        public Thickness StrokeThickness
        {
            get { return (Thickness)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(Brush), typeof(KinectAngle), new PropertyMetadata(new Thickness(0)));

        #endregion

        #region Public methods

        /// <summary>
        /// Calculates the angle and updates the arc according to the specifed vectors.
        /// </summary>
        /// <param name="start">The vector of the starting point.</param>
        /// <param name="middle">The vector of the middle point.</param>
        /// <param name="end">The vector of the end point.</param>
        /// <param name="desiredRadius">The desired arc radius.</param>
        public void Update(Vector3 start, Vector3 middle, Vector3 end, double desiredRadius = 0)
        {
            _vector1 = middle - start;
            _vector2 = middle - end;

            if (desiredRadius == 0)
            {
                desiredRadius = Math.Min(_vector1.Length, _vector2.Length);
            }

            _vector1.Normalize();
            _vector2.Normalize();

            start = middle - desiredRadius * _vector1;
            end = middle - desiredRadius * _vector2;

            line2.Point = start.ToPoint();
            arc.Point = end.ToPoint();
            angleFigure.StartPoint = end.ToPoint();
            line1.Point = middle.ToPoint();

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
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
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

        #endregion
    }
}
