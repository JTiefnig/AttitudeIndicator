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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttitudeIndicator
{
    /// <summary>
    /// Interaktionslogik für TouchJoyStick.xaml
    /// </summary>
    public partial class TouchJoyStick : UserControl
    {
        public TouchJoyStick()
        {
            InitializeComponent();

            BasicRect.Background= NormalBackgroundBrush;

            this.Slider.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);

            this.Slider.MouseDown += SliderMouseDown;
            this.BasicRect.MouseDown += SliderMouseDown;
            this.MouseMove += SliderMouseMove;

            this.MouseLeave += SliderMouseLeave;
            this.MouseUp += SliderMouseLeave;

            this.TouchMove += new EventHandler<TouchEventArgs>(TouchableThing_TouchMove);

            this.TouchLeave += new EventHandler<TouchEventArgs>(TouchableThing_TouchLeave);
        }

        ColorAnimation fadein => new ColorAnimation(NormalBackGroundColor, TouchDownBackGroundColor, new Duration(TimeSpan.FromMilliseconds(300)));
        ColorAnimation fadeout => new ColorAnimation(TouchDownBackGroundColor, NormalBackGroundColor, new Duration(TimeSpan.FromMilliseconds(100)));

        public SolidColorBrush NormalBackgroundBrush => new SolidColorBrush(NormalBackGroundColor);


        public Color NormalBackGroundColor { get; set; } = Color.FromArgb(0, 255, 255, 255);
        public Color TouchDownBackGroundColor { get; set; } = Color.FromArgb(200, 150, 150, 150);

        private DrawingGroup backingStore = new DrawingGroup();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            UIElementTransformer();
            drawingContext.DrawDrawing(backingStore);
        }

        private void UIElementTransformer()
        {
            var translx = XDeflection * 0.49 * (this.BasicRect.ActualWidth - this.Slider.ActualWidth);
            var transly = YDeflection * 0.49 * (this.BasicRect.ActualHeight - this.Slider.ActualHeight);

            TranslateTransform transform = new TranslateTransform(translx, transly);

            this.Slider.RenderTransform = transform;
        }


        private bool isDragging_;
        private bool isDragging
        {
            get => isDragging_;
            set
            {
                if (value == isDragging_)
                    return;
                isDragging_ = value;

                if (value)
                {
                    ((SolidColorBrush)BasicRect.Background).BeginAnimation(SolidColorBrush.ColorProperty,
                    fadein);
                }
                else
                {
                    ((SolidColorBrush)BasicRect.Background).BeginAnimation(SolidColorBrush.ColorProperty,
                    fadeout);
                    XDeflection = 0;
                    YDeflection = 0;
                }

            }
        }

        private void TouchableThing_TouchDown(object sender, TouchEventArgs e)
        {
            isDragging = true;

        }


        private void SliderMouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;

            var myPoint = (e as MouseEventArgs).GetPosition(this.BasicRect);

            CalcDeflectionFromPoint(myPoint);

        }

        private void SliderMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            var myPoint = (e as MouseEventArgs).GetPosition(this.BasicRect);

            CalcDeflectionFromPoint(myPoint);

        }

        private void SliderMouseLeave(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void TouchableThing_TouchMove(object sender, TouchEventArgs e)
        {
            if (!isDragging)
                return;

            var myPoint = (e as TouchEventArgs).GetTouchPoint(this.BasicRect).Position;
            CalcDeflectionFromPoint(myPoint);

        }



        private void CalcDeflectionFromPoint(Point p)
        {
            var maxmoveX = this.BasicRect.ActualWidth - this.Slider.ActualWidth;
            var maxmoveY = this.BasicRect.ActualHeight - this.Slider.ActualHeight;


            XDeflection = (((p.X - this.Slider.ActualWidth / 2) / maxmoveX * 2d - 1d));
            YDeflection = (((p.Y - this.Slider.ActualHeight / 2) / maxmoveY * 2d - 1d));
        }



        private void TouchableThing_TouchLeave(object sender, TouchEventArgs e)
        {
            isDragging = false;
        }

        #region DependencyProperty

        public static readonly DependencyProperty XDeflectionProperty =
        DependencyProperty.Register(nameof(XDeflection),
        typeof(double),
        typeof(TouchJoyStick),
        new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(DeflectionChanged)));

        /// <summary>
        /// a value betweet 100 and -100 (Percent)
        /// </summary>
        public double XDeflection
        {
            get => (double)GetValue(XDeflectionProperty);
            set
            {
                double deflection;
                if (value > 1)
                    deflection = 1;
                else if (value < -1)
                    deflection = -1;
                else
                    deflection = value;

                SetValue(XDeflectionProperty, deflection);
            }
        }

        /// <summary>
        /// Deflection in the second direction 
        /// </summary>
        public static readonly DependencyProperty YDeflectionProperty =
        DependencyProperty.Register(nameof(YDeflection),
        typeof(double),
        typeof(TouchJoyStick),
        new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(DeflectionChanged)));

        public double YDeflection
        {
            get => (double)GetValue(YDeflectionProperty);
            set
            {
                double deflection;
                if (value > 1)
                    deflection = 1;
                else if (value < -1)
                    deflection = -1;
                else
                    deflection = value;

                SetValue(YDeflectionProperty, deflection);
            }
        }


        public static void DeflectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (d as TouchJoyStick);
            obj.UIElementTransformer();

            obj.Command?.Execute(new Point(obj.XDeflection, obj.YDeflection));

            obj.CommandX?.Execute(obj.XDeflection);
            obj.CommandY?.Execute(obj.YDeflection);
            // do nothing for now
        }

        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(TouchJoyStick),
            new UIPropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        //obj.Command?.Execute(obj.Deflection);



        public static readonly DependencyProperty CommandXProperty =
        DependencyProperty.Register(
            nameof(CommandX),
            typeof(ICommand),
            typeof(TouchJoyStick),
            new UIPropertyMetadata(null));

        public ICommand CommandX
        {
            get { return (ICommand)GetValue(CommandXProperty); }
            set { SetValue(CommandXProperty, value); }
        }


        public static readonly DependencyProperty CommandYProperty =
            DependencyProperty.Register(
                nameof(CommandY),
                typeof(ICommand),
                typeof(TouchJoyStick),
                new UIPropertyMetadata(null));

        public ICommand CommandY
        {
            get { return (ICommand)GetValue(CommandYProperty); }
            set { SetValue(CommandYProperty, value); }
        }



        #endregion DependencyProperty
    }
}
