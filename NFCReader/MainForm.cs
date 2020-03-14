using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NFCReader
{
    public partial class MainForm : Form
    {
        private delegate void AppendTextToUI(string text);
        private SerialPort _serialPort;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            comList.Items.AddRange(SerialPort.GetPortNames());
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comList.SelectedText))
            {
                return;
            }

            if (_serialPort == null)
            {
                _serialPort = new SerialPort(comList.SelectedText)
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
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            _serialPort?.Close();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort) sender;

            var srtData = sp.ReadLine();
            if (msgDetails.InvokeRequired)
            {
                var d = new AppendTextToUI(AppendText);
                msgDetails.Invoke(d, new [] { srtData });
            }
            else
            {
                AppendText(srtData);
            }
        }

        private void AppendText(string text)
        {
            msgDetails.AppendText($"{msgDetails.Lines.Length + 1}: {text}");
            msgDetails.SelectionStart = msgDetails.TextLength;
            msgDetails.ScrollToCaret();
        }
    }
}
