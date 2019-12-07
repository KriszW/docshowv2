namespace ServerGUI
{
    partial class Server
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
            this.DGV_Kilokok = new System.Windows.Forms.DataGridView();
            this.TxtBox_asztalszama = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BTN_AllMachineRestart = new System.Windows.Forms.Button();
            this.LBL_LastUpdate = new System.Windows.Forms.Label();
            this.LBL_LastUpdateText = new System.Windows.Forms.Label();
            this.LBL_OrdFileInfos = new System.Windows.Forms.Label();
            this.Lbl_fileErrors = new System.Windows.Forms.Label();
            this.BTN_StartAll = new System.Windows.Forms.Button();
            this.BTN_ShutdownAll = new System.Windows.Forms.Button();
            this.BTN_updateLux = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Kilokok)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_Kilokok
            // 
            this.DGV_Kilokok.AllowUserToAddRows = false;
            this.DGV_Kilokok.AllowUserToDeleteRows = false;
            this.DGV_Kilokok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Kilokok.Location = new System.Drawing.Point(0, 0);
            this.DGV_Kilokok.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DGV_Kilokok.Name = "DGV_Kilokok";
            this.DGV_Kilokok.RowHeadersWidth = 62;
            this.DGV_Kilokok.RowTemplate.Height = 28;
            this.DGV_Kilokok.Size = new System.Drawing.Size(667, 325);
            this.DGV_Kilokok.TabIndex = 0;
            // 
            // TxtBox_asztalszama
            // 
            this.TxtBox_asztalszama.Location = new System.Drawing.Point(413, 448);
            this.TxtBox_asztalszama.MaxLength = 6;
            this.TxtBox_asztalszama.Name = "TxtBox_asztalszama";
            this.TxtBox_asztalszama.Size = new System.Drawing.Size(174, 20);
            this.TxtBox_asztalszama.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(410, 432);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Az asztalszáma (üres az összeshez):";
            // 
            // BTN_AllMachineRestart
            // 
            this.BTN_AllMachineRestart.Location = new System.Drawing.Point(413, 330);
            this.BTN_AllMachineRestart.Name = "BTN_AllMachineRestart";
            this.BTN_AllMachineRestart.Size = new System.Drawing.Size(254, 63);
            this.BTN_AllMachineRestart.TabIndex = 35;
            this.BTN_AllMachineRestart.Text = "Az összes kliens gép újraindítása";
            this.BTN_AllMachineRestart.UseVisualStyleBackColor = true;
            this.BTN_AllMachineRestart.Click += new System.EventHandler(this.BTN_AllMachineRestart_Click);
            // 
            // LBL_LastUpdate
            // 
            this.LBL_LastUpdate.AutoSize = true;
            this.LBL_LastUpdate.Location = new System.Drawing.Point(747, 31);
            this.LBL_LastUpdate.Name = "LBL_LastUpdate";
            this.LBL_LastUpdate.Size = new System.Drawing.Size(0, 13);
            this.LBL_LastUpdate.TabIndex = 34;
            // 
            // LBL_LastUpdateText
            // 
            this.LBL_LastUpdateText.AutoSize = true;
            this.LBL_LastUpdateText.Location = new System.Drawing.Point(669, 31);
            this.LBL_LastUpdateText.Name = "LBL_LastUpdateText";
            this.LBL_LastUpdateText.Size = new System.Drawing.Size(79, 13);
            this.LBL_LastUpdateText.TabIndex = 33;
            this.LBL_LastUpdateText.Text = "Utolsó frissítés:";
            // 
            // LBL_OrdFileInfos
            // 
            this.LBL_OrdFileInfos.AutoSize = true;
            this.LBL_OrdFileInfos.Location = new System.Drawing.Point(669, 1);
            this.LBL_OrdFileInfos.Name = "LBL_OrdFileInfos";
            this.LBL_OrdFileInfos.Size = new System.Drawing.Size(0, 13);
            this.LBL_OrdFileInfos.TabIndex = 32;
            // 
            // Lbl_fileErrors
            // 
            this.Lbl_fileErrors.AutoSize = true;
            this.Lbl_fileErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lbl_fileErrors.ForeColor = System.Drawing.Color.Red;
            this.Lbl_fileErrors.Location = new System.Drawing.Point(9, 523);
            this.Lbl_fileErrors.Name = "Lbl_fileErrors";
            this.Lbl_fileErrors.Size = new System.Drawing.Size(0, 17);
            this.Lbl_fileErrors.TabIndex = 31;
            // 
            // BTN_StartAll
            // 
            this.BTN_StartAll.Location = new System.Drawing.Point(0, 330);
            this.BTN_StartAll.Name = "BTN_StartAll";
            this.BTN_StartAll.Size = new System.Drawing.Size(254, 63);
            this.BTN_StartAll.TabIndex = 30;
            this.BTN_StartAll.Text = "Az összes kliens gép bekapcsolása";
            this.BTN_StartAll.UseVisualStyleBackColor = true;
            this.BTN_StartAll.Click += new System.EventHandler(this.BTN_StartAll_Click);
            // 
            // BTN_ShutdownAll
            // 
            this.BTN_ShutdownAll.Location = new System.Drawing.Point(0, 407);
            this.BTN_ShutdownAll.Name = "BTN_ShutdownAll";
            this.BTN_ShutdownAll.Size = new System.Drawing.Size(254, 63);
            this.BTN_ShutdownAll.TabIndex = 29;
            this.BTN_ShutdownAll.Text = "Az összes kliens gép kikapcsolása";
            this.BTN_ShutdownAll.UseVisualStyleBackColor = true;
            this.BTN_ShutdownAll.Click += new System.EventHandler(this.BTN_ShutdownAll_Click);
            // 
            // BTN_updateLux
            // 
            this.BTN_updateLux.Location = new System.Drawing.Point(671, 47);
            this.BTN_updateLux.Name = "BTN_updateLux";
            this.BTN_updateLux.Size = new System.Drawing.Size(305, 23);
            this.BTN_updateLux.TabIndex = 27;
            this.BTN_updateLux.Text = "LuxScan azonnali frissítés";
            this.BTN_updateLux.UseVisualStyleBackColor = true;
            this.BTN_updateLux.Click += new System.EventHandler(this.BTN_updateLux_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 614);
            this.Controls.Add(this.TxtBox_asztalszama);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTN_AllMachineRestart);
            this.Controls.Add(this.LBL_LastUpdate);
            this.Controls.Add(this.LBL_LastUpdateText);
            this.Controls.Add(this.LBL_OrdFileInfos);
            this.Controls.Add(this.Lbl_fileErrors);
            this.Controls.Add(this.BTN_StartAll);
            this.Controls.Add(this.BTN_ShutdownAll);
            this.Controls.Add(this.BTN_updateLux);
            this.Controls.Add(this.DGV_Kilokok);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Server";
            this.Text = "DocsShow Kezelő felület";
            this.Load += new System.EventHandler(this.Server_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Kilokok)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Kilokok;
        private System.Windows.Forms.TextBox TxtBox_asztalszama;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button BTN_AllMachineRestart;
        public System.Windows.Forms.Label LBL_LastUpdate;
        public System.Windows.Forms.Label LBL_LastUpdateText;
        public System.Windows.Forms.Label LBL_OrdFileInfos;
        public System.Windows.Forms.Label Lbl_fileErrors;
        public System.Windows.Forms.Button BTN_StartAll;
        public System.Windows.Forms.Button BTN_ShutdownAll;
        public System.Windows.Forms.Button BTN_updateLux;
    }
}

