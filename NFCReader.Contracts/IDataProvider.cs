using System;

namespace NFCReader.Contracts
{
    public interface IDataProvider: IDisposable
    {
        void Start();

        void Stop();

        byte[] GetData(string readerName);

        string[] GetReaderNames();

        void CheckReaderStatus(string readerName);
    }
}