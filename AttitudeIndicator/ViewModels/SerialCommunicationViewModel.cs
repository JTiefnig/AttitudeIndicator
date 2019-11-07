using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AttitudeIndicator.ViewModels
{
    public class SerialCommunicationViewModel: BaseViewModel
    {







        /// <summary>
        /// Storing 
        /// </summary>
        private bool _connected = false;
        public bool Connected
        {
            get => _connected;
            set
            {
                _connected = value;
                OnPropertyChanged(nameof(Connected));
            }
        }
    }
}
