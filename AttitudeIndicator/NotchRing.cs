using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace AttitudeIndicator
{
    public class NotchRing : TubeVisual3D
    {
        /// <summary>
        /// Contsructor
        /// </summary>
        public NotchRing() {

            this.Path = CreatePath();

           

        }

        #region Properties


        public static readonly DependencyProperty RingDiameterProperty =
            DependencyProperty.Register(nameof(RingDiameter), typeof(double), typeof(NotchRing),
                                        new UIPropertyMetadata(3.0, PathChanged));

        public double RingDiameter
        {
            get => (double)GetValue(RingDiameterProperty);
            set { SetValue(RingDiameterProperty, value); }
        }

        private static void PathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ring = d as NotchRing;

            ring.Path = ring.CreatePath();
            ring.OnGeometryChanged();
        }

        #endregion


        

        


        protected override MeshGeometry3D Tessellate()
        {


            // sadly the base property Path is not virtual
            


            return base.Tessellate();
            
        }


        private Point3DCollection CreatePath()
        {

            double min = 0;
            double max = Math.PI * 2;

            int n = 125;


            var list = new Point3DCollection(n);
            for (int i = 0; i < n; i++)
            {
                double u = min + (max - min) * i / n;


                
                var prop = Math.Abs(i - n / 2);
                double widener = 1;
                if (prop < 3)
                    widener += 0.1* (3-prop)/3.0;



                var x = Math.Sin(u) * RingDiameter*widener;
                var y = Math.Cos(u) * RingDiameter*widener; 


                list.Add(new Point3D(x, y, 0));
            }
            return list;
        }

    }
}
