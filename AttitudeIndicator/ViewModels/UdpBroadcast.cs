using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AttitudeIndicator.ViewModels
{
    public class UdpBroadcast: BaseViewModel
    {

        public UdpBroadcast(ApplicationViewModel appvm)
        {
            AppVM = appvm;
        }


        /// <summary>
        /// Property if the Broadcaster is running
        /// </summary>
        private bool _broadcast;
        public bool Broadcast
        {
            get => _broadcast;
            set
            {
                _broadcast = value;
                if(_broadcast)
                {
                    Task.Run(() => Broadcaster());
                }
            }
        }



        void Broadcaster()
        {
            while(this.Broadcast == true)
            {

                SendPacket();

                Thread.Sleep(SendIntervall);
            }
        }


        /// <summary>
        /// A Refernece to get the rotational data
        /// </summary>
        public ApplicationViewModel AppVM
        {
            get;
        }



        ushort getYaw()
        {
            ushort ret = (ushort)((AppVM.Psi) / 360 * 65536);

            return ret;
        }
        ushort getPitch()
        {
            ushort ret = (ushort)((AppVM.Theta) / 180 * 65536);

            return ret;
        }
        ushort getRoll()
        {
            ushort ret = (ushort)((AppVM.Phi) / 360 * 65536);
            return ret;
        }
        


        /// <summary>
        /// Intervall in which data frame is broadcasted
        /// </summary>
        public int SendIntervall { get; set; } = 40; // ms


        /// <summary>
        /// Port of the IP Endpoint
        /// </summary>
        public int Port { get; set; } = 9200;

        /// <summary>
        /// IP of the IP Endpoint
        /// </summary>
        public String IP { get; set; } = "192.168.1.255";



        private void AppendLSB(byte[] target, int offset, byte[] source)
        {
            int writePos = offset + source.Length - 1;

            for(int i =0; i<source.Length; i++)
            {
                target[i+offset] = source[i];
                //writePos--;
            }
        }

        private ushort packetNR = 0;
        /// <summary>
        /// Sends Data Over UDP
        /// </summary>
        void SendPacket()
        {

            IPAddress serverAddr = IPAddress.Parse(this.IP);

            IPEndPoint endPoint = new IPEndPoint(serverAddr, this.Port);

            byte[] data = new byte[20]; // todo

            // HEAD 


            // increment id +1 every time 
            packetNR++;

            ushort id = 0x0100;

            byte[] head = new byte[4];
            var pNba = BitConverter.GetBytes(packetNR);
            AppendLSB(head, 0, pNba);

            var idba = BitConverter.GetBytes(id);
            AppendLSB(head, 2, idba);

            // DATA

            byte flag1 = 0b00000111;
            byte flag2 = 0;


            data[0] = flag1;
            data[1] = flag2;

            //byte[] shortasByte = BitConverter.GetBytes(degree);

            ushort yaw = getYaw();
            var yawba = BitConverter.GetBytes(yaw);

            AppendLSB(data, 2, yawba);

            ushort pitch = getPitch();
            var pitchba = BitConverter.GetBytes(pitch);
            AppendLSB(data, 4, pitchba);

            ushort roll = getRoll();
            var rollba = BitConverter.GetBytes(roll);
            AppendLSB(data, 6, rollba);


            // construct message frame

            var messageframe = new byte[24];

            Buffer.BlockCopy(head, 0, messageframe, 0, 4);
            Buffer.BlockCopy(data, 0, messageframe, 4, 20);


            try
            {
                //using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                //{
                //    s.SendTo(messageframe, endPoint);

                //}

                using (var cl = new UdpClient(9101))
                {
                    cl.Send(messageframe, messageframe.Length, endPoint);
                    
                }



            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

        }
    }
}
