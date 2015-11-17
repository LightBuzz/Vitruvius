using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.WPF;
using Microsoft.Kinect;
using System.Linq;
using System.Windows;

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
            KinectSensor sensor = SensorExtensions.Default();

            if (sensor != null)
            {
                sensor.EnableAllStreams();
                sensor.ColorFrameReady += Sensor_ColorFrameReady;
                sensor.DepthFrameReady += Sensor_DepthFrameReady;
                sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

                _gestureController = new GestureController(GestureType.All);
                _gestureController.GestureRecognized += GestureController_GestureRecognized;

                sensor.Start();
            }
        }

        void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Color)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }
        }

        void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenDepthImageFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }
        }

        void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    canvas.ClearSkeletons();
                    tblHeights.Text = string.Empty;

                    var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);

                    foreach (var skeleton in skeletons)
                    {
                        if (skeleton != null)
                        {
                            // Update skeleton gestures.
                            _gestureController.Update(skeleton);

                            // Draw skeleton.
                            canvas.DrawSkeleton(skeleton);

                            // Display user height.
                            tblHeights.Text += string.Format("\nUser {0}: {1}cm", skeleton.TrackingId, skeleton.Height());
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
    }

    public enum Mode
    {
        Color,
        Depth
    }
}
