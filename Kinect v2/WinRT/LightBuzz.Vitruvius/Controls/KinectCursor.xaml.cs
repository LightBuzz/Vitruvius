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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WindowsPreview.Kinect;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LightBuzz.Vitruvius.Controls
{
    public sealed partial class KinectCursor : UserControl
    {
        #region Constructor

        public KinectCursor()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// Image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(KinectHoverButton), new PropertyMetadata(new BitmapImage(new Uri("Cursor.png", UriKind.Relative))));

        #endregion

        #region Public methods

        public void Update(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        public void Update(ColorSpacePoint point, double ratioX = 1.0, double ratioY = 1.0)
        {
            Update(point.X * ratioX, point.Y * ratioY);
        }

        public void Update(DepthSpacePoint point, double ratioX = 1.0, double ratioY = 1.0)
        {
            Update(point.X * ratioX, point.Y * ratioY);
        }

        public void Flip(Joint activeHand)
        {
            double scaleX = activeHand.JointType == JointType.HandRight ? 1.0 : -1.0;

            ScaleTransform transform = image.RenderTransform as ScaleTransform;

            if (transform.ScaleX != scaleX)
            {
                transform.ScaleX = scaleX;
            }
        }

        #endregion
    }
}
