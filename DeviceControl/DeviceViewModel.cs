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
    class DeviceViewModel : INotifyPropertyChanged
    {
        private Device _device;
        private float _setPoint;
        private float _currentTemperature;
        private bool _isAgitationON;
        private bool _isHeatingON;

        private StateMachine _stateMachine;

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

            _stateMachine = new StateMachine();
            _stateMachine.StateChanged+= OnStateChanged;
        }

        public void Connect()
        {
            _device.OpenPort();
        }

        public void Disconnect()
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

        public bool IsAgitationON
        {

            get { return _stateMachine.CurrentState == StateMachine.States.Agitating || _stateMachine.CurrentState == StateMachine.States.AgitatingAndHeating; }
            //get { return _isAgitationON; }
            set
            {
                _isAgitationON = value;
                OnPropertyChanged("IsAgitationON");
            }
        }

        public bool IsHeatingON
        {
            get { return _stateMachine.CurrentState == StateMachine.States.Heating || _stateMachine.CurrentState == StateMachine.States.AgitatingAndHeating; }
            //get { return _isHeatingON; }
            set
            {
                _isHeatingON = value;
                OnPropertyChanged("IsHeatingON");
            }
        }

        private void StartAgitation()
        {
            _device.StartAgitation();
            //_stateMachine.ChangeState(StateMachine.States.Agitating);
            if (_stateMachine.CurrentState == StateMachine.States.Heating)
            {
                _stateMachine.ChangeState(StateMachine.States.AgitatingAndHeating);
            }
            else
            {
                _stateMachine.ChangeState(StateMachine.States.Agitating);
            }

            IsAgitationON = true;
        }

        private void StopAgitation()
        {
            _device.StopAgitation();
            if (IsHeatingON) 
            { 
                _stateMachine.ChangeState(StateMachine.States.Heating); 
            }
            else 
            { 
                _stateMachine.ChangeState(StateMachine.States.Stopped); 
            }

            IsAgitationON = false;
        }

        private void StartHeating()
        {
            _device.StartHeating();
            //_stateMachine.ChangeState(StateMachine.States.Heating);
            if (_stateMachine.CurrentState == StateMachine.States.Agitating)
            {
                _stateMachine.ChangeState(StateMachine.States.AgitatingAndHeating);
            }
            else
            {
                _stateMachine.ChangeState(StateMachine.States.Heating);
            }

            IsHeatingON = true;
        }

        private void StopHeating()
        {
            _device.StopHeating();
            if (IsAgitationON)
            {
                _stateMachine.ChangeState(StateMachine.States.Agitating);
            }
            else
            {
                _stateMachine.ChangeState(StateMachine.States.Stopped);
            }

            IsHeatingON = false;
        }

        private void SetTemperatureSetPoint()
        {
            _device.SetTemperatureSetPoint(_setPoint);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateLeds()
        {
            switch (_stateMachine.CurrentState)
            {
                case StateMachine.States.Agitating:
                    IsAgitationON = true;
                    IsHeatingON = false;
                    //IsRegulatingON = false;
                    break;
                case StateMachine.States.Heating:
                    IsAgitationON = false;
                    IsHeatingON = true;
                    //IsRegulatingON = false;
                    break;
                case StateMachine.States.AgitatingAndHeating:
                    IsAgitationON = true;
                    IsHeatingON = true;
                    //IsRegulatingON = false;
                    break;
                case StateMachine.States.Regulating:
                    IsAgitationON = false;
                    IsHeatingON = false;
                    //IsRegulatingON = true;
                    break;
                case StateMachine.States.Stopped:
                    IsAgitationON = false;
                    IsHeatingON = false;
                    //IsRegulating = false;
                    break;
            }
        }
        private void OnStateChanged(object sender, EventArgs e)
        {
            UpdateLeds();
        }

        public void UpdateDeviceState()
        {
            //string deviceState = _device.GetDeviceState();
            string deviceState = "Stopped";
            if (deviceState == "Agitating")
            {
                _stateMachine.ChangeState(StateMachine.States.Agitating);
            }
            else if (deviceState == "Heating")
            {
                _stateMachine.ChangeState(StateMachine.States.Heating);
            }
            //...
        }
    }


}
