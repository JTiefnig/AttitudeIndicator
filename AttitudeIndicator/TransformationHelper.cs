using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AttitudeIndicator
{


    public static class TransformationHelper
    {

        /// <summary>
        /// Calculating Euler angles
        /// Reference: https://www.gregslabaugh.net/publications/euler.pdf
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Vector3D GetEulerangle(Matrix3D mat)
        {
            double psi = 0;
            double theta = 0;
            double phi = 0;

            if (Math.Abs(mat.M31) != 0)
            {
                theta = -Math.Asin(mat.M31);
                psi = Math.Atan2(mat.M32 / Math.Cos(theta), mat.M33 / Math.Cos(theta));
                phi = Math.Atan2(mat.M21 / Math.Cos(theta), mat.M11 / Math.Cos(theta));
            }
            else
            {
                phi = 0;
                if (mat.M31 == -1)
                {
                    theta = Math.PI / 2;
                    psi = phi + Math.Atan2(mat.M12, mat.M13);
                }
                else
                {
                    theta = -Math.PI / 2;
                    psi = -phi + Math.Atan2(-mat.M12, -mat.M13);
                }
            }

            return new Vector3D(phi, theta, psi);
        }


        public static Vector3D QuaternionToEuler(Quaternion q)
        {

            var angles = new Vector3D();

            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.Y = Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                angles.Y = Math.Sign(sinp) * Math.PI / 2;  
            else
                angles.Y = Math.Asin(sinp);

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.X = Math.Atan2(siny_cosp, cosy_cosp);


            return angles;
        }
    }
}
