using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceControl
{
    class DeviceSerialPort
    {
        private SerialPort _serialPort;
        public DeviceSerialPort(string portName)
        {
            _serialPort = new SerialPort(portName);
            _serialPort.Open();
        }
        public DeviceSerialPort(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
        }
        public void Open()
        {
            _serialPort.Open();
        }

        public Boolean IsOpen
        {
            get { return _serialPort.IsOpen; }
        }
        public void Close()
        {
            _serialPort.Close();
        }
        public void SendCommand(string command)
        {
            _serialPort.Write(command);
        }
    }
}
