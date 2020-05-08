using System;
using System.Security.Cryptography;

namespace CommonClasses
{
    [Serializable]
    public class DataServerToClient
    {
        public RSAParameters PublicKey;
        public EncriptionType EncriptionType;

        public byte[] Data;

        public DataServerToClient(RSAParameters publicKey, EncriptionType encriptionType, byte[] data)
        {
            PublicKey = publicKey;
            EncriptionType = encriptionType;
            Data = data;
        }
    }
}
