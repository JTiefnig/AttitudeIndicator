using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AttitudeIndicator
{
    class HorizontCanvas : Canvas
    {

        /// <summary>
        /// Pitch
        /// </summary>
        public static readonly DependencyProperty PitchProperty =
                DependencyProperty.Register(nameof(Pitch), typeof(double), typeof(HorizontCanvas),
                new PropertyMetadata(0.0, Update));

        /// <summary>
        /// The radius of this pie piece
        /// </summary>
        public double Pitch
        {
            get { return (double)GetValue(PitchProperty); }
            set { SetValue(PitchProperty, value); }
        }



        private static void Update(DependencyObject obj, DependencyPropertyChangedEventArgs eventArgs)
        {
            var sender = obj as HorizontCanvas;


            sender.intUpdate();
        }


        private void intUpdate()
        {
            this.Children.Clear();
            var mybrush = Brushes.Gray;

            int divisions = 5;
            int interval = 20;

            double pixelintervall = ActualHeight / divisions;

            double offset = (Pitch % interval) + (divisions / 2.0d - 1);

            for (int i = 0; i < divisions; i++)
            {
                var ln = new Line() { X1 = -10, X2 = 10, Y1 = (offset + i * 10) };

                this.Children.Add(ln);
            }

        }








    }
}
