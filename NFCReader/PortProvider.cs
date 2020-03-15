using System;
using System.IO.Ports;

namespace NFCReader
{
    public class PortProvider
    {
        private SerialPort _serialPort;

        public event EventHandler<LogEventArgs> NewEventReceived;


        public void StartListening(string port)
        {
            TraceEvent("Try to start listening");
            if (_serialPort == null)
            {
                _serialPort = new SerialPort(port)
                {
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None
                };
                _serialPort.DataReceived += serialPort_DataReceived;
            }

            _serialPort.Open();
            TraceEvent("Serial port is opened");
        }

        public void StopListening()
        {
            _serialPort?.Close();
            TraceEvent("Serial port is closed");
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort)sender;
            var strData = sp.ReadLine();

            NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Data, strData));
        }

        private void TraceEvent(string message)
        {
            NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Trace, $"{message}{Environment.NewLine}"));
        }
    }
}