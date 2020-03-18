using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NFCReader.Contracts;
using PCSC;
using PCSC.Iso7816;

namespace NFCReader.Logic
{
    public class Logic: ILogic
    {
        private readonly ILogger _logger;

        public Logic(ILogger logger)
        {
            _logger = logger;
        }

        public void GetData()
        {
            try
            {
                using (var context = new SCardContext())
                {
                    context.Establish(SCardScope.System);
                    string[] readerNames = context.GetReaders();
                    string readerName = readerNames.FirstOrDefault();

                    if (string.IsNullOrEmpty(readerName))
                    {
                        _logger.Trace("Reader not found");
                        return;
                    }
                    _logger.Trace(new string('=', 15));

                    using (var reader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                    {
                        var status = reader.GetStatus();

                        _logger.Trace($"Reader names: {string.Join(", ", status.GetReaderNames())}");
                        _logger.Trace($"Protocol: {status.Protocol}");
                        _logger.Trace($"State: {status.State}");
                        _logger.Trace($"ATR: {BitConverter.ToString(status.GetAtr() ?? new byte[0])}");
                    }

                    using (var isoReader = new IsoReader(context: context, readerName: readerName, mode: SCardShareMode.Shared, protocol: SCardProtocol.Any, releaseContextOnDispose: false))
                    {
                        SelectApplication(isoReader);
                        ReqeustData(isoReader);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
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

        private void ReqeustData(IIsoReader reader)
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
                if (part > byte.MaxValue)
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

            try
            {
                var rawData = Encoding.UTF8.GetString(buffer.ToArray());
                var jsonObject = JsonConvert.DeserializeObject(rawData);
                var formattedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                _logger.Trace($"Data:{Environment.NewLine}{formattedData}");
            }
            catch (Exception e)
            {
                _logger.Trace("Error occured while parse data: ");
                _logger.Error(e);
                _logger.Trace($"{Environment.NewLine}Recevied data: {BitConverter.ToString(buffer.ToArray())}");
            }
        }
    }
}