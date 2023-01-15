using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace DeviceControl
{
    class DeviceViewModel
    {
        private Device _device;
        private float _setPoint;
        private float _currentTemperature;
        private bool _isAgitating;
        private bool _isHeating;

        public ICommand StartAgitationCommand { get; set; }
        public ICommand StopAgitationCommand { get; set; }
        public ICommand StartHeatingCommand { get; set; }
        public ICommand StopHeatingCommand { get; set; }
        public ICommand SetTemperatureSetPointCommand { get; set; }

        public DeviceViewModel(Device device)
        {
            _device = device;
            StartAgitationCommand = new RelayCommand(StartAgitation);
            StopAgitationCommand = new RelayCommand(StopAgitation);
            StartHeatingCommand = new RelayCommand(StartHeating);
            StopHeatingCommand = new RelayCommand(StopHeating);
            SetTemperatureSetPointCommand = new RelayCommand(SetTemperatureSetPoint);
            _setPoint = _device.GetTemperatureSetPoint();
        }

        public void ClosePort()
        {
            _device.ClosePort();
        }

        public float SetPoint
        {
            get { return _setPoint; }
            set
            {
                _setPoint = value;
                OnPropertyChanged("SetPoint");
            }
        }

        public float CurrentTemperature
        {
            get { return _currentTemperature; }
            set
            {
                _currentTemperature = value;
                OnPropertyChanged("CurrentTemperature");
            }
        }

        public bool IsAgitating
        {
            get { return _isAgitating; }
            set
            {
                _isAgitating = value;
                OnPropertyChanged("IsAgitating");
            }
        }

        public bool IsHeating
        {
            get { return _isHeating; }
            set
            {
                _isHeating = value;
                OnPropertyChanged("IsHeating");
            }
        }

        private void StartAgitation()
        {
            _device.StartAgitation();
            IsAgitating = true;
        }

        private void StopAgitation()
        {
            _device.StopAgitation();
            IsAgitating = false;
        }

        private void StartHeating()
        {
            _device.StartHeating();
            IsHeating = true;
        }

        private void StopHeating()
        {
            _device.StopHeating();
            IsHeating = false;
        }

        private void SetTemperatureSetPoint()
        {
            _device.SetTemperatureSetPoint(_setPoint);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
