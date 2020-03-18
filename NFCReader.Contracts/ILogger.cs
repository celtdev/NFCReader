using System;

namespace NFCReader.Contracts
{
    public interface ILogger
    {
        void Trace(string message);

        void Error(Exception exception, string message = null);

        void Data(string message);
    }
}
