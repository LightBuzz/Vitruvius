using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        #region Constructor

        /// <summary>
        /// Creates a new instance of KinectAngle.
        /// </summary>
        public KinectAngle()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// The coordinate mapper used to convert between the 3D world and the 2D screen coordinates.
        /// </summary>
        public CoordinateMapper CoordinateMapper
        {
            get { return (CoordinateMapper)GetValue(CoordinateMapperProperty); }
            set { SetValue(CoordinateMapperProperty, value); }
        }
        public static readonly DependencyProperty CoordinateMapperProperty =
            DependencyProperty.Register("CoordinateMapper", typeof(CoordinateMapper), typeof(KinectAngle), new PropertyMetadata(KinectSensor.GetDefault().CoordinateMapper));

        /// <summary>
        /// The visualization mode of the control (Color, Depth, Infrared). Defaults to Color.
        /// </summary>
        public Visualization Visualization
        {
            get { return (Visualization)GetValue(VisualizationProperty); }
            set { SetValue(VisualizationProperty, value); }
        }
        public static readonly DependencyProperty VisualizationProperty =
            DependencyProperty.Register("Visualization", typeof(Visualization), typeof(KinectAngle), new PropertyMetadata(Visualization.Color));

        #endregion

        #region Public methods

        public void Update(Vector3 start, Vector3 middle, Vector3 end, double desiredRadius = 0)
        {
            Vector3 v1 = middle - start;
            Vector3 v2 = middle - end;

            if (desiredRadius == 0)
            {
                desiredRadius = Math.Min(v1.Length, v2.Length);
            }

            v1.Normalize();
            v2.Normalize();

            start = middle - desiredRadius * v1;
            end = middle - desiredRadius * v2;

            line2.Point = start.ToPoint();
            arc.Point = end.ToPoint();
            angleFigure.StartPoint = end.ToPoint();
            line1.Point = middle.ToPoint();

            double angle = Vector3.AngleBetween(v1, v2);

            arc.Size = new Size(desiredRadius, desiredRadius);
        }

        public void Update(CameraSpacePoint start, CameraSpacePoint middle, CameraSpacePoint end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        public void Update(ColorSpacePoint start, ColorSpacePoint middle, ColorSpacePoint end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        public void Update(DepthSpacePoint start, DepthSpacePoint middle, DepthSpacePoint end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        public void Update(Point start, Point middle, Point end, double desiredRadius = 0)
        {
            Update(start.ToVector3(), middle.ToVector3(), end.ToVector3(), desiredRadius);
        }

        public void Update(Joint start, Joint middle, Joint end, double desiredRadius = 0)
        {
            Update(start.Position, middle.Position, end.Position, desiredRadius);
        }

        #endregion
    }
}
