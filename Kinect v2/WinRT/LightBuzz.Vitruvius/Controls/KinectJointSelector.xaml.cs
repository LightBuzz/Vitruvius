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
using Windows.UI.Xaml.Shapes;
using WindowsPreview.Kinect;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LightBuzz.Vitruvius.Controls
{
    public sealed partial class KinectJointSelector : UserControl
    {
        #region Constructor

        public KinectJointSelector()
        {
            InitializeComponent();

            DataContext = this;
        }

        #endregion

        #region Dependency properties

        public bool IsMale
        {
            get
            {
                return (bool)GetValue(GenderProperty);
            }
            set
            {
                SetValue(GenderProperty, value);
            }
        }
        public static readonly DependencyProperty GenderProperty =
            DependencyProperty.Register("IsMale", typeof(bool), typeof(KinectJointSelector), new PropertyMetadata(true));

        public bool ShowJoints
        {
            get
            {
                return (bool)GetValue(ShowJointsProperty);
            }
            set
            {
                SetValue(ShowJointsProperty, value);
            }
        }
        public static readonly DependencyProperty ShowJointsProperty =
            DependencyProperty.Register("ShowJoints", typeof(bool), typeof(KinectJointSelector), new PropertyMetadata(true));

        #endregion

        #region Events

        public event EventHandler<JointType> JointSelected;

        #endregion

        #region Event handlers

        private void Joint_Tapped(object sender, TappedRoutedEventArgs e)
        {
            foreach (var item in joints.Children)
            {
                Ellipse element = item as Ellipse;
                //element.Fill = DEFAULT_ITEM_BRUSH;
            }

            Ellipse ellipse = sender as Ellipse;
            //ellipse.Fill = SELECTED_ITEM_BRUSH;

            JointType joint = (JointType)int.Parse(ellipse.Tag.ToString());

            if (JointSelected != null)
            {
                JointSelected(this, joint);
            }
        }

        #endregion

        #region Public methods

        public void SelectJoint(JointType joint)
        {
            string tag = ((int)joint).ToString();

            foreach (var item in joints.Children)
            {
                Ellipse element = item as Ellipse;
                //element.Fill = element.Tag.ToString() == tag ? SELECTED_ITEM_BRUSH : DEFAULT_ITEM_BRUSH;
            }
        }

        public void Clear()
        {
            foreach (var item in joints.Children)
            {
                Ellipse element = item as Ellipse;
                //element.Fill = DEFAULT_ITEM_BRUSH;
            }
        }

        #endregion
    }
}
