using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PCSC;
using PCSC.Utils;
using PCSC.Iso7816;

namespace NFCReader
{
    public class Logic
    {
        private readonly PortProvider _portProvider;
        public event EventHandler<LogEventArgs> NewEventReceived;

        public Logic()
        {
            _portProvider = new PortProvider();
            _portProvider.NewEventReceived += (sender, data) => NewEventReceived?.Invoke(this, data);
        }

        public void StartListening(string port)
        {
            _portProvider.StartListening(port);
        }

        public void StopListening()
        {
            _portProvider.StopListening();
        }

        public void GetReaderInfo()
        {
            var cmd = new byte[] { 0xFF, 0x00, 0x48, 0x00, 0x00 };
            _portProvider.SendRawData(cmd);
        }

        public void GetAID()
        {
            var cmd = new byte[] { 0x00, 0xA4, 0x04, 0x00, 0x0E, 0x32, 0x50, 0x41, 0x59, 0x2E, 0x53, 0x59, 0x53, 0x2E, 0x44, 0x44, 0x46, 0x30, 0x31, 0x00 };
            _portProvider.SendRawData(cmd);
        }

        public void CheckReaders(string prmSelectorSelectedText)
        {
            if (!string.IsNullOrEmpty(prmSelectorSelectedText))
            {
                var strP1 = prmSelectorSelectedText.Substring(2, 2);
                var strP2 = prmSelectorSelectedText.Substring(4, 2);
                var p1 = byte.Parse(strP1, NumberStyles.HexNumber);
                var p2 = byte.Parse(strP2, NumberStyles.HexNumber);
            }

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
                        GetData(isoReader, prmSelectorSelectedText);
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

        private void GetData(IIsoReader reader, string prmSelectorSelectedText)
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
                TraceEvent("Try to parse data: ");
                ErrorEvent(e);
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