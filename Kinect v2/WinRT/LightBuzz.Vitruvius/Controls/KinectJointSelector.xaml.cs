//
// Copyright (c) LightBuzz Software.
// All rights reserved.
//
// http://lightbuzz.com
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
// AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

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
