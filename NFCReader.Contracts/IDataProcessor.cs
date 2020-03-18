namespace NFCReader.Contracts
{
    public interface IDataProcessor
    {
        void ProcessData(byte[] data);
    }
}