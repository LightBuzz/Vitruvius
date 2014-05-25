using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightBuzz.Vitruvius.Controls
{
    /// <summary>
    /// Interaction logic for KinectCursor.xaml
    /// </summary>
    public partial class KinectCursor : UserControl
    {
        #region Constructor

        public KinectCursor()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// The text color.
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(KinectCursor), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region Public methods

        public void Update(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        public void Update(ColorImagePoint point, double ratioX = 1.0, double ratioY = 1.0)
        {
            Update(point.X * ratioX, point.Y * ratioY);
        }

        public void Update(DepthImagePoint point, double ratioX = 1.0, double ratioY = 1.0)
        {
            Update(point.X * ratioX, point.Y * ratioY);
        }

        public void Flip(Joint activeHand)
        {
            double scaleX = activeHand.JointType == JointType.HandRight ? 1.0 : -1.0;

            ScaleTransform transform = root.RenderTransform as ScaleTransform;

            if (transform.ScaleX != scaleX)
            {
                transform.ScaleX = scaleX;
            }
        }

        #endregion
    }
}
