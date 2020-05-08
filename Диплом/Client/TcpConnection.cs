using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Windows.Forms;
using CommonClasses;

namespace Client
{
    public class TcpConnection
    {
        public TcpClient Client;
        public BinaryWriter Writer;
        public BinaryReader Reader;
        public BinaryFormatter Formatter;
        public NetworkStream Stream;

        public TcpConnection(string server, int port)
        {
            try
            {
                Console.WriteLine("Пробуем подключится к серверу");
                Client = new TcpClient(server, port);
                Console.WriteLine("Подключение создано успешно");
            }
            catch
            {
                Console.WriteLine("Ошибка при подключении");
                return;
            }

            Stream = Client.GetStream();
            Writer = new BinaryWriter(new BufferedStream(Stream));
            Reader = new BinaryReader(new BufferedStream(Stream));
            Formatter = new BinaryFormatter();

            WorkClient();
        }

        public void WorkClient()
        {
            //Console.WriteLine("Выберите тип шифрования: ");
            //Console.WriteLine("1 - RSA");
            //Console.WriteLine("2 - XOR");

            //var encType = Console.ReadLine().Trim();

            var f = "";
            while (true)
            {
                Console.WriteLine("Выберите файл для отправки...");

                var ofd = new OpenFileDialog();
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    f = ofd.FileName;
                    break;
                }
                else
                    Console.WriteLine("Вы не выбрали файл");
            }

            var bytes = File.ReadAllBytes(f);
            var fileName = Path.GetFileName(f);

            Write(new DataClientToServer(QueryClientType.Connect, null, null, null, null, bytes.Length));

            var connect = Read();

            if (connect.EncriptionType == EncriptionType.Rsa)
            {
                //Write(new DataClientToServer(QueryClientType.Connect, null, null, null, null,
                //    bytes.Length));
                //var data = Read();

                var data = connect;

                var publicKey = data.PublicKey;

                var aes = Aes.Create();
                aes.GenerateIV();
                aes.GenerateKey();
                var iv = aes.IV;
                var key = aes.Key;

                ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                    {
                        using (var sw = new BinaryWriter(cs))
                        {
                            sw.Write(bytes, 0, bytes.Length);
                        }
                    }

                    encBytes = ms.ToArray();
                }

                Write(new DataClientToServer(QueryClientType.DataSend1,
                    RSAEncrypt(key, publicKey, false), iv, fileName, encBytes,
                    encBytes.Length));

                //var encBytes = RSAEncrypt(bytes, publicKey, false);

                //Write(new DataClientToServer(QueryClientType.DataSend1, EncriptionType.Rsa, fileName, encBytes, encBytes.Length));

                Console.WriteLine("Файл отправлен");

                Stream.Close();
                Client.Close();
            }
            else
            {
                var xorArray = GetBytesForXor(bytes.Length);

                var xorBytes = XorByteArrays(bytes, xorArray);

                Write(new DataClientToServer(QueryClientType.Connect, null, null, null, xorBytes,
                    xorBytes.Length));

                var next = Read();

                Write(new DataClientToServer(QueryClientType.DataSend1, null, null, fileName,
                    XorByteArrays(next.Data, xorArray), next.Data.Length));

                Console.WriteLine("Файл отправлен");

                Stream.Close();
                Client.Close();
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

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            RSA.ImportParameters(RSAKeyInfo);

            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        public void Write(DataClientToServer data)
        {
            var memory = new MemoryStream();
            Formatter.Serialize(memory, data);

            var bytes = memory.ToArray();

            Writer.Write(bytes.Length);
            Writer.Write(bytes);
            Writer.Flush();
        }

        public DataServerToClient Read()
        {
            var bytesCount = Reader.ReadInt32();

            var bytes = Reader.ReadBytes(bytesCount);

            var obj = (DataServerToClient)Formatter.Deserialize(new MemoryStream(bytes));

            return obj;
        }
    }
}
