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

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common fnctionality for drawing Kinect skeleton data on a WPF canvas element.
    /// </summary>
    public static class CanvasExtensions
    {
        #region Methods

        /// <summary>
        /// Draws an ellipse to the specified joint.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the ellipse.</param>
        /// <param name="joint">The joint represented by the ellipse.</param>
        /// <param name="color">The desired color for the ellipse.</param>
        public static void DrawPoint(this Canvas canvas, Joint joint, Color color)
        {
            if (joint.TrackingState == JointTrackingState.NotTracked) return;

            joint = joint.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(ellipse, joint.Position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, joint.Position.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
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
            if (first.TrackingState == JointTrackingState.NotTracked || second.TrackingState == JointTrackingState.NotTracked) return;

            first = first.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);
            second = second.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Line line = new Line
            {
                X1 = first.Position.X,
                Y1 = first.Position.Y,
                X2 = second.Position.X,
                Y2 = second.Position.Y,
                StrokeThickness = 8,
                Stroke = new SolidColorBrush(color)
            };

            canvas.Children.Add(line);
        }

        /// <summary>
        /// Clears the canvas element and draws the specified skeleton on it.
        /// </summary>
        /// <param name="canvas">The Canvas element to draw the skeleton.</param>
        /// <param name="skeleton">The skeleton to draw.</param>
        /// <param name="color">The desired color for the skeleton.</param>
        public static void DrawSkeleton(this Canvas canvas, Skeleton skeleton, Color color)
        {
            if (skeleton == null) return;
            
            foreach (Joint joint in skeleton.Joints)
            {
                canvas.DrawPoint(joint, color);
            }

            canvas.DrawLine(skeleton.Joints[JointType.Head], skeleton.Joints[JointType.ShoulderCenter], color);
            canvas.DrawLine(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.Spine], color);
            canvas.DrawLine(skeleton.Joints[JointType.ShoulderLeft], skeleton.Joints[JointType.ElbowLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.ShoulderRight], skeleton.Joints[JointType.ElbowRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.ElbowLeft], skeleton.Joints[JointType.WristLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.WristRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.WristLeft], skeleton.Joints[JointType.HandLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.WristRight], skeleton.Joints[JointType.HandRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.Spine], skeleton.Joints[JointType.HipCenter], color);
            canvas.DrawLine(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.HipLeft], skeleton.Joints[JointType.KneeLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.HipRight], skeleton.Joints[JointType.KneeRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight], color);
            canvas.DrawLine(skeleton.Joints[JointType.AnkleLeft], skeleton.Joints[JointType.FootLeft], color);
            canvas.DrawLine(skeleton.Joints[JointType.AnkleRight], skeleton.Joints[JointType.FootRight], color);
        }

        #endregion
    }
}
