using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some utilities for displaying a body on screen.
    /// </summary>
    public class BodyVisualizationTool
    {
        #region Constants

        readonly int DEFAULT_COLOR_FRAME_WIDTH = 1920;

        readonly int DEFAULT_COLOR_FRAME_HEIGHT = 1080;

        readonly int DEFAULT_DEPTH_FRAME_WIDTH = 512;

        readonly int DEFAULT_DEPTH_FRAME_HEIGHT = 424;

        readonly double DEFAULT_JOINT_THICKNESS = 8.0;

        readonly double DEFAULT_BONE_THICKNESS = 2.0;

        readonly Brush DEFAULT_TRACKED_JOINT_BRUSH = Brushes.Green;

        readonly Brush DEFAULT_INFERRED_JOINT_BRUSH = Brushes.Yellow;

        readonly Brush DEFAULT_TRACKED_BONE_BRUSH = Brushes.Green;

        readonly Brush DEFAULT_INFERRED_BONE_BRUSH = Brushes.Yellow;

        #endregion

        #region Members

        DrawingContext _drawingContext;

        DrawingVisual _drawingVisual;

        RenderTargetBitmap _bitmap;

        #endregion

        #region Properties

        public CoordinateMapper CoordinateMapper { get; set; }

        public int FrameWidth
        {
            get
            {
                return Mode == VisualizationMode.Color ? DEFAULT_COLOR_FRAME_WIDTH : DEFAULT_DEPTH_FRAME_WIDTH;
            }
        }

        public int FrameHeight
        {
            get
            {
                return Mode == VisualizationMode.Color ? DEFAULT_COLOR_FRAME_HEIGHT : DEFAULT_DEPTH_FRAME_HEIGHT;
            }
        }

        public double JointThickness { get; set; }

        public double BoneThickness { get; set; }

        public VisualizationMode Mode { get; set; }

        public Brush TrackedJointBrush { get; set; }

        public Brush InferredJointBrush { get; set; }

        public Brush TrackedBoneBrush { get; set; }

        public Brush InferredBoneBrush { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of BodyVisualizer.
        /// </summary>
        public BodyVisualizationTool()
        {
            if (CoordinateMapper == null)
            {
                CoordinateMapper = KinectSensor.GetDefault().CoordinateMapper;
            }

            if (JointThickness == 0)
            {
                JointThickness = DEFAULT_JOINT_THICKNESS;
            }

            if (BoneThickness == 0)
            {
                BoneThickness = DEFAULT_BONE_THICKNESS;
            }

            if (TrackedJointBrush == null)
            {
                TrackedJointBrush = DEFAULT_TRACKED_JOINT_BRUSH;
            }

            if (InferredJointBrush == null)
            {
                InferredJointBrush = DEFAULT_INFERRED_JOINT_BRUSH;
            }

            if (TrackedBoneBrush == null)
            {
                TrackedBoneBrush = DEFAULT_TRACKED_BONE_BRUSH;
            }

            if (InferredBoneBrush == null)
            {
                InferredBoneBrush = DEFAULT_INFERRED_BONE_BRUSH;
            }

            _drawingVisual = new DrawingVisual();            
        }

        #endregion

        #region Methods

        public void BeginDrawing()
        {
            _bitmap = new RenderTargetBitmap(FrameWidth, FrameHeight, Constants.DPI, Constants.DPI, PixelFormats.Default);
        }

        public ImageSource Render()
        {
            return _bitmap;
        }

        public void DrawBody(Body body)
        {
            using (_drawingContext = _drawingVisual.RenderOpen())
            {
                IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                Dictionary<JointType, Point> positions = new Dictionary<JointType, Point>();

                foreach (Joint joint in joints.Values)
                {
                    positions[joint.JointType] = ToPoint(joint);
                }

                // Draw joints
                foreach (JointType type in body.Joints.Keys)
                {
                    DrawJoint(joints, positions, type);
                }

                // Draw bones

                // Torso
                DrawBone(joints, positions, JointType.Head, JointType.Neck);
                DrawBone(joints, positions, JointType.Neck, JointType.SpineShoulder);
                DrawBone(joints, positions, JointType.SpineShoulder, JointType.SpineMid);
                DrawBone(joints, positions, JointType.SpineMid, JointType.SpineBase);
                DrawBone(joints, positions, JointType.SpineShoulder, JointType.ShoulderRight);
                DrawBone(joints, positions, JointType.SpineShoulder, JointType.ShoulderLeft);
                DrawBone(joints, positions, JointType.SpineBase, JointType.HipRight);
                DrawBone(joints, positions, JointType.SpineBase, JointType.HipLeft);
                // Right Arm    
                DrawBone(joints, positions, JointType.ShoulderRight, JointType.ElbowRight);
                DrawBone(joints, positions, JointType.ElbowRight, JointType.WristRight);
                DrawBone(joints, positions, JointType.WristRight, JointType.HandRight);
                DrawBone(joints, positions, JointType.HandRight, JointType.HandTipRight);
                DrawBone(joints, positions, JointType.WristRight, JointType.ThumbRight);
                // Left Arm
                DrawBone(joints, positions, JointType.ShoulderLeft, JointType.ElbowLeft);
                DrawBone(joints, positions, JointType.ElbowLeft, JointType.WristLeft);
                DrawBone(joints, positions, JointType.WristLeft, JointType.HandLeft);
                DrawBone(joints, positions, JointType.HandLeft, JointType.HandTipLeft);
                DrawBone(joints, positions, JointType.WristLeft, JointType.ThumbLeft);
                // Right Leg
                DrawBone(joints, positions, JointType.HipRight, JointType.KneeRight);
                DrawBone(joints, positions, JointType.KneeRight, JointType.AnkleRight);
                DrawBone(joints, positions, JointType.AnkleRight, JointType.FootRight);
                // Left Leg
                DrawBone(joints, positions, JointType.HipLeft, JointType.KneeLeft);
                DrawBone(joints, positions, JointType.KneeLeft, JointType.AnkleLeft);
                DrawBone(joints, positions, JointType.AnkleLeft, JointType.FootLeft);
                                
                _bitmap.Render(_drawingVisual);
            }
        }

        public void DrawJoint(Joint joint)
        {
            using (_drawingContext = _drawingVisual.RenderOpen())
            {
                Point position = ToPoint(joint);

                TrackingState state = joint.TrackingState;

                if (state != TrackingState.NotTracked)
                {
                    Brush brush = state == TrackingState.Tracked ? TrackedJointBrush : InferredJointBrush;

                    DrawJoint(position, brush, JointThickness);
                }

                _bitmap.Render(_drawingVisual);
            }
        }

        public void DrawBone(Joint first, Joint second)
        {
            using (_drawingContext = _drawingVisual.RenderOpen())
            {
                if (first.TrackingState != TrackingState.NotTracked && second.TrackingState != TrackingState.NotTracked)
                {
                    Brush brush = first.TrackingState == TrackingState.Inferred || second.TrackingState == TrackingState.Inferred ? InferredBoneBrush : TrackedBoneBrush;

                    DrawBone(ToPoint(first), ToPoint(second), brush, BoneThickness);
                }

                _bitmap.Render(_drawingVisual);
            }
        }

        private void DrawJoint(IReadOnlyDictionary<JointType, Joint> joints, Dictionary<JointType, Point> positions, JointType type)
        {
            TrackingState state = joints[type].TrackingState;

            if (state != TrackingState.NotTracked)
            {
                Brush brush = state == TrackingState.Tracked ? TrackedJointBrush : InferredJointBrush;

                DrawJoint(positions[type], brush, JointThickness);
            }
        }

        private void DrawJoint(Point position, Brush brush, double thickness)
        {
            _drawingContext.DrawEllipse(brush, null, position, thickness, thickness);
        }

        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, Dictionary<JointType, Point> positions, JointType type1, JointType type2)
        {
            Joint first = joints[type1];
            Joint second = joints[type2];

            if (first.TrackingState != TrackingState.NotTracked && second.TrackingState != TrackingState.NotTracked)
            {
                Brush brush = first.TrackingState == TrackingState.Inferred || second.TrackingState == TrackingState.Inferred ? InferredBoneBrush : TrackedBoneBrush;

                DrawBone(positions[type1], positions[type2], brush, BoneThickness);
            }
        }

        private void DrawBone(Point first, Point second, Brush brush, double thickness)
        {
            Pen pen = new Pen(brush, thickness);

            _drawingContext.DrawLine(pen, first, second);
        }

        private Point ToPoint(Joint joint)
        {
            double x;
            double y;

            switch (Mode)
            {
                case VisualizationMode.Color:
                    ColorSpacePoint colorPoint = CoordinateMapper.MapCameraPointToColorSpace(joint.Position);
                    x = colorPoint.X;
                    y = colorPoint.Y;
                    break;
                case VisualizationMode.Depth:
                case VisualizationMode.Infrared:
                    DepthSpacePoint depthPoint = CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);
                    x = depthPoint.X;
                    y = depthPoint.Y;
                    break;
                default:
                    x = joint.Position.X;
                    y = joint.Position.Y;
                    break;
            }

            return new Point(x, y);
        }

        #endregion
    }
}
