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
        static Vector3D GetEulerangle(Matrix3D mat)
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

            return new Vector3D(psi, theta, phi);
        }
    }
}
