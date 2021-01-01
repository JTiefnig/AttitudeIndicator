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
            //SerialDataConnection.Connected = true;

            Broadcaster = new UdpBroadcast(this);
        }
    


        /// <summary>
        /// Defining diffrent Conversion directions
        /// This is needed to keep the system conistend and avoid stack overflow 
        /// otherwise ther would be a pingpong situation between property changed events
        /// </summary>
        private enum TransformDirection
        {
            None,
            QuaternionToEuler,
            EulerToQuaternion
        }
        private TransformDirection ProcessDirection = TransformDirection.None;



        /// <summary>
        /// Conversion of Quaternion to Euler angles
        /// </summary>
        void RotationToEulerAngles()
        {
            if (this.ProcessDirection != TransformDirection.None)
                return;

            this.ProcessDirection = TransformDirection.QuaternionToEuler;

            var mat = new Matrix3D();
            mat.Rotate(Rotation);
            this.AirPlaneMatrixTransform = mat;


            var euler = TransformationHelper.QuaternionToEuler(Rotation);

            euler *= 180.0 / Math.PI;

            this.Psi = euler.Z;
            this.Theta = euler.Y;
            this.Phi = euler.X;

            this.ProcessDirection = TransformDirection.None;

        }


        /// <summary>
        /// Conversion of Euler angles to quaternion
        /// </summary>
        void EulerAnglesToRotation()
        {

            if (this.ProcessDirection != TransformDirection.None)
                return;

            this.ProcessDirection = TransformDirection.EulerToQuaternion;

            var q = new Quaternion(new Vector3D(0, 0, 1), Psi);
            q *= new Quaternion(new Vector3D(0, 1, 0), Theta);
            q *= new Quaternion(new Vector3D(1, 0, 0), Phi);

            this.Rotation = q;
            var mat = new Matrix3D();
            mat.Rotate(q);
            this.AirPlaneMatrixTransform = mat;

            this.ProcessDirection = TransformDirection.None;


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
                RotationToEulerAngles();
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
                EulerAnglesToRotation();
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
                EulerAnglesToRotation();
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
                EulerAnglesToRotation();
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



        public Rotator Rotator => new Rotator(this);

        #endregion


    }
}
