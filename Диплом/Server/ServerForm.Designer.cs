namespace Server
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Table = new System.Windows.Forms.DataGridView();
            this.StartServer = new System.Windows.Forms.Button();
            this.OpenFolder = new System.Windows.Forms.Button();
            this.EncType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Table)).BeginInit();
            this.SuspendLayout();
            // 
            // Table
            // 
            this.Table.AllowUserToAddRows = false;
            this.Table.AllowUserToDeleteRows = false;
            this.Table.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Table.Location = new System.Drawing.Point(12, 119);
            this.Table.Name = "Table";
            this.Table.ReadOnly = true;
            this.Table.Size = new System.Drawing.Size(785, 600);
            this.Table.TabIndex = 0;
            // 
            // StartServer
            // 
            this.StartServer.BackColor = System.Drawing.Color.Gray;
            this.StartServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartServer.ForeColor = System.Drawing.Color.White;
            this.StartServer.Location = new System.Drawing.Point(12, 12);
            this.StartServer.Name = "StartServer";
            this.StartServer.Size = new System.Drawing.Size(398, 52);
            this.StartServer.TabIndex = 1;
            this.StartServer.Text = "Запустить сервер";
            this.StartServer.UseVisualStyleBackColor = false;
            this.StartServer.Click += new System.EventHandler(this.StartServer_Click);
            // 
            // OpenFolder
            // 
            this.OpenFolder.BackColor = System.Drawing.Color.Gray;
            this.OpenFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OpenFolder.ForeColor = System.Drawing.Color.White;
            this.OpenFolder.Location = new System.Drawing.Point(416, 12);
            this.OpenFolder.Name = "OpenFolder";
            this.OpenFolder.Size = new System.Drawing.Size(379, 52);
            this.OpenFolder.TabIndex = 2;
            this.OpenFolder.Text = "Открыть папку с принятыми файлами";
            this.OpenFolder.UseVisualStyleBackColor = false;
            this.OpenFolder.Click += new System.EventHandler(this.OpenFolder_Click);
            // 
            // EncType
            // 
            this.EncType.FormattingEnabled = true;
            this.EncType.Items.AddRange(new object[] {
            "RSA",
            "XOR"});
            this.EncType.Location = new System.Drawing.Point(123, 70);
            this.EncType.Name = "EncType";
            this.EncType.Size = new System.Drawing.Size(121, 21);
            this.EncType.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(21, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Тип шифрования:";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(807, 731);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EncType);
            this.Controls.Add(this.OpenFolder);
            this.Controls.Add(this.StartServer);
            this.Controls.Add(this.Table);
            this.Name = "ServerForm";
            this.Text = "Сервер";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Table)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Table;
        private System.Windows.Forms.Button StartServer;
        private System.Windows.Forms.Button OpenFolder;
        private System.Windows.Forms.ComboBox EncType;
        private System.Windows.Forms.Label label1;
    }
}