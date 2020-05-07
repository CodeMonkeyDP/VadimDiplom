using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using CommonClasses;

namespace Server
{
    public class ClientConnection
    {
        public Thread Thread;
        public TcpClient Client;
        public NetworkStream Stream;
        public BinaryWriter Writer;
        public BinaryReader Reader;
        public BinaryFormatter Formatter;
        public RSAParameters PrivateKey;
        public RSAParameters PublicKey;
        public RSACryptoServiceProvider RSA;
        public TableDataRow Row;
        public Dispatcher Dispatcher;
        public EncriptionType EncriptionType;

        public ClientConnection(TcpClient client, Dispatcher dispatcher, EncriptionType encriptionType)
        {
            Client = client;
            Stream = client.GetStream();
            Reader = new BinaryReader(new BufferedStream(Stream));
            Writer = new BinaryWriter(new BufferedStream(Stream));
            Formatter = new BinaryFormatter();
            RSA = new RSACryptoServiceProvider();
            Row = new TableDataRow(new DataGridViewRow());
            Dispatcher = dispatcher;
            EncriptionType = encriptionType;

            PrivateKey = RSA.ExportParameters(true);
            PublicKey = RSA.ExportParameters(false);

            StartThread();
        }

        public void Close()
        {
            Stream.Close();
            Client.Close();
        }

        public void WorkClient()
        {
            try
            {
                var connect = Read();

                if (EncriptionType == EncriptionType.Rsa)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Row.Status = QueryClientType.Connect;
                        Row.EncriptionType = EncriptionType.Rsa;
                    });

                    Write(new DataServerToClient(PublicKey, EncriptionType.Rsa, null));

                    var data = Read();

                    Dispatcher.Invoke(() =>
                    {
                        Row.Status = QueryClientType.DataSend1;
                        Row.EncriptionType = EncriptionType.Rsa;
                    });

                    var key = RSADecrypt(data.AesKey, PrivateKey, false);
                    var iv = data.AesSalt;

                    byte[] decryptedByes;

                    var aes = Aes.Create();
                    aes.Key = key;
                    aes.IV = iv;

                    ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream ms = new MemoryStream(data.FileData))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                        {
                            using (var sr = new BinaryReader(cs))
                            {
                                decryptedByes = sr.ReadBytes(data.BytesCount);
                            }
                        }
                    }

                    File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Files", data.Filename),
                        decryptedByes);

                    Dispatcher.Invoke(() => { Row.FileName = data.Filename; });
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        Row.Status = QueryClientType.Connect;
                        Row.EncriptionType = EncriptionType.Xor;
                    });

                    Write(new DataServerToClient(PublicKey, EncriptionType.Xor, null));

                    connect = Read();

                    var bytesCount = connect.BytesCount;

                    var xorBytes = GetBytesForXor(bytesCount);

                    var xorArray = XorByteArrays(connect.FileData, xorBytes);

                    Write(new DataServerToClient(PublicKey, EncriptionType.Xor, xorArray));

                    var next = Read();

                    Dispatcher.Invoke(() =>
                    {
                        Row.Status = QueryClientType.DataSend1;
                        Row.EncriptionType = EncriptionType.Xor;
                    });

                    var file = XorByteArrays(next.FileData, xorBytes);
                    var filename = next.Filename;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", filename);

                    File.WriteAllBytes(path, file);

                    Dispatcher.Invoke(() => { Row.FileName = filename; });
                }
            }
            catch
            {
                Row.FileName = "Ошибка";
            }
        }

        public byte[] XorByteArrays(byte[] array, byte[] xorArray)
        {
            var result = new byte[array.Length];

            for (int i = 0; i < array.Length; i++)
                result[i] = (byte)(array[i] ^ xorArray[i]);

            return result;
        }

        public byte[] GetBytesForXor(int bytesCount)
        {
            var result = new byte[bytesCount];
            var rnd = new Random();

            rnd.NextBytes(result);

            return result;
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            RSA.ImportParameters(RSAKeyInfo);

            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        public void Write(DataServerToClient data)
        {
            var memory = new MemoryStream();
            Formatter.Serialize(memory, data);

            var bytes = memory.ToArray();

            Writer.Write(bytes.Length);
            Writer.Write(bytes);
            Writer.Flush();
        }

        public DataClientToServer Read()
        {
            var bytesCount = Reader.ReadInt32();

            var bytes = Reader.ReadBytes(bytesCount);

            var obj = (DataClientToServer)Formatter.Deserialize(new MemoryStream(bytes));

            return obj;
        }

        public void StartThread()
        {
            Thread = new Thread(WorkClient);
            Thread.IsBackground = true;
            Thread.Start();
        }
    }
}
