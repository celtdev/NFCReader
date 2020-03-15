using System;

namespace NFCReader
{
    public class LogEventArgs: EventArgs
    {
        public LogType Type { get; }

        public string Message { get; }

        public LogEventArgs(LogType type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}