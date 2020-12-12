using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Media;


namespace AttitudeIndicator
{
    class GimbalVisual3D: UIElement3D
    {

        private readonly NocheRing AlphaRing = new NocheRing() { Fill = new SolidColorBrush(Colors.Purple), Diameter = 0.2 };
        private readonly NocheRing BetaRing = new NocheRing() { Fill = new SolidColorBrush(Colors.Green), Diameter = 0.2 };
        private readonly NocheRing GammaRing = new NocheRing() { Fill = new SolidColorBrush(Colors.Blue), Diameter = 0.2 };

        public GimbalVisual3D()
        {
            var group = new Model3DGroup();
            group.Children.Add(AlphaRing.Model);
            //BetaRing.Model.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));
            group.Children.Add(BetaRing.Model);
            group.Children.Add(GammaRing.Model);
            this.Visual3DModel = group;
        }



        #region Properties

        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(GimbalVisual3D),
                                        new UIPropertyMetadata(6.0, DiameterChanged));

        public double Diameter
        {
            get => (double)GetValue(DiameterProperty);
            set { SetValue(DiameterProperty, value); }
        }

        private static void DiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gimb = d as GimbalVisual3D;

            double dia = (double)e.NewValue;

            gimb.AlphaRing.RingDiameter = dia;
            gimb.AlphaRing.RingDiameter = dia*0.95;
            gimb.AlphaRing.RingDiameter = dia*0.9;
        }



        private void OrientationChanged()
        {
            
        }


        public RotateTransform3D Orientation
        {
            get; set;
        }

        #endregion

    }
}
