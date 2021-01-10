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

            this.XYSlider.TouchDown += new EventHandler<TouchEventArgs>((s, e)=> 
            {
                this.DragState = InputState.DragXY;
                CalcXYDeflection(e.GetTouchPoint(XYBasicRect).Position);
            });

            this.XYSlider.MouseDown += (s, e) =>
            {
                this.DragState = InputState.DragXY;
                CalcXYDeflection(e.GetPosition(XYBasicRect));
            };

            this.XYBasicRect.MouseDown += (s, e) => 
            {
                this.DragState = InputState.DragXY;
                CalcXYDeflection(e.GetPosition(XYBasicRect));
            };

            // For Z Slider:

            ZBasicRect.Background = NormalBackgroundBrush;

            this.ZSlider.TouchDown += new EventHandler<TouchEventArgs>((s, e)=> 
            {
                this.DragState = InputState.DragZ;
                CalcZDeflection(e.GetTouchPoint(ZBasicRect).Position);
            });


            this.ZSlider.MouseDown += (s, e) =>
            {
                this.DragState = InputState.DragZ;
                CalcZDeflection(e.GetPosition(ZBasicRect));
            }; 
            this.ZBasicRect.MouseDown += (s,e)=>
            {
                this.DragState = InputState.DragZ;
                CalcZDeflection(e.GetPosition(ZBasicRect));
            };



            //Both 
            this.MouseMove += MouseMoveHandler;
            this.MouseLeave += (s, e)=> this.DragState = InputState.None;
            this.MouseUp += (s, e) => this.DragState = InputState.None;
            this.TouchMove += new EventHandler<TouchEventArgs>(TouchableThing_TouchMove);
            this.TouchLeave += new EventHandler<TouchEventArgs>((s, e) => this.DragState = InputState.None);


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

                if(_dragstate == InputState.DragXY)
                {
                    XDeflection = 0;
                    YDeflection = 0;
                }else if(_dragstate == InputState.DragZ )
                {
                    ZDeflection = 0;
                }
                _dragstate = value;

            }
        }
        

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (this.DragState == InputState.DragXY)
            {
                CalcXYDeflection((e as MouseEventArgs).GetPosition(this.XYBasicRect));
            }
            else if (DragState == InputState.DragZ)
            {
                CalcZDeflection((e as MouseEventArgs).GetPosition(this.ZBasicRect));
            }
        }

        

        private void TouchableThing_TouchMove(object sender, TouchEventArgs e)
        {
            if(this.DragState == InputState.DragXY)
            {
                CalcXYDeflection((e as TouchEventArgs).GetTouchPoint(this.XYBasicRect).Position);
            }
            else if(DragState == InputState.DragZ)
            {
                CalcZDeflection((e as TouchEventArgs).GetTouchPoint(this.ZBasicRect).Position);
            }
        }



        private void CalcXYDeflection(Point p)
        {
            var maxmoveX = this.XYBasicRect.ActualWidth - this.XYSlider.ActualWidth;
            var maxmoveY = this.XYBasicRect.ActualHeight - this.XYSlider.ActualHeight;
            XDeflection = (((p.X - this.XYSlider.ActualWidth / 2) / maxmoveX * 2d - 1d));
            YDeflection = (((p.Y - this.XYSlider.ActualHeight / 2) / maxmoveY * 2d - 1d));

        }


        private void CalcZDeflection(Point p)
        { 
            var maxmoveZ = this.ZBasicRect.ActualHeight - this.ZSlider.ActualHeight;
            ZDeflection = (((p.Y - this.ZSlider.ActualHeight / 2) / maxmoveZ * 2d - 1d));
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
