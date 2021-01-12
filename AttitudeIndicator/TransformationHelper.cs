using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AttitudeIndicator
{

    public struct EulerAngles
    {
        public EulerAngles(double Psi, double Theta, double Phi)
        {
            psi = Psi;
            theta = Theta;
            phi = Phi;
        }

            double psi;
        double theta;
        double phi;
    }



    public static class TransformationHelper
    {

        /// <summary>
        /// Calculating the Euler Angles form a Rotation Matrix - Has a bug!! will work on that
        /// Reference: https://www.gregslabaugh.net/publications/euler.pdf
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Vector3D GetEulerangle(Matrix3D mat)
        {
            double psi = 0;
            double theta = 0;
            double phi = 0;

            // does not work as expected!!! 

            if(mat.M31 < 1)
            {
                if(mat.M31 > -1)
                {
                    theta = Math.Asin( -mat.M31);
                    psi = Math.Atan2(mat.M21, mat.M11);
                    phi = Math.Atan2(mat.M32, mat.M33);
                }
                else
                {
 
                    theta = Math.PI / 2;
                    psi = -Math.Atan2((-1)*mat.M23, mat.M22);
                    phi = 0;
                }
            }
            else
            {
                theta = -Math.PI / 2;
                psi = Math.Atan2(-mat.M23, mat.M22);
                phi = 0;
            }


            return new Vector3D(phi, theta, psi);
        }

        public static Quaternion UnitQuaternionFromEulerAngles(double Psi, double Theta, double Phi)
        {
            var q = new Quaternion(new Vector3D(0, 0, 1), Psi);
            q *= new Quaternion(new Vector3D(0, 1, 0), Theta);
            q *= new Quaternion(new Vector3D(1, 0, 0), Phi);
            return q;
        }

        /// <summary>
        /// Calculating Euler Angles form Quaternion... 
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3D QuaternionToEuler(Quaternion q)
        {

            var angles = new Vector3D();

            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                angles.Y = Math.Sign(sinp) * Math.PI / 2;  
            else
                angles.Y = Math.Asin(sinp);

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = Math.Atan2(siny_cosp, cosy_cosp);


            return angles;
        }
    }
}
