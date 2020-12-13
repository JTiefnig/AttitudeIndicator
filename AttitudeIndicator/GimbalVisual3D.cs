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

        private readonly NotchRing AlphaRing = new NotchRing() { Fill = new SolidColorBrush(Colors.Purple), Diameter = 0.2 };
        private readonly NotchRing BetaRing = new NotchRing() { Fill = new SolidColorBrush(Colors.Green), Diameter = 0.2 };
        private readonly NotchRing GammaRing = new NotchRing() { Fill = new SolidColorBrush(Colors.Blue), Diameter = 0.2 };

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
                                        new UIPropertyMetadata(5.0, DiameterChanged));

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
            gimb.BetaRing.RingDiameter = dia*0.9;
            gimb.GammaRing.RingDiameter = dia*0.8;
        }



        


        private void OrientationChanged()
        {
            // NO thats not how it works...clarly
            //double psi = Vector3D.AngleBetween(new Vector3D(1, 0, 0), Orientation.Transform(new Vector3D(1, 0, 0)));
            //double theta = Vector3D.AngleBetween(new Vector3D(0, 1, 0), Orientation.Transform(new Vector3D(0, 1, 0)));
            //double phi = Vector3D.AngleBetween(new Vector3D(0, 0, 1), Orientation.Transform(new Vector3D(0, 0, 1)));


            


        }


        public RotateTransform3D Orientation
        {
            get; set;
        }

        #endregion

    }
}
