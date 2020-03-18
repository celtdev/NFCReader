using System;
using System.IO;
using System.Windows.Forms;
using NFCReader.Contracts;

namespace NFCReader.Logic
{
    public class SaveToFileDataProcessor: IDataProcessor
    {
        private readonly ILogger _logger;

        public SaveToFileDataProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void ProcessData(byte[] data)
        {
            var directory = Directory.GetCurrentDirectory();
            var fileName = $"received_data-{DateTime.Now:yyyy_MM_dd-HH_mm_ss}.bin";
            var sfd = new SaveFileDialog
            {
                InitialDirectory = directory,
                FileName = fileName
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, data);
                _logger.Trace($"Data saved to file '{sfd.FileName}'");
            }
        }
    }
}