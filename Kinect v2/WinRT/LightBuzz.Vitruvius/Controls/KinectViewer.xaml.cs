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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WindowsPreview.Kinect;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LightBuzz.Vitruvius.Controls
{
    public sealed partial class KinectViewer : UserControl
    {
        #region Constants

        /// <summary>
        /// The default drawing color.
        /// </summary>
        static readonly Brush DEFAULT_BRUSH = new SolidColorBrush(Colors.LightCyan);

        /// <summary>
        /// The default circle radius.
        /// </summary>
        static readonly double DEFAULT_RADIUS = 15;

        /// <summary>
        /// The default line thickness.
        /// </summary>
        static readonly double DEFAULT_THICKNESS = 8;

        #endregion

        #region Members

        List<BodyVisual> _bodyVisuals = new List<BodyVisual>();

        #endregion

        #region Constructor

        public KinectViewer()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency properties

        public CoordinateMapper CoordinateMapper
        {
            get { return (CoordinateMapper)GetValue(CoordinateMapperProperty); }
            set { SetValue(CoordinateMapperProperty, value); }
        }
        public static readonly DependencyProperty CoordinateMapperProperty =
            DependencyProperty.Register("CoordinateMapper", typeof(CoordinateMapper), typeof(KinectViewer), new PropertyMetadata(KinectSensor.GetDefault().CoordinateMapper));

        public VisualizationMode VisualizationMode
        {
            get { return (VisualizationMode)GetValue(VisualizationModeTypeProperty); }
            set { SetValue(VisualizationModeTypeProperty, value); }
        }
        public static readonly DependencyProperty VisualizationModeTypeProperty =
            DependencyProperty.Register("VisualizationMode", typeof(VisualizationMode), typeof(KinectViewer), new PropertyMetadata(VisualizationMode.Color));

        #endregion

        #region Public methods

        public void Clear()
        {
            canvas.Children.Clear();

            _bodyVisuals.Clear();
        }

        public Point GetPoint(CameraSpacePoint position)
        {
            Point point;

            switch (VisualizationMode)
            {
                case VisualizationMode.Color:
                    {
                        ColorSpacePoint colorPoint = CoordinateMapper.MapCameraPointToColorSpace(position);
                        point.X = float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
                        point.Y = float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;
                    }
                    break;
                case VisualizationMode.Depth:
                case VisualizationMode.Infrared:
                    {
                        DepthSpacePoint depthPoint = CoordinateMapper.MapCameraPointToDepthSpace(position);
                        point.X = float.IsInfinity(depthPoint.X) ? 0.0 : depthPoint.X;
                        point.Y = float.IsInfinity(depthPoint.Y) ? 0.0 : depthPoint.Y;
                    }
                    break;
                default:
                    break;
            }

            return point;
        }

        public void DrawBody(Body body, double jointRadius, Brush jointBrush, double boneThickness, Brush boneBrush)
        {
            if (body == null || !body.IsTracked) return;

            BodyVisual visual = _bodyVisuals.Where(b => b.TrackingId == body.TrackingId).FirstOrDefault();

            if (visual == null)
            {
                visual = BodyVisual.Create(body.TrackingId, body.Joints.Keys, jointRadius, jointBrush, boneThickness, boneBrush);

                foreach (var ellipse in visual.Joints.Values)
                {
                    canvas.Children.Add(ellipse);
                }

                foreach (var line in visual.Bones.Values)
                {
                    canvas.Children.Add(line);
                }

                _bodyVisuals.Add(visual);
            }

            foreach (var joint in body.Joints)
            {
                Point point = GetPoint(joint.Value.Position);

                visual.UpdateJoint(joint.Key, point);
            }

            foreach (var bone in visual.CONNECTIONS)
            {
                Point first = GetPoint(body.Joints[bone.Item1].Position);
                Point second = GetPoint(body.Joints[bone.Item2].Position);

                visual.UpdateBone(bone, first, second);
            }
        }

        public void DrawBody(Body body)
        {
            DrawBody(body, DEFAULT_RADIUS, DEFAULT_BRUSH, DEFAULT_THICKNESS, DEFAULT_BRUSH);
        }

        public void Update(WriteableBitmap source)
        {
            if (source != null)
            {
                camera.Source = source;
            }
        }

        #endregion
    }

    class BodyVisual
    {
        public readonly List<Tuple<JointType, JointType>> CONNECTIONS = new List<Tuple<JointType, JointType>>
        {
            // Torso
            new Tuple<JointType, JointType>(JointType.Head, JointType.Neck),
            new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder),
            new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid),
            new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase),
            new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight),
            new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft),
            new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight),
            new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft),

            // Right Arm
            new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight),
            new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight),
            new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight),
            new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight),
            new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight),

            // Left Arm
            new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft),
            new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft),
            new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft),
            new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft),
            new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft),

            // Right Leg
            new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight),
            new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight),
            new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight),

            // Left Leg
            new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft),
            new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft),
            new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft)
        };

        public ulong TrackingId { get; set; }

        public Dictionary<JointType, Ellipse> Joints { get; set; }

        public Dictionary<Tuple<JointType, JointType>, Line> Bones { get; set; }

        public BodyVisual()
        {
            Joints = new Dictionary<JointType, Ellipse>();
            Bones = new Dictionary<Tuple<JointType, JointType>, Line>();
        }

        public void Clear()
        {
            Joints.Clear();
            Bones.Clear();
        }

        public void AddJoint(JointType joint, double radius, Brush brush)
        {
            Joints.Add(joint, new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = brush
            });
        }

        public void AddBone(Tuple<JointType, JointType> joints, double thickness, Brush brush)
        {
            Bones.Add(joints, new Line
            {
                StrokeThickness = thickness,
                Stroke = brush
            });
        }

        public void UpdateJoint(JointType joint, Point point)
        {
            Ellipse ellipse = Joints[joint];

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);
        }

        public void UpdateBone(Tuple<JointType, JointType> bone, Point first, Point second)
        {
            Line line = Bones[bone];

            line.X1 = first.X;
            line.Y1 = first.Y;
            line.X2 = second.X;
            line.Y2 = second.Y;
        }

        public static BodyVisual Create(ulong trackingId, IEnumerable<JointType> joints, double jointRadius, Brush jointBrush, double boneThickness, Brush boneBrush)
        {
            BodyVisual bodyVisual = new BodyVisual
            {
                TrackingId = trackingId
            };

            foreach (var joint in joints)
            {
                bodyVisual.AddJoint(joint, jointRadius, jointBrush);
            }

            foreach (var bone in bodyVisual.CONNECTIONS)
            {
                bodyVisual.AddBone(bone, boneThickness, boneBrush);
            }

            return bodyVisual;
        }
    }
}
