using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using System.Windows.Media.Media3D;

namespace AttitudeIndicator.ViewModels
{
    public class Rotator: ICommand
    {
        public ApplicationViewModel AppVM { get; }

        public Rotator(ApplicationViewModel AppViewModel)
        {
            AppVM = AppViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        private Task Jogger;

        private Vector3D jogspeed = new Vector3D();

        /// <summary>
        /// Stetting the jog Speed 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            jogspeed = (Vector3D)parameter;


            if (Jogger?.Status != TaskStatus.Running && jogspeed.Length != 0)
            {
                Jogger = new Task(() => jog());
                Jogger.Start();
            }
        }

        private void jog()
        {
            while (jogspeed.Length != 0)
            {
                var scaledJS = jogspeed * Turnspeed * LoopDelay / 1000.0;

                if(Mode == ControlMode.AirplaneConrol)
                {
                    var q = AppVM.Rotation;
                    q *= new Quaternion(new Vector3D(0, 0, 1), scaledJS.Z);
                    q *= new Quaternion(new Vector3D(0, 1, 0), scaledJS.Y);
                    q *= new Quaternion(new Vector3D(1, 0, 0), scaledJS.X);
                    AppVM.Rotation = q;

                }else if(Mode == ControlMode.AngleJogger)
                {
                    var curr_orient = new Vector3D(AppVM.Psi, AppVM.Theta, AppVM.Phi);

                    curr_orient += scaledJS;

                    AppVM.Psi = curr_orient.X;
                    AppVM.Theta = curr_orient.Y;
                    AppVM.Phi = curr_orient.Z;

                }

                Thread.Sleep(LoopDelay);
            }
        }


        public enum ControlMode
        {
            AngleJogger,
            AirplaneConrol
        }

        public ControlMode Mode
        {
            get; set;
        } = ControlMode.AirplaneConrol;

        public static int LoopDelay = 80;

        /// <summary>
        /// Maximum Turning Speed in Degree Per second
        /// </summary>
        public double Turnspeed { get; set; } = 80;

    }








}
