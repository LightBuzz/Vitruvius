using LightBuzz.Vitruvius;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace VitruviusTest
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BackgroundRemovalPage : Page
    {
        NavigationHelper _navigationHelper;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        BackgroundRemovalTool _backgroundRemoval;

        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }


        public BackgroundRemovalPage()
        {
            InitializeComponent();
            
            _navigationHelper = new NavigationHelper(this);

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body | FrameSourceTypes.BodyIndex);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _backgroundRemoval = new BackgroundRemovalTool(_sensor.CoordinateMapper);
            }
        }

        private void PageRoot_Unloaded(object sender, RoutedEventArgs e)
        {
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
            using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
            // Depth
            using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
            // Body Index
            using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
            {
                if (colorFrame != null && depthFrame != null && bodyIndexFrame != null)
                {
                    image.Source = _backgroundRemoval.GreenScreen(colorFrame, depthFrame, bodyIndexFrame);
                }
            }
        }
    }
}
