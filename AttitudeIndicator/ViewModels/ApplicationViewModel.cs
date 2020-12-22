using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.ComponentModel;

namespace AttitudeIndicator.ViewModels
{
    public class ApplicationViewModel: BaseViewModel
    {

        public ApplicationViewModel()
        {
            AirPlaneMatrixTransform = new Matrix3D();
            SerialDataConnection = new SerialCommunicationViewModel(new SerialMessageProcessor(this));

            // for debugging
            SerialDataConnection.SelectedPortName = "COM4";
            SerialDataConnection.Connected = true;

            Broadcaster = new UdpBroadcast(this);
        }
    


        void RotationChanged()
        {

            var mat = new Matrix3D();
            mat.Rotate(Rotation);
            this.AirPlaneMatrixTransform = mat;


            var euler = TransformationHelper.GetEulerangle(mat);


            euler *= 180.0 / Math.PI;

            this.Psi = euler.X;
            this.Theta = euler.Y;
            this.Phi = euler.Z;

        }

        void CalculateTransform()
        {

        

            //var q = new Quaternion(new Vector3D(0, 0, 1), Psi);
            //q *= new Quaternion(new Vector3D(0, 1, 0), Theta);
            //q *= new Quaternion(new Vector3D(1, 0, 0), Phi);

            //this.Rotation = q;


            //var mat = new Matrix3D();
            //mat.Rotate(q);
            //this.AirPlaneMatrixTransform = mat;
        }


        #region Properties

        /// <summary>
        /// ViewModel for Network Broadcasting
        /// </summary>
        public UdpBroadcast Broadcaster { get; }

        /// <summary>
        /// Viewmodel for Serial Input!
        /// </summary>
        public SerialCommunicationViewModel SerialDataConnection
        {
            get; set;
        }


        /// <summary>
        /// ViewModel for Network Broadcasting
        /// </summary>
        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
                RotationChanged();
            }
        }


        /// <summary>
        /// Psi ... Yaw
        /// </summary>
        private double _psi;
        public double Psi
        {
            get => _psi;
            set
            {
                _psi = value;
                OnPropertyChanged(nameof(Psi));
                CalculateTransform();
            }
        }


        /// <summary>
        /// Theta ... Pitch
        /// </summary>
        private double _theta;
        public double Theta
        {
            get => _theta;
            set
            {
                _theta = value;
                OnPropertyChanged(nameof(Theta));
                CalculateTransform();
            }

        }

        /// <summary>
        /// Phi ... Roll
        /// </summary>
        private double _phi;
        public double Phi
        {
            get => _phi;
            set
            {
                _phi = value;
                OnPropertyChanged(nameof(Phi));
                CalculateTransform();
            }
        }



        /// <summary>
        /// Representation of the Orientation in 3d
        /// </summary>
        private Matrix3D _airplaneTranform;
        public Matrix3D AirPlaneMatrixTransform
        {
            get => _airplaneTranform;
            set
            {
                _airplaneTranform = value;
                OnPropertyChanged(nameof(AirPlaneMatrixTransform));
            }

        }

        #endregion



        public Point3DCollection Ring => CreatePath(0, Math.PI * 2, 100, u => Math.Cos(u)*5, u => Math.Sin(u)*5, (u)=>0);


        private Point3DCollection CreatePath(double min, double max, int n, Func<double, double> fx, Func<double, double> fy, Func<double, double> fz)
        {
            var list = new Point3DCollection(n);
            for (int i = 0; i < n; i++)
            {
                double u = min + (max - min) * i / n;
                list.Add(new Point3D(fx(u), fy(u), fz(u)));
            }
            return list;
        }
    }
}
