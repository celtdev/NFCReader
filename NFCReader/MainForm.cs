using System;
using System.Drawing;
using System.IO.Ports;
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
            comList.Items.AddRange(SerialPort.GetPortNames());
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

            msgDetails.SelectionColor = eventArgs.Type == LogType.Trace
                ? Color.DarkCyan
                : Color.Blue;

            msgDetails.AppendText($"{msgDetails.Lines.Length + 1}: {eventArgs.Message}");
            msgDetails.SelectionColor = msgDetails.ForeColor;

            msgDetails.SelectionStart = msgDetails.TextLength;
            msgDetails.ScrollToCaret();
        }
    }
}
