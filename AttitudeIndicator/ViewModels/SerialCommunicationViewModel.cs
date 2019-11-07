using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;

namespace AttitudeIndicator.ViewModels
{
    public class SerialCommunicationViewModel: BaseViewModel
    {

        public SerialCommunicationViewModel()
        {
           
        }

        private SerialPort Port { get; set; }

        public int Baud { get; set; } = 115200; // Standard for HC-05 Bluetooth controller


        public void Connect()
        {
            try
            {
                Port = new SerialPort(SelectedPortName, Baud);
                Port.Open();
                Port.DataReceived += DataHandler;
                
                
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }


        private void DataHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = sender as SerialPort;

            Console.WriteLine(sp.ReadExisting());
        }

        public void Disconnect()
        {
            Port?.Close();
            Port.Dispose();
        }


        public ObservableCollection<String> PortList
        {
            get
            {
                var list = SerialPort.GetPortNames();
                return new ObservableCollection<string>(list);
            }
        }

        public String SelectedPortName
        {
            get; set;
        }




        /// <summary>
        /// Storing 
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
