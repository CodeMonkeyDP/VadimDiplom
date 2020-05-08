using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using CommonClasses;

namespace Server
{
    public class ServerCore
    {
        public bool IsStarted { get; private set; }

        private TcpListener _server;
        private Thread _mainThread;
        private List<ClientConnection> _connections;
        private Dispatcher _dispatcher;
        private ServerForm _form;
        private EncriptionType _encriptionType;

        public ServerCore(Dispatcher dispatcher, ServerForm form, EncriptionType encriptionType)
        {
            _dispatcher = dispatcher;
            _form = form;
            _encriptionType = encriptionType;
        }

        public void Start()
        {
            IsStarted = true;
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            _server.Start();
            _connections = new List<ClientConnection>();
            _mainThread = new Thread(Listen);
            _mainThread.IsBackground = true;
            _mainThread.Start();
        }

        private void Listen()
        {
            while (true)
            {
                try
                {
                    var client = _server.AcceptTcpClient();
                    var connection = new ClientConnection(client, _dispatcher, _encriptionType);

                    _connections.Add(connection);

                    _dispatcher.Invoke(() => { _form.AddRow(connection.Row.GetRow()); });
                }
                catch
                {
                }
            }
        }

        public void Close()
        {
            foreach (var connection in _connections)
                connection.Close();
            IsStarted = false;
            _server.Stop();
        }
    }
}
