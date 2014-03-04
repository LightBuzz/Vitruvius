using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// Provides some common fnctionality for drawing Kinect skeleton data on a WPF canvas element.
    /// </summary>
    public static class CanvasExtensions
    {
        #region Constants

        /// <summary>
        /// A custom tag indicating that the UIElement was drawn with Vitruvius.
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
        static readonly double DEFAULT_LINE_THICKNESS = 8;

        #endregion

        #region Methods

        /// <summary>
        /// Draws an ellipse to the specified joint.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the ellipse.</param>
        /// <param name="joint">The joint represented by the ellipse.</param>
        /// <param name="color">The desired color for the ellipse.</param>
        /// <param name="radius">The desired length for the ellipse.</param>
        public static void DrawJoint(this Canvas canvas, Joint joint, Color color, double radius)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Ellipse ellipse = new Ellipse
            {
                Tag = TAG,
                Width = radius,
                Height = radius,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(ellipse, joint.Position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, joint.Position.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        /// <summary>
        /// Draws an ellipse to the specified joint.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the ellipse.</param>
        /// <param name="joint">The joint represented by the ellipse.</param>
        /// <param name="color">The desired color for the ellipse.</param>
        public static void DrawJoint(this Canvas canvas, Joint joint, Color color)
        {
            DrawJoint(canvas, joint, color, DEFAULT_ELLIPSE_RADIUS);
        }

        /// <summary>
        /// Draws an ellipse to the specified joint.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the ellipse.</param>
        /// <param name="joint">The joint represented by the ellipse.</param>
        public static void DrawJoint(this Canvas canvas, Joint joint)
        {
            DrawJoint(canvas, joint, DEFAULT_COLOR, DEFAULT_ELLIPSE_RADIUS);
        }

        /// <summary>
        /// Draws a line connecting the specified joints.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the line.</param>
        /// <param name="first">The first joint (start of the line).</param>
        /// <param name="second">The second joint (end of the line)</param>
        /// <param name="color">The desired color for the line.</param>
        /// <param name="thickness">The desired line thickness.</param>
        public static void DrawLine(this Canvas canvas, Joint first, Joint second, Color color, double thickness)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            first = first.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);
            second = second.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Line line = new Line
            {
                Tag = TAG,
                X1 = first.Position.X,
                Y1 = first.Position.Y,
                X2 = second.Position.X,
                Y2 = second.Position.Y,
                StrokeThickness = thickness,
                Stroke = new SolidColorBrush(color)
            };

            canvas.Children.Add(line);
        }

        /// <summary>
        /// Draws a line connecting the specified joints.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the line.</param>
        /// <param name="first">The first joint (start of the line).</param>
        /// <param name="second">The second joint (end of the line)</param>
        /// <param name="color">The desired color for the line.</param>
        public static void DrawLine(this Canvas canvas, Joint first, Joint second, Color color)
        {
            DrawLine(canvas, first, second, color, DEFAULT_LINE_THICKNESS);
        }

        /// <summary>
        /// Draws a line connecting the specified joints.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the line.</param>
        /// <param name="first">The first joint (start of the line).</param>
        /// <param name="second">The second joint (end of the line)</param>
        public static void DrawLine(this Canvas canvas, Joint first, Joint second)
        {
            DrawLine(canvas, first, second, DEFAULT_COLOR, DEFAULT_LINE_THICKNESS);
        }

        /// <summary>
        /// Clears the canvas element and draws the specified body on it.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the skeleton.</param>
        /// <param name="body">The body to draw.</param>
        /// <param name="color">The desired color for the skeleton.</param>
        public static void DrawBody(this Canvas canvas, Body body, Color color)
        {
            if (body == null) return;
            
            foreach (Joint joint in body.Joints.Values)
            {
                canvas.DrawJoint(joint, color);
            }

            canvas.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck], color);
            canvas.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder], color);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft], color);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight], color);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid], color);
            canvas.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], color);
            canvas.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], color);
            canvas.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], color);
            canvas.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], color);
            canvas.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft], color);
            canvas.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight], color);
            canvas.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft], color);
            canvas.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight], color);
            canvas.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft], color);
            canvas.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight], color);
            canvas.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase], color);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft], color);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight], color);
            canvas.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], color);
            canvas.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], color);
            canvas.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft], color);
            canvas.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight], color);
            canvas.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft], color);
            canvas.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight], color);
        }

        /// <summary>
        /// Clears the canvas element and draws the specified body on it.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the skeleton.</param>
        /// <param name="body">The body to draw.</param>
        public static void DrawBody(this Canvas canvas, Body body)
        {
            DrawBody(canvas, body, DEFAULT_COLOR);
        }

        /// <summary>
        /// Removes all the Kinect-related elements drawn by Vitruvius.
        /// </summary>
        /// <param name="canvas">The Canvas element where the elements are drawn.</param>
        public static void ClearSkeletons(this Canvas canvas)
        {
            List<UIElement> items = new List<UIElement>();

            foreach (UIElement item in canvas.Children)
            {
                if (item is Shape)
                {
                    Shape shape = item as Shape;

                    if (shape.Tag == null || shape.Tag.ToString() != TAG)
                    {
                        items.Add(item);
                    }
                }
            }

            // Clear all items.
            canvas.Children.Clear();
            
            // Add the non-Kinect items.
            foreach (UIElement item in items)
            {
                canvas.Children.Add(item);
            }
        }

        #endregion
    }
}
