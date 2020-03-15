using System;
using System.IO.Ports;
using System.Linq;

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
            try
            {
                var sp = (SerialPort)sender;
                var strData = sp.ReadLine();

                NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Data, strData));
            }
            catch (Exception exception)
            {
                ErrorEvent(exception);
            }
        }

        private void TraceEvent(string message)
        {
            NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Trace, $"{message}{Environment.NewLine}"));
        }

        private void ErrorEvent(Exception e)
        {
            NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Error, $"{e}{Environment.NewLine}"));
        }

        public void SendRawData(byte[] cmd)
        {
            try
            {
                TraceEvent($"Try to send message: [{string.Join(", ", cmd.Select(b => $"0x{b:X2}"))}]");
                _serialPort.Write(cmd, 0, cmd.Length);
                TraceEvent("Message is sent");
            }
            catch (Exception e)
            {
                ErrorEvent(e);
            }
        }
    }
}