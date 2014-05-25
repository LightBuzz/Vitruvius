using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightBuzz.Vitruvius.Controls
{
    /// <summary>
    /// Interaction logic for KinectViewer.xaml
    /// </summary>
    public partial class KinectViewer : UserControl
    {
        #region Constants

        /// <summary>
        /// A custom tag indicating that the UIElement was drawn with Kinetisense.
        /// </summary>
        static readonly string TAG = "LightBuzz.Vitruvius";

        /// <summary>
        /// The default drawing color.
        /// </summary>
        static readonly Color DEFAULT_COLOR = Colors.LightCyan;

        /// <summary>
        /// The default circle radius.
        /// </summary>
        static readonly double DEFAULT_ELLIPSE_RADIUS = 20;

        /// <summary>
        /// The default line thickness.
        /// </summary>
        static readonly double DEFAULT_BONE_THICKNESS = 8;

        #endregion

        #region Members

        int _kinectFrameWidth;
        int _kinectFrameHeight;

        double _ratioX = 1.0;
        double _ratioY = 1.0;

        #endregion

        #region Constructor

        public KinectViewer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Dependency properties

        public CoordinateMapper CoordinateMapper
        {
            get { return (CoordinateMapper)GetValue(CoordinateMapperProperty); }
            set { SetValue(CoordinateMapperProperty, value); }
        }
        public static readonly DependencyProperty CoordinateMapperProperty =
            DependencyProperty.Register("CoordinateMapper", typeof(CoordinateMapper), typeof(KinectViewer), new PropertyMetadata(KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault().CoordinateMapper));

        public VisualizationMode FrameType
        {
            get { return (VisualizationMode)GetValue(KinectFrameTypeProperty); }
            set { SetValue(KinectFrameTypeProperty, value); }
        }
        public static readonly DependencyProperty KinectFrameTypeProperty =
            DependencyProperty.Register("FrameType", typeof(VisualizationMode), typeof(KinectViewer), new PropertyMetadata(VisualizationMode.Color));

        public Brush JointBrush
        {
            get { return (Brush)GetValue(JointBrushProperty); }
            set { SetValue(JointBrushProperty, value); }
        }
        public static readonly DependencyProperty JointBrushProperty =
            DependencyProperty.Register("JointBrush", typeof(Brush), typeof(KinectViewer), new PropertyMetadata(new SolidColorBrush(DEFAULT_COLOR)));

        public Brush BoneBrush
        {
            get { return (Brush)GetValue(BoneBrushProperty); }
            set { SetValue(BoneBrushProperty, value); }
        }
        public static readonly DependencyProperty BoneBrushProperty =
            DependencyProperty.Register("BoneBrush", typeof(Brush), typeof(KinectViewer), new PropertyMetadata(new SolidColorBrush(DEFAULT_COLOR)));

        public double JointRadius
        {
            get { return (double)GetValue(JointRadiusProperty); }
            set { SetValue(JointRadiusProperty, value); }
        }
        public static readonly DependencyProperty JointRadiusProperty =
            DependencyProperty.Register("JointRadius", typeof(double), typeof(KinectViewer), new PropertyMetadata(DEFAULT_ELLIPSE_RADIUS));

        public double BoneThickness
        {
            get { return (double)GetValue(BoneThicknessProperty); }
            set { SetValue(BoneThicknessProperty, value); }
        }
        public static readonly DependencyProperty BoneThicknessProperty =
            DependencyProperty.Register("BoneThickness", typeof(double), typeof(KinectViewer), new PropertyMetadata(DEFAULT_BONE_THICKNESS));

        #endregion

        #region Public methods

        public void Clear()
        {
            canvas.Children.Clear();
        }

        public void DrawJoint(Joint joint, Brush brush, double radius)
        {
            if (joint.TrackingState == JointTrackingState.NotTracked) return;

            Point point = new Point(_ratioX, _ratioY);

            switch (FrameType)
            {
                case VisualizationMode.Color:
                    {
                        ColorImagePoint colorPoint = CoordinateMapper.MapSkeletonPointToColorPoint(joint.Position, ColorImageFormat.RgbResolution640x480Fps30);
                        point.X *= float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
                        point.Y *= float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;
                    }
                    break;
                case VisualizationMode.Depth:
                case VisualizationMode.Infrared:
                    {
                        DepthImagePoint depthPoint = CoordinateMapper.MapSkeletonPointToDepthPoint(joint.Position, DepthImageFormat.Resolution320x240Fps30);
                        point.X *= float.IsInfinity(depthPoint.X) ? 0.0 : depthPoint.X;
                        point.Y *= float.IsInfinity(depthPoint.Y) ? 0.0 : depthPoint.Y;
                    }
                    break;
                default:
                    break;
            }

            Ellipse ellipse = new Ellipse
            {
                Tag = TAG,
                Width = radius,
                Height = radius,
                Fill = brush
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public void DrawJoint(Joint joint)
        {
            DrawJoint(joint, JointBrush, DEFAULT_ELLIPSE_RADIUS);
        }

        public void DrawBone(Joint first, Joint second, Brush brush, double thickness)
        {
            if (first.TrackingState == JointTrackingState.NotTracked || second.TrackingState == JointTrackingState.NotTracked) return;

            Point firstPoint = new Point(_ratioX, _ratioY);
            Point secondPoint = new Point(_ratioX, _ratioY);

            switch (FrameType)
            {
                case VisualizationMode.Color:
                    {
                        ColorImagePoint colorFirstPoint = CoordinateMapper.MapSkeletonPointToColorPoint(first.Position, ColorImageFormat.RgbResolution640x480Fps30);
                        firstPoint.X *= float.IsInfinity(colorFirstPoint.X) ? 0.0 : colorFirstPoint.X;
                        firstPoint.Y *= float.IsInfinity(colorFirstPoint.Y) ? 0.0 : colorFirstPoint.Y;

                        ColorImagePoint colorSecondPoint = CoordinateMapper.MapSkeletonPointToColorPoint(second.Position, ColorImageFormat.RgbResolution640x480Fps30);
                        secondPoint.X *= float.IsInfinity(colorSecondPoint.X) ? 0.0 : colorSecondPoint.X;
                        secondPoint.Y *= float.IsInfinity(colorSecondPoint.Y) ? 0.0 : colorSecondPoint.Y;
                    }
                    break;
                case VisualizationMode.Depth:
                case VisualizationMode.Infrared:
                    {
                        DepthImagePoint depthFirstPoint = CoordinateMapper.MapSkeletonPointToDepthPoint(first.Position, DepthImageFormat.Resolution320x240Fps30);
                        firstPoint.X *= float.IsInfinity(depthFirstPoint.X) ? 0.0 : depthFirstPoint.X;
                        firstPoint.Y *= float.IsInfinity(depthFirstPoint.Y) ? 0.0 : depthFirstPoint.Y;

                        DepthImagePoint depthSecondPoint = CoordinateMapper.MapSkeletonPointToDepthPoint(second.Position, DepthImageFormat.Resolution320x240Fps30);
                        secondPoint.X *= float.IsInfinity(depthSecondPoint.X) ? 0.0 : depthSecondPoint.X;
                        secondPoint.Y *= float.IsInfinity(depthSecondPoint.Y) ? 0.0 : depthSecondPoint.Y;
                    }
                    break;
                default:
                    break;
            }

            Line line = new Line
            {
                Tag = TAG,
                X1 = firstPoint.X,
                Y1 = firstPoint.Y,
                X2 = secondPoint.X,
                Y2 = secondPoint.Y,
                StrokeThickness = thickness,
                Stroke = brush
            };

            canvas.Children.Add(line);
        }

        public void DrawBone(Joint first, Joint second)
        {
            DrawBone(first, second, BoneBrush, DEFAULT_BONE_THICKNESS);
        }

        public void DrawBody(Skeleton body)
        {
            Clear();

            if (body == null || body.TrackingState != SkeletonTrackingState.Tracked) return;

            foreach (Joint joint in body.Joints)
            {
                DrawJoint(joint, JointBrush, JointRadius);
            }

            DrawBone(body.Joints[JointType.Head], body.Joints[JointType.ShoulderCenter], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ShoulderCenter], body.Joints[JointType.ShoulderLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ShoulderCenter], body.Joints[JointType.ShoulderRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ShoulderCenter], body.Joints[JointType.Spine], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.Spine], body.Joints[JointType.HipCenter], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.HipCenter], body.Joints[JointType.HipLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.HipCenter], body.Joints[JointType.HipRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft], BoneBrush, BoneThickness);
            DrawBone(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight], BoneBrush, BoneThickness);
        }

        public void Update(WriteableBitmap source)
        {
            if (source != null)
            {
                camera.Source = source;

                if (_kinectFrameWidth == 0 || _kinectFrameHeight == 0)
                {
                    _kinectFrameWidth = source.PixelWidth;
                    _kinectFrameHeight = source.PixelHeight;
                }

                if (double.IsNaN(canvas.Width) || double.IsNaN(canvas.Height) || canvas.Width == 0.0 || canvas.Height == 0.0 || double.IsInfinity(canvas.Width) || double.IsInfinity(canvas.Height))
                {
                    SetCanvasSize();
                }
            }
        }

        #endregion

        #region Private methods

        private void SetCanvasSize()
        {
            canvas.Width = camera.ActualWidth;
            canvas.Height = camera.ActualHeight;

            _ratioX = canvas.Width / _kinectFrameWidth;
            _ratioY = canvas.Height / _kinectFrameHeight;
        }

        #endregion

        #region Event handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _kinectFrameWidth = 0;
            _kinectFrameHeight = 0;
        }

        #endregion
    }
}
