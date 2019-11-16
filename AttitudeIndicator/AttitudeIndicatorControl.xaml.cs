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

            this.HorizontCanvas.Children.Clear();

            var col = Brushes.Black;
            

            var ln = new Line() { X1 = 10, X2 = 100, Y1 = 50, Y2 = 50, Fill=col};

            this.HorizontCanvas.Children.Add(ln);


        }





    }
}
