using VitruviusTest.Common;
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
using LightBuzz.Vitruvius;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace VitruviusTest
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AnglePage : Page
    {
        NavigationHelper _navigationHelper;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        UsersController _userReporter;

        JointType _start = JointType.ShoulderRight;
        JointType _center = JointType.ElbowRight;
        JointType _end = JointType.WristRight;

        public NavigationHelper NavigationHelper { get { return _navigationHelper; } }

        public AnglePage()
        {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _userReporter = new UsersController();
                _userReporter.BodyEntered += UserReporter_BodyEntered;
                _userReporter.BodyLeft += UserReporter_BodyLeft;
                _userReporter.Start();
            }
        }

        private void PageRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_userReporter != null)
            {
                _userReporter.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();

                    _userReporter.Update(bodies);

                    Body body = bodies.Default();

                    if (body != null)
                    {
                        viewer.DrawBody(body);
                        angle.Update(body.Joints[_start], body.Joints[_center], body.Joints[_end], 100);

                        tblAngle.Text = ((int)angle.Angle).ToString();
                    }
                }
            }
        }

        void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
        }

        void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
            viewer.Clear();
            angle.Clear();

            tblAngle.Text = "-";
        }
    }
}
