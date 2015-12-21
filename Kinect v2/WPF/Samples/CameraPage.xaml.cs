using LightBuzz.Vitruvius;
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

namespace Samples
{
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;

        bool _displaySkeleton;

        public CameraPage()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _playersController = new PlayersController();
                _playersController.BodyEntered += UserReporter_BodyEntered;
                _playersController.BodyLeft += UserReporter_BodyLeft;
                _playersController.Start();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_playersController != null)
            {
                _playersController.Stop();
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            viewer.Visualization = Visualization.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            viewer.Visualization = Visualization.Depth;
        }

        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            viewer.Visualization = Visualization.Infrared;
        }

        private void Skeleton_Checked(object sender, RoutedEventArgs e)
        {
            _displaySkeleton = true;
        }

        private void Skeleton_Unchecked(object sender, RoutedEventArgs e)
        {
            _displaySkeleton = false;
            viewer.Clear(); //calling this after setting _displaySkeleton=false to clear any previously drawn skeleton
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

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Depth)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Infrared)
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

                    _playersController.Update(bodies);

                    foreach (Body body in bodies)
                    {
                        if (_displaySkeleton)
                        {
                            viewer.DrawBody(body);
                        }
                    }
                }
            }
        }

        void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
            // A new user has entered the scene.
        }

        void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
            // A user has left the scene.
            viewer.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "vitruvius-capture.jpg");

            (viewer.Image as WriteableBitmap).Save(path);
        }
    }
}
