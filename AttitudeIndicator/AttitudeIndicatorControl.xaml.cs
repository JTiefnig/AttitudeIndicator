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

namespace AttitudeIndicator
{
    /// <summary>
    /// Interaktionslogik für AttitudeIndicatorControl.xaml
    /// </summary>
    public partial class AttitudeIndicatorControl : UserControl
    {
        public AttitudeIndicatorControl()
        {
            InitializeComponent();
            
        }






        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // dont konw..

        }




        #region DependencyProperties





        public static readonly DependencyProperty PitchProperty =
        DependencyProperty.Register(nameof(Pitch),
        typeof(double),
        typeof(AttitudeIndicatorControl),
        new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(PitchPropertyChanged)));


        public double Pitch
        {
            get => (double)GetValue(PitchProperty);
            set
            {
                SetValue(PitchProperty, value);
            }
        }

        public static void PitchPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var posSlid = d as AttitudeIndicatorControl;


            

        }

        #endregion



    }
}
