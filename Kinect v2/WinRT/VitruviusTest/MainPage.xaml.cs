using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VitruviusTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Camera_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CameraPage));
        }

        private void BackgroundRemoval_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BackgroundRemovalPage));
        }

        private void Angle_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AnglePage));
        }

        private void Gestures_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GesturesPage));
        }
    }
}
