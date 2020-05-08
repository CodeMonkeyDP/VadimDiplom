using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;
using CommonClasses;

namespace Server
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();

            Table.Columns.Add("EncType", "Шифрование");
            Table.Columns.Add("Status", "Статус");
            Table.Columns.Add("File", "Имя файла");

            
        }

        public ServerCore Server;
        private bool _isStarted = false;

        private void StartServer_Click(object sender, EventArgs e)
        {
            if (_isStarted)
            {
                Server.Close();
                _isStarted = false;
                EncType.Enabled = true;
                StartServer.Text = "Запустить сервер";
            }
            else
            {
                _isStarted = true;
                Server = new ServerCore(Dispatcher.CurrentDispatcher, this, EncType.SelectedIndex == 0 ? EncriptionType.Rsa : EncriptionType.Xor);
                StartServer.Text = "Остановить сервер";
                EncType.Enabled = false;
                Server.Start();
            }
        }

        public void AddRow(DataGridViewRow row)
        {
            Table.Rows.Add(row);
        }

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            Process.Start("explorer.exe", path);
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
