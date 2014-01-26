using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.WPF;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VitruviusTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mode _mode = Mode.Color;

        GestureController _gestureController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KinectSensor sensor = KinectSensor.Default;

            if (sensor != null)
            {
                sensor.Open();

                MultiSourceFrameReader reader = sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                reader.MultiSourceFrameArrived += reader_MultiSourceFrameArrived;

                _gestureController = new GestureController(GestureType.All);
                _gestureController.GestureRecognized += GestureController_GestureRecognized;
            }
        }

        void reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Color)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // TODO
                }
            }

            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.ClearSkeletons();
                    tblHeights.Text = string.Empty;

                    var bodies = frame.Bodies().Where(body => body.IsTracked);

                    foreach (var body in bodies)
                    {
                        if (body != null)
                        {
                            // Update skeleton gestures.
                            _gestureController.Update(body);

                            // Draw skeleton.
                            canvas.DrawSkeleton(body);

                            // Display user height.
                            tblHeights.Text += string.Format("\nUser {0}: {1}cm", body.TrackingId, body.Height());
                        }
                    }
                }
            }
        }

        void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            // Display the gesture type.
            tblGestures.Text = e.Name;

            // Do something according to the type of the gesture.
            switch (e.Type)
            {
                case GestureType.JoinedHands:
                    break;
                case GestureType.Menu:
                    break;
                case GestureType.SwipeDown:
                    break;
                case GestureType.SwipeLeft:
                    break;
                case GestureType.SwipeRight:
                    break;
                case GestureType.SwipeUp:
                    break;
                case GestureType.WaveLeft:
                    break;
                case GestureType.WaveRight:
                    break;
                case GestureType.ZoomIn:
                    break;
                case GestureType.ZoomOut:
                    break;
                default:
                    break;
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Depth;
        }

        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Infrared;
        }
    }
    
    public enum Mode
    {
        Color,
        Depth,
        Infrared
    }
}
