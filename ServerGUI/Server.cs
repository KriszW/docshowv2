using KilokoModelLibrary;
using LuxScanOrdReader;
using LuxScanRawItems;
using Machines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPServer;

namespace ServerGUI
{
    public partial class Server : Form
    {
        public FileReader OrdReader { get; set; }

        public Timer ReaderTimer { get; set; }

        public BindingList<MachinesToGUIModel> GUIModels { get; set; }

        private bool _IsUpdating;

        public Server()
        {
            InitializeComponent();

            OrdReader = new FileReader();

            OrdReader.CopySucceeded += OrdReader_CopySucceeded;

            CreateTimer();

            GUIModels = new BindingList<MachinesToGUIModel>();
            DGV_Kilokok.AutoGenerateColumns = true;
            DGV_Kilokok.DataSource = GUIModels;
        }

        private void CreateTimer()
        {
            ReaderTimer = new Timer();
            ReaderTimer.Interval = 1000;
            ReaderTimer.Tick += ReaderTimer_Tick;
            //ReaderTimer.Start();
        }

        private void UpdateGUI(List<KilokoModel> models)
        {
            foreach (var newModel in models)
            {
                for (int i = 0; i < GUIModels.Count; i++)
                {
                    var curModel = GUIModels[i];

                    if (curModel.Kiloko == newModel.Kiloko)
                    {
                        var newGUIModel = new MachinesToGUIModel(newModel);

                        curModel.Itemnumber = newGUIModel.Itemnumber;
                        curModel.DocLeft = newGUIModel.DocLeft;
                        curModel.DocRight = newGUIModel.DocRight;
                        break;
                    }
                }
            }

            UpdateDataSource();
        }

        private void MakeUpdate(System.IO.FileInfo file)
        {
            var converter = new OrderConverterToModel();

            var datas = converter.Convert(file.FullName);

            var grouper = new KilokoGrouper(datas);
            var reals = grouper.GroupKilokok();

            var models = new SendOutModels.SendOutModels()
            {
                Machines = MachineModel.Machines,
                Models = reals
            };

            models.Send();

            UpdateGUI(reals);
            OrdReader.OrderFile = file;

            UpdateORDFileInfos();
        }

        private void UpdateORDFileInfos()
        {
            if (InvokeRequired)
            {
                DGV_Kilokok.Invoke(new Action(() =>
                {
                    UpdateORDFileInfos();
                }));
            }
            else
            {
                LBL_LastUpdate.Text = DateTime.Now.ToString();
                LBL_OrdFileInfos.Text = "Név: " + OrdReader.OrderFile.Name + "\nMódosítás dátuma: " + OrdReader.OrderFile.LastWriteTime.ToString();
            }
        }

        private async void OrdReader_CopySucceeded(object sender, SuccessFileCopyArgs args)
        {
            if (OrdReader.OrderFile == default || args.NewFile.LastWriteTime > OrdReader.OrderFile.LastWriteTime)
            {
                await Task.Run(() =>
                {
                    MakeUpdate(args.NewFile);
                });
            }
        }

        private async void ReaderTimer_Tick(object sender, EventArgs e)
        {
            if (_IsUpdating == false)
            {
                if (DocsShowServer.DocsShow.Clients.Count > 0)
                {
                    _IsUpdating = true;
                    await Task.Run(() => OrdReader.CopyOrderFile());
                    _IsUpdating = false;
                }
            }
        }

        private void UpdateDataSource()
        {
            if (DGV_Kilokok.InvokeRequired)
            {
                DGV_Kilokok.Invoke(new Action(() =>
                {
                    UpdateDataSource();
                }));
            }
            else
            {
                var bindingSource = new BindingSource(GUIModels, null);
                DGV_Kilokok.DataSource = bindingSource;
                DGV_Kilokok.Update();
            }
        }

        private void SetClientState(System.Net.Sockets.Socket e, bool state)
        {
            var ip = e.RemoteEndPoint.ToString().Split(':')[0];

            foreach (var item in GUIModels)
            {
                if (item.GetIP() == ip)
                {
                    item.Active = state;
                }
            }

            UpdateDataSource();
        }

        private void Server_ClientDisconnected(object sender, System.Net.Sockets.Socket e)
        {
            DocsShowServer.DocsShow.RemoveClient(e);

            SetClientState(e, false);
        }

        private void Server_ClientConnected(object sender, System.Net.Sockets.Socket e)
        {
            DocsShowServer.DocsShow.CreateNewClient(e);

            SetClientState(e, true);
        }

        private void Server_Load(object sender, EventArgs e)
        {
            KilokoModel.Kilokok = new List<KilokoModel>();
            ItemNumberManager.ItemNumberConverter.Lines = System.IO.File.ReadAllLines(@"K:\programs\DocShow\Common\pdfek.csv").ToList();

            var machineLoader = new MachineLoader(@"C:\Users\itdiak.lkr-h\OneDrive - VELUX\Projects\pozicionalo\Pozicionalo\ServerGUI\bin\Debug\gépek.csv");
            MachineModel.Machines = machineLoader.Load();

            foreach (var item in MachineModel.Machines)
            {
                var newModel = new MachinesToGUIModel(item.KilokoNum, "", "", "", false);
                GUIModels.Add(newModel);
            }

            UpdateDataSource();

            var codeTableConv = new CodeTableConverter(@"K:\programs\DocShow\LAM_XCUT1\CodeTable.csv");
            codeTableConv.Convert();

            DocsShowServer.DocsShow = new DocsShowServer();
            DocsShowServer.DocsShow.Start(43213);

            DocsShowServer.DocsShow.Server.ClientConnected += Server_ClientConnected;
            DocsShowServer.DocsShow.Server.ClientDisconnected += Server_ClientDisconnected;
        }

        private async void BTN_updateLux_Click(object sender, EventArgs e)
        {
            if (DocsShowServer.DocsShow.Clients.Count > 0)
            {
                await Task.Run(() => OrdReader.CopyOrderFile());
            }
        }

        private async void BTN_StartAll_Click(object sender, EventArgs e)
        {
            BTN_StartAll.Enabled = false;

            await Task.Run(() => MachineManager.Start());

            BTN_StartAll.Enabled = true;
        }

        private async void BTN_AllMachineRestart_Click(object sender, EventArgs e)
        {
            BTN_AllMachineRestart.Enabled = false;

            await Task.Run(() => MachineManager.Restart());

            BTN_AllMachineRestart.Enabled = true;
        }

        private async void BTN_ShutdownAll_Click(object sender, EventArgs e)
        {
            BTN_ShutdownAll.Enabled = false;

            await Task.Run(() => MachineManager.Shutdown());

            BTN_ShutdownAll.Enabled = true;
        }
    }
}