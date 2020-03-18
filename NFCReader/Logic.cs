using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PCSC;
using PCSC.Iso7816;

namespace NFCReader
{
    public class Logic
    {
        //private readonly PortProvider _portProvider;
        public event EventHandler<LogEventArgs> NewEventReceived;

//        public Logic()
//        {
//            _portProvider = new PortProvider();
//            _portProvider.NewEventReceived += (sender, data) => NewEventReceived?.Invoke(this, data);
//        }

        public void CheckReaders()
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
                        TraceEvent("Reader not found");
                        return;
                    }
                    TraceEvent(new string('=', 15));

                    using (var reader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                    {
                        var status = reader.GetStatus();

                        TraceEvent($"Reader names: {string.Join(", ", status.GetReaderNames())}");
                        TraceEvent($"Protocol: {status.Protocol}");
                        TraceEvent($"State: {status.State}");
                        TraceEvent($"ATR: {BitConverter.ToString(status.GetAtr() ?? new byte[0])}");
                    }

                    using (var isoReader = new IsoReader(context: context, readerName: readerName, mode: SCardShareMode.Shared, protocol: SCardProtocol.Any, releaseContextOnDispose: false))
                    {
                        SelectApplication(isoReader);
                        GetData(isoReader);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorEvent(e);
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

            TraceEvent($"Send SelectAID: {BitConverter.ToString(cmd.ToArray())}");
            var response = reader.Transmit(cmd);
            TraceEvent($"SW1 SW2 = {response.SW1:X2} {response.SW2:X2}{Environment.NewLine}");
        }

        private void GetData(IIsoReader reader)
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
            TraceEvent($"{new string('=', 15)}{Environment.NewLine}");
            TraceEvent($"Send GetData: {BitConverter.ToString(cmd.ToArray())}");
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

                TraceEvent($"Receved part {part} - SW1 SW2 = {response.SW1:X2} {response.SW2:X2} DataCount: {dataInfo}");
            }

            try
            {
                var rawData = Encoding.UTF8.GetString(buffer.ToArray());
                var jsonObject = JsonConvert.DeserializeObject(rawData);
                var formattedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                TraceEvent($"Data:{Environment.NewLine}{formattedData}");
            }
            catch (Exception e)
            {
                TraceEvent("Error occured while parse data: ");
                ErrorEvent(e);
                TraceEvent($"{Environment.NewLine}Recevied data: {BitConverter.ToString(buffer.ToArray())}");
            }
        }


        private void TraceEvent(string message)
        {
            NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Trace, $"{message}{Environment.NewLine}"));
        }

        private void ErrorEvent(Exception e)
        {
            NewEventReceived?.Invoke(this, new LogEventArgs(LogType.Error, $"{e}{Environment.NewLine}"));
        }
    }
}