using System.IO.Ports;

namespace NFCReader
{
    public class SerialPortConfiguration
    {
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public int DataBits { get; set; }
        public Handshake Handshake { get; set; }

        public SerialPortConfiguration()
        {
            BaudRate = 9600;
            Parity = Parity.None;
            StopBits = StopBits.One;
            DataBits = 8;
            Handshake = Handshake.None;
        }
    }
}