using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Samples
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

        private async void Face_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://vitruviuskinect.com/download", UriKind.Absolute));
        }

        private void JointSelection_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(JointSelectionPage));
        }

        private void Features_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FeaturesPage));
        }

        private async void Avateering_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://vitruviuskinect.com/download", UriKind.Absolute));
        }

        private async void Purchase_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://vitruviuskinect.com/download", UriKind.Absolute));
        }
    }
}
