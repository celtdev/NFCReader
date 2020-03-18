using System;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using NFCReader.Contracts;
using NFCReader.Logic;

namespace NFCReader
{
    public partial class MainForm : Form, ILogger
    {
        private delegate void AppendTextToUI(LogArgs args);
        private readonly ILogic _logic;

        public MainForm()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(this).As<ILogger>();
            builder.RegisterType<Logic.Logic>().As<ILogic>();
            //builder.RegisterType<IsoProvider>().As<IDataProvider>();
            builder.RegisterType<FakeDataProvider>().As<IDataProvider>();
            builder.RegisterType<SaveToFileDataProcessor>().As<IDataProcessor>();

            var container = builder.Build();
            _logic = container.Resolve<ILogic>();
        }


        private void WriteLog(LogArgs args)
        {
            if (msgDetails.InvokeRequired)
            {
                var d = new AppendTextToUI(AppendText);
                msgDetails.Invoke(d, args);
            }
            else
            {
                AppendText(args);
            }
        }

        private void AppendText(LogArgs args)
        {
            msgDetails.SelectionStart = msgDetails.TextLength;
            msgDetails.SelectionLength = 0;

            switch (args.Type)
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

            msgDetails.AppendText($"{args.Timestamp}> {args.Message}{Environment.NewLine}");
            msgDetails.SelectionColor = msgDetails.ForeColor;

            msgDetails.SelectionStart = msgDetails.TextLength;
            msgDetails.ScrollToCaret();
        }

        public void Trace(string message)
        {
            WriteLog(new LogArgs(LogType.Trace, message));
        }

        public void Error(Exception exception, string message = null)
        {
            var msg = string.IsNullOrEmpty(message) ? exception.ToString() : $"{message}{exception}";
            WriteLog(new LogArgs(LogType.Error, msg));
        }

        public void Data(string message)
        {
            WriteLog(new LogArgs(LogType.Data, message));
        }


        private void checkReaders_Click(object sender, EventArgs e)
        {
            _logic.GetData();
        }
    }
}
