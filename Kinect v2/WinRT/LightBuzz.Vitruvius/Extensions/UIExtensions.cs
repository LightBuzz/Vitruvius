using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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
                Point absolute = element.TransformToVisual(Window.Current.Content).TransformPoint(ZERO);

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
        public static bool IntersectsWith(this FrameworkElement first, Control second)
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
        /// <param name="language">The language.</param>
        /// <returns>Visibility.Visible if true. Visibility.Collapsed if false.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts the specified visibility enumeration to its corresponding boolean value.
        /// </summary>
        /// <param name="value">The visibility value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter of the conversion.</param>
        /// <param name="language">The language.</param>
        /// <returns>True for visible. False otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (Visibility)value == Visibility.Visible ? true : false;
        }

        #endregion
    }
}
