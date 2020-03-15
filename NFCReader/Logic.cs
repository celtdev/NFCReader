using System;

namespace NFCReader
{
    public class Logic
    {
        private readonly PortProvider _portProvider;
        public event EventHandler<LogEventArgs> NewEventReceived;

        public Logic()
        {
            _portProvider = new PortProvider();
            _portProvider.NewEventReceived += (sender, data) => NewEventReceived?.Invoke(this, data);
        }

        public void StartListening(string port)
        {
            _portProvider.StartListening(port);
        }

        public void StopListening()
        {
            _portProvider.StopListening();
        }

        public void GetReaderInfo()
        {
            var cmd = new byte[] {0xFF, 0x00, 0x48, 0x00, 0x00};
            _portProvider.SendRawData(cmd);
        }

        public void GetAID()
        {
            var cmd = new byte[] {0x00, 0xA4, 0x04, 0x00, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31, 0x00 };
            _portProvider.SendRawData(cmd);
        }
    }
}