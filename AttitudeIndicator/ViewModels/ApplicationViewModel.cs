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
            SerialDataConnection = new SerialCommunicationViewModel();

 
        }


        void CalculateTransform()
        {

            var q = new Quaternion(new Vector3D(1, 0, 0), Psi);

            q *= new Quaternion(new Vector3D(0, 1, 0), Theta);
            q *= new Quaternion(new Vector3D(0, 0, 1), Phi);


            var mat = new Matrix3D();
            mat.Rotate(q);
            this.AirPlaneMatrixTransform = mat;
        }



        #region Properties


        public SerialCommunicationViewModel SerialDataConnection
        {
            get; set;
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

    }
}
