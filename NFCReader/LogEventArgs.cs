using System;

namespace NFCReader
{
    public class LogEventArgs: EventArgs
    {
        public LogDirection Direction { get; }

        public LogType Type { get; }

        public DateTime Timestamp { get; }

        public string Message { get; }

        public LogEventArgs(LogType type, string message, LogDirection direction = LogDirection.Request)
        {
            Timestamp = DateTime.Now;
            Type = type;
            Message = message;
            Direction = direction;
        }
    }
}