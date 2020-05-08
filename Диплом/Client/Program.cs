using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using CommonClasses;

namespace Client
{
    public static class Program
    {
        private const int port = 8888;
        private const string server = "127.0.0.1";

        [STAThread]
        public static void Main()
        {
            var connection = new TcpConnection(server, port);
            Console.ReadKey();
        }
    }
}
