using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LightBuzz.Vitruvius
{
    public static class ControlExtensions
    {
        static readonly Point ZERO = new Point(0, 0);

        public static Rect Position(this Control element)
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

        public static bool IntersectsWith(this Control first, Control second)
        {
            Rect p1 = first.Position();
            Rect p2 = second.Position();

            return (p1.Y + p1.Height < p2.Y) || (p1.Y > p2.Y + p2.Height) || (p1.X + p1.Width < p2.X) || (p1.X > p2.X + p2.Width);
        }

        public static void Show(this UIElement control)
        {
            if (control.Visibility != Visibility.Visible)
            {
                control.Visibility = Visibility.Visible;
            }
        }

        public static void Hide(this UIElement control)
        {
            if (control.Visibility != Visibility.Collapsed)
            {
                control.Visibility = Visibility.Collapsed;
            }
        }
    }
}
