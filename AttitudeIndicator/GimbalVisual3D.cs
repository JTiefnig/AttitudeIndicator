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
    class GimbalVisual3D : UIElement3D
    {

        private NotchRing PsiRing = new NotchRing() { Fill = new SolidColorBrush(Colors.Purple), Diameter = 0.2 };
        private NotchRing ThetaRing = new NotchRing() { Fill = new SolidColorBrush(Colors.Green), Diameter = 0.2 };
        private NotchRing PhiRing = new NotchRing() { Fill = new SolidColorBrush(Colors.Blue), Diameter = 0.2 };

        public GimbalVisual3D()
        {
            var group = new Model3DGroup();
            group.Children.Add(PsiRing.Model);
            //BetaRing.Model.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));
            group.Children.Add(ThetaRing.Model);
            group.Children.Add(PhiRing.Model);
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

            gimb.PsiRing.RingDiameter = dia;
            gimb.ThetaRing.RingDiameter = dia * 0.9;
            gimb.PhiRing.RingDiameter = dia * 0.8;
        }




        //private Transform3DGroup PhiRingTransform => { var ret = new Transform3DGroup(); return ret; }

        


        private void OrientationChanged()
        {

            var rotation = TransformationHelper.GetEulerangle(Orientation);


            rotation *= 180 / Math.PI; // RAD to Degree converstion



   
            var tgPsiRing = new Transform3DGroup();
            tgPsiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotation.X)));

            this.PsiRing.Model.Transform = tgPsiRing;

            var tgthetaRing = new Transform3DGroup();
            tgthetaRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            tgthetaRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotation.X)));
            tgthetaRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), rotation.Y)));
            
            this.ThetaRing.Model.Transform = tgthetaRing;

            var tgPhiRing = new Transform3DGroup();
            tgPhiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            tgPhiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotation.X)));
            tgPhiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), rotation.Y)));
            tgPhiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), rotation.Z)));

            this.PhiRing.Model.Transform = tgPhiRing;

        }


        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Matrix3D), typeof(GimbalVisual3D),
                                        new UIPropertyMetadata(new Matrix3D(), (s, d) => (s as GimbalVisual3D).OrientationChanged()));


        public Matrix3D Orientation
        {
            get => (Matrix3D)GetValue(OrientationProperty);
            set { SetValue(OrientationProperty, value); }
        }

        




        #endregion

    }
}
