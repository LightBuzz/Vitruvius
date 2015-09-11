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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common utilities for manipulating UI elements.
    /// </summary>
    public static class UIExtensions
    {
        #region Constants

        /// <summary>
        /// The default zero point.
        /// </summary>
        static readonly Point ZERO = new Point(0, 0);

        #endregion

        #region Public methods

        /// <summary>
        /// Finds the absolute position of the specified framework element.
        /// </summary>
        /// <param name="element">The framework element.</param>
        /// <returns>A rectangle relative to the screen dimensions.</returns>
        public static Rect Position(this FrameworkElement element)
        {
            try
            {
                Point absolute = element.TransformToVisual(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)).Transform(ZERO);

                return new Rect
                {
                    X = absolute.X,
                    Y = absolute.Y,
                    Width = element.ActualWidth,
                    Height = element.ActualHeight
                };
            }
            catch
            {
                return Rect.Empty;
            }
        }

        /// <summary>
        /// Decides whether the specified framework elements intersect within a canvas.
        /// </summary>
        /// <param name="first">The first framework element.</param>
        /// <param name="second">The second framework element.</param>
        /// <returns>True if the elements intersect. False otherwise.</returns>
        public static bool IntersectsWith(this FrameworkElement first, FrameworkElement second)
        {
            Rect p1 = first.Position();
            Rect p2 = second.Position();

            return (p1.Y + p1.Height < p2.Y) || (p1.Y > p2.Y + p2.Height) || (p1.X + p1.Width < p2.X) || (p1.X > p2.X + p2.Width);
        }

        /// <summary>
        /// Displays the specified framework element, if hidden.
        /// </summary>
        /// <param name="element">The element to show.</param>
        public static void Show(this FrameworkElement element)
        {
            if (element.Visibility != Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hides the specified element, if visible.
        /// </summary>
        /// <param name="element">The element to hide.</param>
        public static void Hide(this FrameworkElement element)
        {
            if (element.Visibility != Visibility.Collapsed)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }

    /// <summary>
    /// Converts boolean values to visibility. True represents visible and false represents hidden elements.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Public methods

        /// <summary>
        /// Converts the specified boolean value to its corresponding visibility enumeration.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter of the conversion.</param>
        /// <param name="culture">The language.</param>
        /// <returns>Visibility.Visible if true. Visibility.Collapsed if false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts the specified visibility enumeration to its corresponding boolean value.
        /// </summary>
        /// <param name="value">The visibility value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter of the conversion.</param>
        /// <param name="culture">The language.</param>
        /// <returns>True for visible. False otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? true : false;
        }

        #endregion
    }
}
