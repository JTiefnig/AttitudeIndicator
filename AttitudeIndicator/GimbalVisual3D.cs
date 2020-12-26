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
            group.Children.Add(ThetaRing.Model);
            group.Children.Add(PhiRing.Model);
            this.Visual3DModel = group;
            OrientationChanged();
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

        /// <summary>
        /// If the size of the Gimbal is changed, the rings must be resized as well
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void DiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gimb = d as GimbalVisual3D;

            double dia = (double)e.NewValue;

            gimb.PsiRing.RingDiameter = dia;
            gimb.ThetaRing.RingDiameter = dia * 0.9;
            gimb.PhiRing.RingDiameter = dia * 0.8;
        }
  

        /// <summary>
        /// Transform individual rings accordingly
        /// </summary>
        private void OrientationChanged()
        {

            var rotation = TransformationHelper.QuaternionToEuler(Orientation);


            rotation *= 180 / Math.PI; // RAD to Degree converstion

            var q = new Quaternion(new Vector3D(0, 0, -1), rotation.Z);
            var tgPsiRing = new Transform3DGroup();
            tgPsiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(q.Axis, q.Angle)));
            this.PsiRing.Model.Transform = tgPsiRing;
            
            


            var tgthetaRing = new Transform3DGroup();
            q *= new Quaternion(new Vector3D(0, -1, 0), rotation.Y);
            tgthetaRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            tgthetaRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(q.Axis, q.Angle)));
           
            
            this.ThetaRing.Model.Transform = tgthetaRing;
            q *= new Quaternion(new Vector3D(1, 0, 0), rotation.X);
            var tgPhiRing = new Transform3DGroup();
            tgPhiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            tgPhiRing.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(q.Axis, q.Angle)));
            this.PhiRing.Model.Transform = tgPhiRing;

        }


        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Quaternion), typeof(GimbalVisual3D),
                                        new UIPropertyMetadata(new Quaternion(), (s, d) => (s as GimbalVisual3D).OrientationChanged()));


        /// <summary>
        /// Property for the current flight orientation
        /// </summary>
        public Quaternion Orientation
        {
            get => (Quaternion)GetValue(OrientationProperty);
            set { SetValue(OrientationProperty, value); }
        }



        #endregion

    }
}
