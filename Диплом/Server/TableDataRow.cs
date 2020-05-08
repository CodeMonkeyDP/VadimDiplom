using System.Windows.Forms;
using CommonClasses;

namespace Server
{
    public class TableDataRow
    {
        private DataGridViewRow _row;

        public DataGridViewRow GetRow()
        {
            return _row;
        }

        public EncriptionType EncriptionType
        {
            set
            {
                var cell = _row.Cells[0];
                cell.Value = value == EncriptionType.Rsa ? "RSA" : "XOR";
            }
        }

        public QueryClientType Status
        {
            set
            {
                var cell = _row.Cells[1];
                cell.Value = value == QueryClientType.Connect ? "Подлючено" : "Файл передан";
            }
        }

        public string FileName
        {
            set
            {
                var cell = _row.Cells[2];
                cell.Value = value;
            }
        }

        public TableDataRow(DataGridViewRow row)
        {
            _row = row;
            
            var type = new DataGridViewTextBoxCell();
            type.Value = "-";
            row.Cells.Add(type);
            type.ReadOnly = true;

            var status = new DataGridViewTextBoxCell();
            status.Value = "Подключено";
            row.Cells.Add(status);
            status.ReadOnly = true;

            var filename = new DataGridViewTextBoxCell();
            filename.Value = "-";
            row.Cells.Add(filename);
            filename.ReadOnly = true;
        }
    }
}
