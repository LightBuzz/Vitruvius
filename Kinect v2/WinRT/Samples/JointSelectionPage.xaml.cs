using Samples.Common;
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

namespace Samples
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class JointSelectionPage : Page
    {
        private NavigationHelper _navigationHelper;

        public NavigationHelper NavigationHelper { get { return _navigationHelper; } }

        public JointSelectionPage()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
        }

        private void JointSelector_JointSelected(object sender, JointType e)
        {
            tblJoint.Text = e.ToString();
        }
    }
}
