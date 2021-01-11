using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.ComponentModel;

namespace AttitudeIndicator.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {

        public ApplicationViewModel()
        {
            AirPlaneMatrixTransform = new Matrix3D();
            SerialDataConnection = new SerialCommunicationViewModel(new SerialMessageProcessor(this));

            Broadcaster = new UdpBroadcast(this);

            this.Modes= new List<InputMode>()
            {
                new ManualMode(this),
                new SerialInput(this)
            };

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

            this.Rotation = TransformationHelper.UnitQuaternionFromEulerAngles(Psi, Theta, Phi);
            var mat = new Matrix3D();
            mat.Rotate(this.Rotation);
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
        public SerialCommunicationViewModel SerialDataConnection { get; set; } 
            


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


        public List<InputMode> Modes { get;} 


        private InputMode _selectedMode;

        public InputMode SelectedMode 
        {
            get => _selectedMode;
            set
            {
                if (value == _selectedMode) return;

                // NEW
                value?.Enable();

                //OLD
                _selectedMode?.Disable();

                _selectedMode = value;

            }
        }



        public System.Windows.Visibility JoyStickVisibility { get; set; } = System.Windows.Visibility.Collapsed;



        public Rotator Rotator => new Rotator(this);

        #endregion


    }

    public interface InputMode
    {
        string Name { get; }
        void Enable();
        void Disable();
    }

    public class ManualMode : InputMode
    {
        public string Name => "ManualMode";

        public ApplicationViewModel AppVM {get;}

        public ManualMode(ApplicationViewModel App)
        {
            AppVM = App;
        }

        public void Disable()
        {
            AppVM.JoyStickVisibility = System.Windows.Visibility.Collapsed;
        }

        public void Enable()
        {
            AppVM.JoyStickVisibility = System.Windows.Visibility.Visible;
        }
    }

    public class SerialInput : InputMode
    {
        public string Name => "SerialMode";


        public ApplicationViewModel AppVM {get;}

        public SerialInput(ApplicationViewModel App)
        {
            AppVM = App;
        }

        public void Disable()
        {
            AppVM.SerialDataConnection.Connected = false;
        }

        public void Enable()
        {
            AppVM.SerialDataConnection.Connected = true;
        }
    }


}
