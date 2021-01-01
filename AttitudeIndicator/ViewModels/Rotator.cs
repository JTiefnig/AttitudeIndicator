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
    public class Rotator: BaseViewModel
    {
        public ApplicationViewModel AppVM { get; }

        public Rotator(ApplicationViewModel AppViewModel)
        {
            AppVM = AppViewModel;
        }



        private Task JogXTask;

        private void JogXAction()
        {
            while (jogSpeedX != 0)
            {

                var dA = LoopDelay / 1000.0 * Turnspeed * jogSpeedX;
                var D_Q = new Quaternion(new Vector3D(1, 0, 0), dA);// just for debbuggnig
                Thread.Sleep(LoopDelay);
                AppVM.Rotation *= D_Q;
            }
        }



        private double jogSpeedX=0;

        #region Properties

        public ICommand JogX => new RotationJogger(AppVM, new Vector3D(0, 0, 1));
        public ICommand JogY => new RotationJogger(AppVM, new Vector3D(0, 1, 0));


        //public ICommand JogX => new RelayParameterizedCommand((a)=> {
        //    var val = (double)a;
        //    jogSpeedX = val;

        //    if (JogXTask?.Status != TaskStatus.Running && val != 0)
        //    {
        //        JogXTask = new Task(()=>JogXAction());
        //        JogXTask.Start();
        //    }
        //});



        //public ICommand JogY => new RelayParameterizedCommand((a)=> { });

        public static int LoopDelay = 80;

        /// <summary>
        /// Maximum Turning Speed in Degree Per second
        /// </summary>
        public double Turnspeed { get; set; } = 20;

        #endregion
    }


    public class RotationJogger : ICommand
    {

        public ApplicationViewModel Rotation { get; private set; }
        public Vector3D Axis { get;  }

        public RotationJogger(ApplicationViewModel rotation, Vector3D axis)
        {
            Rotation = rotation;
            Axis = axis;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        private Task Jogger;

        private double jogspeed =0;

        /// <summary>
        /// Stetting the jog Speed 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            jogspeed = (double)parameter;


            if(Jogger?.Status != TaskStatus.Running && jogspeed!=0)
            {
                Jogger = new Task(() => jog());
                Jogger.Start();
            }



        }

        private void jog()
        {
            while(jogspeed != 0)
            {
                Rotation.Rotation *= new Quaternion(Axis, jogspeed);
                Thread.Sleep(50);
            }
        }

    }





}
