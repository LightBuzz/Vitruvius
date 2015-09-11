using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new MainPage());
        }

        private void Purchase_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vitruviuskinect.com");
        }
    }
}
