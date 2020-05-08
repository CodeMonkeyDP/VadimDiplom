using System;

namespace CommonClasses
{
    [Serializable]
    public class DataClientToServer
    {
        public QueryClientType QueryType;

        public byte[] AesKey;
        public byte[] AesSalt;

        public string Filename;

        public byte[] FileData;

        public int BytesCount;

        public DataClientToServer(QueryClientType queryType, byte[] aesKey,
            byte[] aesSalt, string filename, byte[] fileData, int bytesCount)
        {
            QueryType = queryType;
            AesKey = aesKey;
            AesSalt = aesSalt;
            Filename = filename;
            FileData = fileData;
            BytesCount = bytesCount;
        }
    }
}
