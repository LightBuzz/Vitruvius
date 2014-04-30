using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightBuzz.Vitruvius.WPF
{
    public static class BodyExtensions
    {
        static BodyVisualizationTool _bodyVisualizationTool = new BodyVisualizationTool();

        public static ImageSource ToBitmap(this Body body)
        {
            _bodyVisualizationTool.BeginDrawing();
            _bodyVisualizationTool.DrawBody(body);

            return _bodyVisualizationTool.Render();
        }

        public static ImageSource ToBitmap(this Body body, VisualizationMode mode)
        {
            _bodyVisualizationTool.Mode = mode;

            return ToBitmap(body);
        }
    }
}
