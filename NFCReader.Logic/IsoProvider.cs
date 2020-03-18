using System;
using System.Collections.Generic;
using NFCReader.Contracts;
using PCSC;
using PCSC.Iso7816;

namespace NFCReader.Logic
{
    public class IsoProvider: IDataProvider
    {
        private readonly ILogger _logger;
        private SCardContext _context;

        public IsoProvider(ILogger logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            if (_context != null)
            {
                throw new ApplicationException("Previous context not disposed");
            }

            try
            {
                _context = new SCardContext();
                _context.Establish(SCardScope.System);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Can't initialize context: ");
            }
        }

        public void Stop()
        {
            _context.Dispose();
            _context = null;
        }

        public void Dispose()
        {
            Stop();
        }

        public string[] GetReaderNames()
        {
            if (_context == null)
            {
                throw new ApplicationException("Context not initialized");
            }

            return _context.GetReaders();
        }

        public void CheckReaderStatus(string readerName)
        {
            if (_context == null)
            {
                throw new ApplicationException("Context not initialized");
            }

            using (var reader = _context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
            {
                var status = reader.GetStatus();

                _logger.Trace($"Reader names: {string.Join(", ", status.GetReaderNames())}");
                _logger.Trace($"Protocol: {status.Protocol}");
                _logger.Trace($"State: {status.State}");
                _logger.Trace($"ATR: {BitConverter.ToString(status.GetAtr() ?? new byte[0])}");
            }
        }

        public byte[] GetData(string readerName)
        {
            if (_context == null)
            {
                throw new ApplicationException("Context not initialized");
            }

            using (var isoReader = new IsoReader(context: _context, readerName: readerName, mode: SCardShareMode.Shared, protocol: SCardProtocol.Any, releaseContextOnDispose: false))
            {
                SelectApplication(isoReader);
                return ReqeustData(isoReader);
            }
        }


        private void SelectApplication(IIsoReader reader)
        {
            var cmd = new CommandApdu(IsoCase.Case3Short, reader.ActiveProtocol)
            {
                CLA = 0x00,
                Instruction = InstructionCode.SelectFile,
                P1 = 0x04,
                P2 = 0x00,
                Data = new byte[] { 0xF0, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }
            };

            _logger.Trace($"Send SelectAID: {BitConverter.ToString(cmd.ToArray())}");
            var response = reader.Transmit(cmd);
            _logger.Trace($"SW1 SW2 = {response.SW1:X2} {response.SW2:X2}{Environment.NewLine}");
        }

        private byte[] ReqeustData(IIsoReader reader)
        {
            var cmd = new CommandApdu(IsoCase.Case2Short, reader.ActiveProtocol)
            {
                CLA = 0x00,
                Instruction = InstructionCode.GetData,
                P1 = 0x00,
                P2 = 0x00,
                Le = 0x00
            };

            var part = 0;
            var finish = false;
            _logger.Trace($"{new string('=', 15)}{Environment.NewLine}");
            _logger.Trace($"Send GetData: {BitConverter.ToString(cmd.ToArray())}");
            var buffer = new List<byte>();

            while (!finish)
            {
                if (part > short.MaxValue)
                {
                    break;
                }

                part++;
                var response = reader.Transmit(cmd);

                var dataInfo = "No Data";
                if (!response.HasData)
                {
                    finish = true;
                }
                else
                {
                    var data = response.GetData();
                    buffer.AddRange(data);
                    dataInfo = $"DataCount: {data.Length}";
                    if (data.Length < 256)
                    {
                        finish = true;
                    }
                }

                _logger.Trace($"Receved part {part} - SW1 SW2 = {response.SW1:X2} {response.SW2:X2} DataCount: {dataInfo}");
            }

            return buffer.ToArray();
        }
    }
}