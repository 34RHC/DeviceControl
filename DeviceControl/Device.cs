using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceControl
{
    class Device
    {
        private DeviceSerialPort _serialPort;
        private bool _isAgitating;
        private bool _isHeating;
        private float _setPoint;
        private float _currentTemperature;

        public Device(DeviceSerialPort serialPort)
        {
            _serialPort = serialPort;
            _isAgitating = false;
            _isHeating = false;
            OpenPort();
        }

        public void OpenPort()
        {
            // Abrir el puerto serie y configurarlo
            _serialPort.Open();
        }
        public void ClosePort()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
        public void StartAgitation()
        {
            _isAgitating = true;
            _serialPort.SendCommand("Agit_ON");
        }

        public void StopAgitation()
        {
            _isAgitating = false;
            _serialPort.SendCommand("Agit_OFF");
        }

        public void StartHeating()
        {
            _isHeating = true;
            _serialPort.SendCommand("Temp_ON");
        }

        public void StopHeating()
        {
            _isHeating = false;
            _serialPort.SendCommand("Temp_OFF");
        }

        public void SetTemperatureSetPoint(float temperature)
        {
            _setPoint = temperature;
            _serialPort.SendCommand("SP_" + temperature);
        }

        public float GetCurrentTemperature()
        {
            return _currentTemperature;
        }

        public bool IsAgitating()
        {
            return _isAgitating;
        }

        public bool IsHeating()
        {
            return _isHeating;
        }

        public float GetTemperatureSetPoint()
        {
            return _setPoint;
        }
    }

}
