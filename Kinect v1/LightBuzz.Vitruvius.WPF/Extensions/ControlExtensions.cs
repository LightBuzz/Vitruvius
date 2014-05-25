using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LightBuzz.Vitruvius
{
    public static class ControlExtensions
    {
        static readonly Point ZERO = new Point(0, 0);

        public static Rect Position(this Control element)
        {
            try
            {
                return new Rect
                {
                    Location = element.PointToScreen(ZERO),
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
            return first.Position().IntersectsWith(second.Position());
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
