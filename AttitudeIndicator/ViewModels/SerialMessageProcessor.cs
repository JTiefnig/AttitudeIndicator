using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttitudeIndicator.ViewModels
{
    public class SerialMessageProcessor
    {

        public SerialMessageProcessor(ApplicationViewModel appvm)
        {
            AppVM = appvm;
        }



        public ApplicationViewModel AppVM
        {
            get; 
        }

        public void ProcessMessage(byte[] msgar)
        {

            if (msgar.Length != 9)
                return;

            byte id = msgar[0];



            double value = 0;
            try
            {
                value = System.BitConverter.ToDouble(msgar, 1);

                //Console.WriteLine(String.Format("ID: {0}; val: {1}", id, value));

                switch (id)
                {
                    case 1:
                        AppVM.Psi = value;
                        break;
                    case 2:
                        AppVM.Theta = value;
                        break;
                    case 3:
                        AppVM.Phi = value;
                        break;
                }


            }
            catch
            { Console.WriteLine("EXC"); return; }
            

           



        }



    }
}
