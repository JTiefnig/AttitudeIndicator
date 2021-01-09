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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttitudeIndicator
{
    /// <summary>
    /// Interaktionslogik für TouchJoyStick.xaml
    /// </summary>
    public partial class TouchJoyStick : UserControl
    {


        private enum InputState { None, DragXY, DragZ}


        public TouchJoyStick()
        {
            InitializeComponent();

            // For XY Slider:

            XYBasicRect.Background= NormalBackgroundBrush;

            this.XYSlider.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);

            this.XYSlider.MouseDown += ( s,  e) => { mousedodou };

            this.XYBasicRect.MouseDown += SliderMouseDown;

            // For Z Slider:

            ZBasicRect.Background = NormalBackgroundBrush;

            this.ZSlider.TouchDown += new EventHandler<TouchEventArgs>(TouchableThing_TouchDown);
            this.ZSlider.MouseDown += SliderMouseDown;
            this.ZBasicRect.MouseDown += SliderMouseDown;



            //Both 
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
            var translx = XDeflection * 0.49 * (this.XYBasicRect.ActualWidth - this.XYSlider.ActualWidth);
            var transly = YDeflection * 0.49 * (this.XYBasicRect.ActualHeight - this.XYSlider.ActualHeight);
            this.XYSlider.RenderTransform = new TranslateTransform(translx, transly);

            var translz = ZDeflection * 0.49 * (this.ZBasicRect.ActualHeight - this.ZSlider.ActualHeight);
            this.ZSlider.RenderTransform = new TranslateTransform(0.0, translz);
        }


        private void StartAnimation(InputState from, InputState to)
        {
            if(to == InputState.DragXY)
                ((SolidColorBrush)XYBasicRect.Background).BeginAnimation(SolidColorBrush.ColorProperty, fadein);
            if (to == InputState.DragZ)
                ((SolidColorBrush)ZBasicRect.Background).BeginAnimation(SolidColorBrush.ColorProperty, fadein);

            if(from == InputState.DragXY)
                ((SolidColorBrush)XYBasicRect.Background).BeginAnimation(SolidColorBrush.ColorProperty, fadeout);
            if (from == InputState.DragZ)
                ((SolidColorBrush)ZBasicRect.Background).BeginAnimation(SolidColorBrush.ColorProperty, fadeout);
        }


        private InputState _dragstate = InputState.None;

        private InputState DragState
        {
            get => _dragstate;
            set
            {
                if (value == _dragstate)
                    return;

                StartAnimation(_dragstate, value);

                _dragstate = value;

            }
        }




        private void TouchableThing_TouchDown(object sender, TouchEventArgs e)
        {

        }


        private void SliderMouseDown(object sender, MouseEventArgs e)
        {

            var obj = (sender as Control);


            var myPoint = (e as MouseEventArgs).GetPosition(this.XYBasicRect);

            CalcDeflectionFromPoint(myPoint);

        }

        private void SliderMouseMove(object sender, MouseEventArgs e)
        {
            if()
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


        /// <summary>
        /// Deflection in the second direction 
        /// </summary>
        public static readonly DependencyProperty ZDeflectionProperty =
        DependencyProperty.Register(nameof(ZDeflection),
        typeof(double),
        typeof(TouchJoyStick),
        new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(DeflectionChanged)));

        public double ZDeflection
        {
            get => (double)GetValue(ZDeflectionProperty);
            set
            {
                double deflection;
                if (value > 1)
                    deflection = 1;
                else if (value < -1)
                    deflection = -1;
                else
                    deflection = value;

                SetValue(ZDeflectionProperty, deflection);
            }
        }


        public static void DeflectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (d as TouchJoyStick);
            obj.UIElementTransformer();

            obj.Command?.Execute(new Vector3D(obj.XDeflection, obj.YDeflection, obj.ZDeflection));
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


        #endregion DependencyProperty
    }
}
