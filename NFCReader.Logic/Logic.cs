using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NFCReader.Contracts;

namespace NFCReader.Logic
{
    public class Logic : ILogic
    {
        private readonly IDataProvider _dataProvider;
        private readonly IDataProcessor _dataProcessor;
        private readonly ILogger _logger;

        public Logic(IDataProvider dataProvider, IDataProcessor dataProcessor, ILogger logger)
        {
            _dataProvider = dataProvider;
            _dataProcessor = dataProcessor;
            _logger = logger;
        }

        public void GetData()
        {
            try
            {
                _logger.Trace("Start");
                _dataProvider.Start();

                var readerNames = _dataProvider.GetReaderNames();
                _logger.Trace($"Next readers found: {string.Join(", ", readerNames)}");

                var readerName = readerNames.FirstOrDefault();
                if (string.IsNullOrEmpty(readerName))
                {
                    _logger.Trace("Reader not found");
                    return;
                }

                _logger.Trace($"The first found reader will be used: {readerName}");

                var buffer = _dataProvider.GetData(readerName);
                _logger.Trace($"Received data count: {buffer.Length} bytes");

                _dataProcessor.ProcessData(buffer);
                _logger.Trace("Finish");
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            finally
            {
                _dataProvider.Stop();
            }
        }
    }
}