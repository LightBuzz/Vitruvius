#define USE_KINECTVIEWER //comment this out if you edit the XAML and use instead of KinectViewer control the Viewbox control (that contains a Grid with an Image and a Canvas)

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
        #region --- Fields ---

        private GestureController _gestureController;

        #endregion

        #region --- Initialization ---

        public MainWindow()
        {
            InitializeComponent();

            #if USE_KINECTVIEWER
            /* optional display flipping (vertical flipping may be useful when using a projector) */
            kinectViewer.FlippedHorizontally = true;
            kinectViewer.FlippedVertically = false;
            #endif
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

        #endregion

        #region --- Properties ---

        public VisualizationMode Mode
        {
            #if USE_KINECTVIEWER
            get { return kinectViewer.FrameType; }
            set { kinectViewer.FrameType = value; }
            #else
            get; set;
            #endif
        }

        #endregion

        #region --- Events ---

        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (Mode != VisualizationMode.Color) return;

            using (var frame = e.OpenColorImageFrame())
                if (frame != null)
                    #if USE_KINECTVIEWER
                    kinectViewer.Update(frame.ToBitmap());
                    #else
                    camera.Source = frame.ToBitmap();
                    #endif
        }

        private void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            if (Mode != VisualizationMode.Depth) return;

            using (var frame = e.OpenDepthImageFrame())
                if (frame != null)
                    #if USE_KINECTVIEWER
                    kinectViewer.Update(frame.ToBitmap());
                    #else
                    camera.Source = frame.ToBitmap();
                    #endif
        }

        private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
                if (frame != null)
                {
                    #if USE_KINECTVIEWER
                    kinectViewer.Clear();
                    #else
                    canvas.ClearSkeletons();
                    #endif

                    tblHeights.Text = string.Empty;

                    var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);

                    foreach (var skeleton in skeletons)
                        if (skeleton != null)
                        {
                            // Update skeleton gestures
                            _gestureController.Update(skeleton);

                            // Draw skeleton
                            #if USE_KINECTVIEWER
                            kinectViewer.DrawBody(skeleton);
                            #else
                            canvas.DrawSkeleton(skeleton);
                            #endif

                            // Display user height
                            tblHeights.Text += string.Format("\nUser {0}: {1}cm", skeleton.TrackingId, skeleton.Height());
                        }
                    }
        }

        private void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            // Display the gesture type
            tblGestures.Text = e.Name;

            // Do something according to the type of the gesture
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
            Mode = VisualizationMode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            Mode = VisualizationMode.Depth;
        }

        #endregion
    }

}
