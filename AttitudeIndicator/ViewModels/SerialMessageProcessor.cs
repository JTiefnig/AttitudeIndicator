using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

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



        private Boolean[] FlagArray = { false, false, false, false }; 
        private void ResetFlagArray()
        {
            for (uint i = 0; i < FlagArray.Length; i++)
                FlagArray[i] = false; 
        }

        private Quaternion Buffer = new Quaternion();

        private bool allFlags()
        {
            foreach (var flag in FlagArray)
                if (!flag)
                    return false;

            return true;
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
                        Buffer.W = value;
                        FlagArray[0] = true;
                        break;
                    case 2:
                        Buffer.X = value;
                        FlagArray[1] = true;
                        break;
                    case 3:
                        Buffer.Y = value;
                        FlagArray[2] = true;
                        break;
                    case 4:
                        Buffer.Z = value;
                        FlagArray[3] = true;
                        break;
                }


                if(allFlags())
                {
                    ResetFlagArray();

                    AppVM.Rotation = Buffer;
                }

            }
            catch
            { Console.WriteLine("EXC"); return; }
            
            
            



        }



    }
}
