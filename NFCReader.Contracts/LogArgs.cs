using System;

namespace NFCReader.Contracts
{
    public class LogArgs
    {
        public LogType Type { get; }

        public DateTime Timestamp { get; }

        public string Message { get; }

        public LogArgs(LogType type, string message)
        {
            Timestamp = DateTime.Now;
            Type = type;
            Message = message;
        }
    }
}