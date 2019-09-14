namespace DocsShowServer
{
    public partial class Tables
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
            this.components = new System.ComponentModel.Container();
            this.Lbl_fileErrors = new System.Windows.Forms.Label();
            this.Timer_PcCheck = new System.Windows.Forms.Timer(this.components);
            this.btn_updateLux = new System.Windows.Forms.Button();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.BTN_ShutdownAll = new System.Windows.Forms.Button();
            this.btn_StartAll = new System.Windows.Forms.Button();
            this.LBL_OrdFileInfos = new System.Windows.Forms.Label();
            this.LBL_LastUpdateText = new System.Windows.Forms.Label();
            this.LBL_LastUpdate = new System.Windows.Forms.Label();
            this.BTN_AllMachineRestart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtBox_asztalszama = new System.Windows.Forms.TextBox();
            this.Col_Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_monSzam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_dbSzam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_standJobb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_standBal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Cikk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_asztal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgw_Asztalok = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgw_Asztalok)).BeginInit();
            this.SuspendLayout();
            // 
            // Lbl_fileErrors
            // 
            this.Lbl_fileErrors.AutoSize = true;
            this.Lbl_fileErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Lbl_fileErrors.ForeColor = System.Drawing.Color.Red;
            this.Lbl_fileErrors.Location = new System.Drawing.Point(6, 572);
            this.Lbl_fileErrors.Name = "Lbl_fileErrors";
            this.Lbl_fileErrors.Size = new System.Drawing.Size(0, 17);
            this.Lbl_fileErrors.TabIndex = 20;
            // 
            // btn_updateLux
            // 
            this.btn_updateLux.Location = new System.Drawing.Point(663, 57);
            this.btn_updateLux.Name = "btn_updateLux";
            this.btn_updateLux.Size = new System.Drawing.Size(254, 23);
            this.btn_updateLux.TabIndex = 9;
            this.btn_updateLux.Text = "LuxScan azonnali frissítés";
            this.btn_updateLux.UseVisualStyleBackColor = true;
            this.btn_updateLux.Click += new System.EventHandler(this.Btn_updateLux_Click);
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(663, 556);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(56, 13);
            this.lbl_IP.TabIndex = 15;
            this.lbl_IP.Text = "SzerverIP:";
            // 
            // BTN_ShutdownAll
            // 
            this.BTN_ShutdownAll.Location = new System.Drawing.Point(663, 385);
            this.BTN_ShutdownAll.Name = "BTN_ShutdownAll";
            this.BTN_ShutdownAll.Size = new System.Drawing.Size(254, 63);
            this.BTN_ShutdownAll.TabIndex = 18;
            this.BTN_ShutdownAll.Text = "Az összes kliens gép kikapcsolása";
            this.BTN_ShutdownAll.UseVisualStyleBackColor = true;
            this.BTN_ShutdownAll.Click += new System.EventHandler(this.BTN_ShutdownAll_Click);
            // 
            // btn_StartAll
            // 
            this.btn_StartAll.Location = new System.Drawing.Point(663, 247);
            this.btn_StartAll.Name = "btn_StartAll";
            this.btn_StartAll.Size = new System.Drawing.Size(254, 63);
            this.btn_StartAll.TabIndex = 19;
            this.btn_StartAll.Text = "Az összes kliens gép bekapcsolása";
            this.btn_StartAll.UseVisualStyleBackColor = true;
            this.btn_StartAll.Click += new System.EventHandler(this.Btn_StartAll_Click);
            // 
            // LBL_OrdFileInfos
            // 
            this.LBL_OrdFileInfos.AutoSize = true;
            this.LBL_OrdFileInfos.Location = new System.Drawing.Point(660, 12);
            this.LBL_OrdFileInfos.Name = "LBL_OrdFileInfos";
            this.LBL_OrdFileInfos.Size = new System.Drawing.Size(0, 13);
            this.LBL_OrdFileInfos.TabIndex = 21;
            // 
            // LBL_LastUpdateText
            // 
            this.LBL_LastUpdateText.AutoSize = true;
            this.LBL_LastUpdateText.Location = new System.Drawing.Point(660, 41);
            this.LBL_LastUpdateText.Name = "LBL_LastUpdateText";
            this.LBL_LastUpdateText.Size = new System.Drawing.Size(79, 13);
            this.LBL_LastUpdateText.TabIndex = 22;
            this.LBL_LastUpdateText.Text = "Utolsó frissítés:";
            // 
            // LBL_LastUpdate
            // 
            this.LBL_LastUpdate.AutoSize = true;
            this.LBL_LastUpdate.Location = new System.Drawing.Point(739, 41);
            this.LBL_LastUpdate.Name = "LBL_LastUpdate";
            this.LBL_LastUpdate.Size = new System.Drawing.Size(0, 13);
            this.LBL_LastUpdate.TabIndex = 23;
            // 
            // BTN_AllMachineRestart
            // 
            this.BTN_AllMachineRestart.Location = new System.Drawing.Point(663, 316);
            this.BTN_AllMachineRestart.Name = "BTN_AllMachineRestart";
            this.BTN_AllMachineRestart.Size = new System.Drawing.Size(254, 63);
            this.BTN_AllMachineRestart.TabIndex = 24;
            this.BTN_AllMachineRestart.Text = "Az összes kliens gép újraindítása";
            this.BTN_AllMachineRestart.UseVisualStyleBackColor = true;
            this.BTN_AllMachineRestart.Click += new System.EventHandler(this.BTN_AllMachineRestart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(663, 457);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Az asztalszáma (üres az összeshez):";
            // 
            // TxtBox_asztalszama
            // 
            this.TxtBox_asztalszama.Location = new System.Drawing.Point(666, 473);
            this.TxtBox_asztalszama.MaxLength = 6;
            this.TxtBox_asztalszama.Name = "TxtBox_asztalszama";
            this.TxtBox_asztalszama.Size = new System.Drawing.Size(174, 20);
            this.TxtBox_asztalszama.TabIndex = 26;
            this.TxtBox_asztalszama.TextChanged += new System.EventHandler(this.TxtBox_asztalszama_TextChanged);
            this.TxtBox_asztalszama.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBox_asztalszama_KeyPress);
            // 
            // Col_Active
            // 
            this.Col_Active.HeaderText = "Elérhető a gép";
            this.Col_Active.Name = "Col_Active";
            this.Col_Active.ReadOnly = true;
            // 
            // col_monSzam
            // 
            this.col_monSzam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_monSzam.HeaderText = "Monitorszám";
            this.col_monSzam.Name = "col_monSzam";
            this.col_monSzam.ReadOnly = true;
            this.col_monSzam.Visible = false;
            this.col_monSzam.Width = 91;
            // 
            // col_dbSzam
            // 
            this.col_dbSzam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_dbSzam.HeaderText = "Darabszám";
            this.col_dbSzam.Name = "col_dbSzam";
            this.col_dbSzam.ReadOnly = true;
            this.col_dbSzam.Visible = false;
            this.col_dbSzam.Width = 85;
            // 
            // col_standJobb
            // 
            this.col_standJobb.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_standJobb.HeaderText = "Dokumentum(jobb)";
            this.col_standJobb.Name = "col_standJobb";
            this.col_standJobb.ReadOnly = true;
            this.col_standJobb.Width = 121;
            // 
            // col_standBal
            // 
            this.col_standBal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_standBal.HeaderText = "Dokumentum(bal)";
            this.col_standBal.Name = "col_standBal";
            this.col_standBal.ReadOnly = true;
            this.col_standBal.Width = 115;
            // 
            // col_Cikk
            // 
            this.col_Cikk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_Cikk.HeaderText = "Cikkszám";
            this.col_Cikk.Name = "col_Cikk";
            this.col_Cikk.ReadOnly = true;
            this.col_Cikk.Width = 77;
            // 
            // col_asztal
            // 
            this.col_asztal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_asztal.HeaderText = "Monitor száma";
            this.col_asztal.Name = "col_asztal";
            this.col_asztal.ReadOnly = true;
            // 
            // dgw_Asztalok
            // 
            this.dgw_Asztalok.AllowUserToAddRows = false;
            this.dgw_Asztalok.AllowUserToDeleteRows = false;
            this.dgw_Asztalok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgw_Asztalok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_asztal,
            this.col_Cikk,
            this.col_standBal,
            this.col_standJobb,
            this.col_dbSzam,
            this.col_monSzam,
            this.Col_Active});
            this.dgw_Asztalok.Location = new System.Drawing.Point(9, 12);
            this.dgw_Asztalok.Name = "dgw_Asztalok";
            this.dgw_Asztalok.ReadOnly = true;
            this.dgw_Asztalok.Size = new System.Drawing.Size(651, 557);
            this.dgw_Asztalok.TabIndex = 7;
            this.dgw_Asztalok.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgw_Asztalok_SortCompare);
            // 
            // Tables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 611);
            this.Controls.Add(this.TxtBox_asztalszama);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTN_AllMachineRestart);
            this.Controls.Add(this.LBL_LastUpdate);
            this.Controls.Add(this.LBL_LastUpdateText);
            this.Controls.Add(this.LBL_OrdFileInfos);
            this.Controls.Add(this.Lbl_fileErrors);
            this.Controls.Add(this.btn_StartAll);
            this.Controls.Add(this.BTN_ShutdownAll);
            this.Controls.Add(this.lbl_IP);
            this.Controls.Add(this.btn_updateLux);
            this.Controls.Add(this.dgw_Asztalok);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(947, 650);
            this.MinimumSize = new System.Drawing.Size(947, 650);
            this.Name = "Tables";
            this.ShowIcon = false;
            this.Text = "Szerver";
            this.Load += new System.EventHandler(this.Tables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgw_Asztalok)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer Timer_PcCheck;
        public System.Windows.Forms.Label Lbl_fileErrors;
        public System.Windows.Forms.Button btn_updateLux;
        public System.Windows.Forms.Label lbl_IP;
        public System.Windows.Forms.Button BTN_ShutdownAll;
        public System.Windows.Forms.Button btn_StartAll;
        public System.Windows.Forms.Label LBL_OrdFileInfos;
        public System.Windows.Forms.Label LBL_LastUpdateText;
        public System.Windows.Forms.Label LBL_LastUpdate;
        public System.Windows.Forms.Button BTN_AllMachineRestart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtBox_asztalszama;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Col_Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_monSzam;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_dbSzam;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_standJobb;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_standBal;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Cikk;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_asztal;
        public System.Windows.Forms.DataGridView dgw_Asztalok;
    }
}