using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Samples
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Camera_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CameraPage());
        }

        private void BackgroundRemoval_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BackgroundRemovalPage());
        }

        private void Angle_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AnglePage());
        }

        private void Gestures_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GesturesPage());
        }

        private void Face_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vitruviuskinect.com/download");
        }

        private void JointSelection_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new JointSelectionPage());
        }

        private void Features_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeaturesPage());
        }

        private void Avateering_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vitruviuskinect.com/download");
        }
    }
}
