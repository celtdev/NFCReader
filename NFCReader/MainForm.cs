using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace NFCReader
{
    public partial class MainForm : Form
    {
        private delegate void AppendTextToUI(LogEventArgs eventArgs);

        private readonly Logic _logic;

        public MainForm()
        {
            InitializeComponent();
            _logic = new Logic();
            _logic.NewEventReceived += logic_NewEventReceived;
        }

        private void logic_NewEventReceived(object sender, LogEventArgs e)
        {
            if (msgDetails.InvokeRequired)
            {
                var d = new AppendTextToUI(AppendText);
                msgDetails.Invoke(d, new[] { e });
            }
            else
            {
                AppendText(e);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            updateCOM_Click(sender, e);
        }

        private void updateCOM_Click(object sender, EventArgs e)
        {
            var newInfoList = SerialPort.GetPortNames();

            comList.Items.Clear();
            comList.Items.AddRange(newInfoList.Cast<object>().ToArray());
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comList.SelectedText))
            {
                return;
            }

            _logic.StartListening(comList.SelectedText);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            _logic.StopListening();
        }

        private void AppendText(LogEventArgs eventArgs)
        {
            msgDetails.SelectionStart = msgDetails.TextLength;
            msgDetails.SelectionLength = 0;

            switch (eventArgs.Type)
            {
                case LogType.Trace:
                    msgDetails.SelectionColor = Color.DarkCyan;
                    break;
                case LogType.Error:
                    msgDetails.SelectionColor = Color.Red;
                    break;
                case LogType.Data:
                    msgDetails.SelectionColor = Color.Blue;
                    break;
                default:
                    msgDetails.SelectionColor = msgDetails.ForeColor;
                    break;
            }

            msgDetails.AppendText($"> {eventArgs.Message}");
            msgDetails.SelectionColor = msgDetails.ForeColor;

            msgDetails.SelectionStart = msgDetails.TextLength;
            msgDetails.ScrollToCaret();
        }

        private void getReaderInfo_Click(object sender, EventArgs e)
        {
            _logic.GetReaderInfo();
        }

        private void getAID_Click(object sender, EventArgs e)
        {
            _logic.GetAID();
        }

        private void checkReaders_Click(object sender, EventArgs e)
        {
            _logic.CheckReaders();
        }
    }
}
