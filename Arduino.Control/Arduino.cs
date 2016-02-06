using System.IO.Ports;

namespace ArduinoControl
{
    public class Arduino
    {
        private readonly SerialPort _arduino;

        public Arduino(string portName, int baudRate)
        {
            _arduino = new SerialPort(portName, baudRate);
        }

        public void Connect()
        {
            _arduino.Open();
        }

        public void Close()
        {
            _arduino.Close();
        }

        public void Write(byte[] buffer)
        {
            _arduino.Write(buffer, 0, buffer.Length);
        }
    }
}
