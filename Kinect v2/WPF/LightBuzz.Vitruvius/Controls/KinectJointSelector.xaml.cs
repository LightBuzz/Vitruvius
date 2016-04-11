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

using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LightBuzz.Vitruvius.Controls
{
    /// <summary>
    /// Provides a XAML User Interface to select body joints.
    /// </summary>
    public sealed partial class KinectJointSelector : UserControl
    {
        #region --- Initialization ---

        /// <summary>
        /// Creates a new instance of <see cref="KinectJointSelector"/>.
        /// </summary>
        public KinectJointSelector()
        {
            InitializeComponent();

            DataContext = this;
        }

        #endregion

        #region --- Properties ---

        #region JointsVisibility

        /// <summary>
        /// Determines whether the User Interface will display the joints.
        /// </summary>
        public Visibility JointsVisibility
        {
            get
            {
                return (Visibility)GetValue(JointsVisibilityProperty);
            }
            set
            {
                SetValue(JointsVisibilityProperty, value);
            }
        }

        /// <summary>
        /// The <see cref="JointsVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty JointsVisibilityProperty =
            DependencyProperty.Register("JointsVisibility", typeof(Visibility), typeof(KinectJointSelector),
                new PropertyMetadata(Visibility.Visible));

        #endregion

        #region BodyBackground

        /// <summary>
        /// The fill color of the human body shape.
        /// </summary>
        public Brush BodyBackground
        {
            get
            {
                return (Brush)GetValue(BodyBackgroundProperty);
            }
            set
            {
                SetValue(BodyBackgroundProperty, value);
            }
        }

        /// <summary>
        /// The <see cref="BodyBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BodyBackgroundProperty =
            DependencyProperty.Register("BodyBackground", typeof(Brush), typeof(KinectJointSelector),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

        #endregion

        #region --- Methods ---

        /// <summary>
        /// Selects the specified joint.
        /// </summary>
        /// <param name="joint">The joint to select.</param>
        public void SelectJoint(JointType joint)
        {
            string tag = ((int)joint).ToString();

            foreach (var item in joints.Children) //TODO: is this really doing something?
            {
                Ellipse element = item as Ellipse;
            }
        }

        /// <summary>
        /// Clears the selections and resets the User Interface.
        /// </summary>
        public void Clear()
        {
            foreach (var item in joints.Children) //TODO: is this really doing something?
            {
                Ellipse element = item as Ellipse;
            }
        }

        #endregion

        #region --- Events ---

        /// <summary>
        /// Raised when a joint is selected.
        /// </summary>
        public event EventHandler<JointType> JointSelected;

        private void Joint_Tapped(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in joints.Children) //TODO: is this really doing something?
            {
                Ellipse element = item as Ellipse;
            }

            Ellipse ellipse = sender as Ellipse;

            JointType joint = (JointType)int.Parse(ellipse.Tag.ToString());

            if (JointSelected != null)
                JointSelected(this, joint);
        }

        #endregion
    }
}
