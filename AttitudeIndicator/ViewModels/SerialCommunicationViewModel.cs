using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using System.Threading;

namespace AttitudeIndicator.ViewModels
{
    public class SerialCommunicationViewModel: BaseViewModel
    {

        public SerialCommunicationViewModel(SerialMessageProcessor prcessor)
        {
            MessageProcessor = prcessor;

            // for debugging
            _selectedPortName = PortList.FirstOrDefault();
        }


        public SerialMessageProcessor MessageProcessor
        {
            get; 
        }

        private SerialPort Port { get; set; }

        public int Baud { get; set; } = 115200; // Standard for HC-05 Bluetooth controller


        private void Connect()
        {
            try
            {
                Port = new SerialPort(SelectedPortName, Baud);
                Port.Open();
                Port.DiscardInBuffer();

                Port.DataReceived += DataHandler;

                Port.ReadTimeout = 500;

                Port.PinChanged += SerialEvent;

                Connected = true;
            }
            catch(Exception e)
            {
                Connected = false;
                MessageBox.Show(e.Message);
            }
            
        }

        void SerialEvent(object sender, SerialPinChangedEventArgs e)
        {
            if(e.EventType == SerialPinChange.Break)
            {
                this.Connected = false;
            }
        }

        private void DataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = sender as SerialPort;

            try
            {
                while (sp.BytesToRead > 0)
                {
                    Thread.Sleep(2);
                    var head = new byte[3];
                    sp.Read(head, 0, 3);

                    if (head[0] != '>') continue;
                    if (head[1] != 0 ) continue;

                    //debug
                    //Console.WriteLine(String.Format("HEAD: {0} {1} {2}", head[0], head[1], head[2]));

                    int dlc = head[2]; // DLC
                    if (dlc != 9)
                        continue;
                    byte[] inp = new byte[dlc];
                    sp.Read(inp, 0, dlc);
                    MessageProcessor.ProcessMessage(inp);

                }
            }catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void ReConnect()
        {
            Disconnect();
            Connect();
        }


        private void Disconnect()
        {
            try
            {
                Port?.Close();
                Port?.Dispose();
                Connected = false;
            }
            catch { }
        }


        public ObservableCollection<String> PortList => 
            new ObservableCollection<string>(SerialPort.GetPortNames());


        private String _selectedPortName;
        public String SelectedPortName
        {
            get=> _selectedPortName;
            set
            {
                if (value == _selectedPortName)
                    return;
                _selectedPortName = value;

                if(Connected)
                    ReConnect();
            }
        }

        /// <summary>
        /// The State of the connection 
        /// </summary>
        private bool _connected = false;
        public bool Connected
        {
            get => _connected;
            set
            {
                if (_connected == value)
                    return;

                _connected = value;

                OnPropertyChanged(nameof(Connected));
                if (_connected)
                    Connect();
                else
                    Disconnect();

            }
        }

        
    }

}
