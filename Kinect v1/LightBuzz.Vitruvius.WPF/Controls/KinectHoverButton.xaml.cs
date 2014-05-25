using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightBuzz.Vitruvius.Controls
{
    /// <summary>
    /// Interaction logic for KinectHoverButton.xaml
    /// </summary>
    public partial class KinectHoverButton : UserControl
    {
        #region Constants

        readonly Point FROM = new Point(0, 0);
        readonly Point TO = new Point(0, 4);
        readonly Duration REVERSE_DURATION = new Duration(TimeSpan.FromMilliseconds(200));

        #endregion

        #region Members

        Duration _duration;
        PointAnimation _animation;

        bool _hasIntersected;

        #endregion

        #region Dependency properties

        /// <summary>
        /// Timer interval.
        /// </summary>
        public int Time
        {
            get { return (int)this.GetValue(TimeProperty); }
            set { this.SetValue(TimeProperty, value); }
        }
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(int), typeof(KinectHoverButton), new PropertyMetadata((int)2));

        /// <summary>
        /// Image width.
        /// </summary>
        public double ImageWidth
        {
            get { return (double)this.GetValue(ImageWidthProperty); }
            set { this.SetValue(ImageWidthProperty, value); }
        }
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(
            "ImageWidth", typeof(double), typeof(KinectHoverButton), new PropertyMetadata((double)70.0));

        /// <summary>
        /// Image height.
        /// </summary>
        public double ImageHeight
        {
            get { return (double)this.GetValue(ImageHeightProperty); }
            set { this.SetValue(ImageHeightProperty, value); }
        }
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(
            "ImageHeight", typeof(double), typeof(KinectHoverButton), new PropertyMetadata((double)70.0));

        /// <summary>
        /// Image margin.
        /// </summary>
        public Thickness ImageMargin
        {
            get { return (Thickness)this.GetValue(ImageMarginProperty); }
            set { this.SetValue(ImageMarginProperty, value); }
        }
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(
            "ImageMargin", typeof(Thickness), typeof(KinectHoverButton), new PropertyMetadata(new Thickness(0, 30, 0, 5)));

        /// <summary>
        /// The background color.
        /// </summary>
        public Color BackgroundColor
        {
            get { return (Color)this.GetValue(BackgroundColorProperty); }
            set { this.SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            "BackgroundColor", typeof(Color), typeof(KinectHoverButton), new PropertyMetadata(Colors.White));

        /// <summary>
        /// The stroke color.
        /// </summary>
        public Color StrokeColor
        {
            get { return (Color)this.GetValue(StrokeColorProperty); }
            set { this.SetValue(StrokeColorProperty, value); }
        }
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            "StrokeColor", typeof(Color), typeof(KinectHoverButton), new PropertyMetadata(Colors.CornflowerBlue));

        /// <summary>
        /// The text color.
        /// </summary>
        public Brush TextColorNormal
        {
            get { return (Brush)this.GetValue(TextColorNormalProperty); }
            set { this.SetValue(TextColorNormalProperty, value); }
        }
        public static readonly DependencyProperty TextColorNormalProperty = DependencyProperty.Register(
            "TextColorNormal", typeof(Brush), typeof(KinectHoverButton), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// The hover text color.
        /// </summary>
        public Brush TextColorHover
        {
            get { return (Brush)this.GetValue(TextColorHoverProperty); }
            set { this.SetValue(TextColorHoverProperty, value); }
        }
        public static readonly DependencyProperty TextColorHoverProperty = DependencyProperty.Register(
            "TextColorHover", typeof(Brush), typeof(KinectHoverButton), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Display text.
        /// </summary>
        public string DisplayText
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "DisplayText", typeof(string), typeof(KinectHoverButton), new PropertyMetadata(""));

        /// <summary>
        /// Image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(KinectHoverButton), new PropertyMetadata(null));

        #endregion

        #region Events

        public event EventHandler Pressed;

        #endregion

        #region Constructor

        public KinectHoverButton()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Methods

        void BeginUpdateUI()
        {
            _animation = new PointAnimation
            {
                From = FROM,
                To = TO,
                Duration = _duration
            };

            _animation.Completed += Animation_Completed;

            Effect.BeginAnimation(LinearGradientBrush.EndPointProperty, _animation);
        }

        void EndUpdateUI()
        {
            _animation.Completed -= Animation_Completed;
            _animation = new PointAnimation
            {
                From = Effect.EndPoint,
                To = FROM,
                Duration = REVERSE_DURATION
            };

            Effect.BeginAnimation(LinearGradientBrush.EndPointProperty, _animation);
        }

        public void Update(Control cursor)
        {
            bool intersects = cursor.IntersectsWith(this);

            if (intersects && !_hasIntersected)
            {
                // Cursor just hovered.
                _hasIntersected = true;

                BeginUpdateUI();
            }
            else if (!intersects && _hasIntersected)
            {
                // Cursor just left.
                _hasIntersected = false;

                EndUpdateUI();
            }
        }

        #endregion

        #region Event handlers

        void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _duration = new Duration(TimeSpan.FromSeconds(Time));
        }

        void Animation_Completed(object sender, EventArgs e)
        {
            if (Pressed != null)
            {
                Pressed(sender, e);
            }

            Effect.BeginAnimation(LinearGradientBrush.EndPointProperty, null);

            _hasIntersected = false;
        }

        #endregion
    }
}
