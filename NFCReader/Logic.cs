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
    }
}