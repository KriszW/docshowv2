using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;

namespace DocsShowServer
{
    public partial class Tables : Form
    {
        int _maxAsztalSzama = 0;

        #region props and fields

        public DataLoader Loader { get; set; }
        public DataGridViewManager DGVManager { get; set; }
        public KilokoSetter Setter { get; set; }
        public OrderUpdateLabelSetter LabelSetter { get; set; }

        private PCLifeCheck checker = new PCLifeCheck();
        #endregion

        int GetMaxAsztalSzama()
        {
            return (from kiloko in CommonDatas.Kilokok select int.Parse(kiloko.Kiloko)).Max();
        }

        //ha vége a hibának ez állítja üresre a fileerror labelt
        public void SetFileErrorDone()
        {
            if(Lbl_fileErrors.InvokeRequired)
            {
                Lbl_fileErrors.BeginInvoke((Action)(() => {
                    Lbl_fileErrors.Text = "";
                }));
            }
        }

        //ha file error van ez állítja be a piros hiba üzenetet
        public void SetFileError(string errMSG)
        {
            if(Lbl_fileErrors.InvokeRequired)
            {
                Lbl_fileErrors.BeginInvoke((Action)(() => {
                    Lbl_fileErrors.Text = errMSG;
                    //hang lejátszása?
                    //System.Media.SoundPlayer player = new SoundPlayer("wav file location");
                }));
            }
        }

        private void Tables_Load(object sender, EventArgs e)
        {
            //init classok inicializálása
            Loader = new DataLoader(this);
            DGVManager = new DataGridViewManager(this);
            Setter = new KilokoSetter(this);
            LabelSetter = new OrderUpdateLabelSetter(this);

            //a kilökök lista loadolása és a machineok loadolása
            Loader.SetUpDefaultLoads();

            //a table beállítsa
            CommonDatas.GUI = this;

            CommonDatas.NetworkSpareLevel = int.Parse(ConfigurationManager.AppSettings["NetworkSpareLevel"]);

            //DocsShowServerek elindítás
            Server.InIt(int.Parse(ConfigurationManager.AppSettings["Port"]));

            //az ip beállítása
            lbl_IP.Text += Server.ServerIP + ":" + Server.ServerPort;

            SetUpPcCheckTimer();

            TxtBox_asztalszama.MaxLength = Server.MaxClientCount.ToString().Length;

            CommonDatas.GetInfos.Start();

            _maxAsztalSzama = GetMaxAsztalSzama();
        }

        void SetUpPcCheckTimer()
        {
            //a pc vizsgáló idejét állítja be, ha 0 akkor ki van kapcsolva
            int time = int.Parse(ConfigurationManager.AppSettings["PcCheckIntervall"].ToString()) * 1000;

            //ha 0 akkor ki van kapcsolva, ha nem akkor megy tovább
            if (time == 0)
            {
                Timer_PcCheck.Enabled = false;
            }
            else
            {
                //az intervall beállítása
                Timer_PcCheck.Interval = time;

                //a Tick eseményre az algoritmus beállítása
                Timer_PcCheck.Tick += Timer_PcCheck_Tick;

                //a timer elindítása
                Timer_PcCheck.Start();
            }
        }


        public Tables()
        {
            InitializeComponent();
        }

        public void Btn_updateLux_Click(object sender, EventArgs e)
        {
            //a button letíltása amíg küldi a fájlokat 
            btn_updateLux.Enabled = false;

            //az adatok elküldése
            Task luxscanTask = Task.Run(() => {
                LuxscanOperations.MakeLuxscanUpdate();
            });

            luxscanTask.Wait();

            //a luxscan gomb engedélyezése
            btn_updateLux.Enabled = true;
        }

        public void BTN_ShutdownAll_Click(object sender, EventArgs e)
        {
            BTN_ShutdownAll.Enabled = false;

            string asztalSzama = TxtBox_asztalszama.Text;

            //a shutdown task indítása
            Task shutdownTask = Task.Run(() =>
            {
                MachineOperations.Shutdown(asztalSzama);
            });

            shutdownTask.Wait();

            BTN_ShutdownAll.Enabled = true;
        }

        public void Btn_StartAll_Click(object sender, EventArgs e)
        {
            btn_StartAll.Enabled = false;

            //a start task indítása
            Task startTask = Task.Run(() => {
                MachineOperations.StartAll();
            });

            startTask.Wait();

            btn_StartAll.Enabled = true;
        }


        private void Timer_PcCheck_Tick(object sender, EventArgs e)
        {
            if (checker.IsChecking == false)
            {
                Task.Run(() => {
                    checker.Check();
                });
            }
        }

        private void BTN_AllMachineRestart_Click(object sender, EventArgs e)
        {
            BTN_AllMachineRestart.Enabled = false;

            string asztalSzama = TxtBox_asztalszama.Text;

            Task restartTask = Task.Run(() => {
                MachineOperations.Restart(asztalSzama);
            });

            restartTask.Wait();

            BTN_AllMachineRestart.Enabled = true;
        }

        private void TxtBox_asztalszama_KeyPress(object sender, KeyPressEventArgs e)
        {
            char pressedKey = e.KeyChar;

            if (char.IsDigit(pressedKey))
            {

            }
            else if (pressedKey=='\b')
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtBox_asztalszama_TextChanged(object sender, EventArgs e)
        {
            if (TxtBox_asztalszama.Text=="")
            {
                BTN_ShutdownAll.Text = "Az összes kliens gép kikapcsolása";
                BTN_AllMachineRestart.Text = "Az összes kliens gép újraindítása";
            }
            else
            {
                int asztalSzama = int.Parse(TxtBox_asztalszama.Text);

                if (asztalSzama>_maxAsztalSzama)
                {
                    asztalSzama = _maxAsztalSzama;
                }
                else if (asztalSzama==0)
                {
                    asztalSzama = 1;
                }

                TxtBox_asztalszama.Text = asztalSzama.ToString();

                BTN_ShutdownAll.Text = $"A(z) {asztalSzama}. kliens kikapcsolása";
                BTN_AllMachineRestart.Text = $"A(z) {asztalSzama}. kliens újraindítása";
            }
        }

        private void dgw_Asztalok_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //ha a kilőkőket rendezi
            if (e.Column.Index==0)
            {
                //a két intté konvertálás és összehasonlítás
                e.SortResult = int.Parse(e.CellValue1.ToString()).CompareTo(int.Parse(e.CellValue2.ToString()));
                e.Handled = true;
            }
        }
    }
}
