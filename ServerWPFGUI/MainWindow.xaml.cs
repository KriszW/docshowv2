﻿using ItemNumberManager;
using KilokoModelLibrary;
using LuxScanOrdReader;
using LuxScanRawItems;
using Machines;
using SendOutModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCPServer;

namespace ServerWPFGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DocsShowServerGUI : Window
    {
        public FileReader OrdReader { get; set; }

        public Timer ReaderTimer { get; set; }

        private List<MachinesToGUIModel> _dgModels { get; set; }
        
        public IReadOnlyCollection<MachinesToGUIModel> GUIModelrs { get => _dgModels.AsReadOnly(); }

        private bool _IsUpdating;

        public DocsShowServerGUI()
        {
            InitializeComponent();

            OrdReader = new FileReader();

            OrdReader.CopySucceeded += OrdReader_CopySucceeded;

            ReaderTimer = CreateTimer(15000);

            _dgModels = new List<MachinesToGUIModel>();
        }

        private Timer CreateTimer(double interval)
        {
            var output = new Timer();
            output.Interval = interval;
            output.Elapsed += Update;
            output.Start();
            return output;
        }
        private void UpdateGUI(List<KilokoModel> models)
        {
            foreach (var newModel in models)
            {
                for (int i = 0; i < _dgModels.Count; i++)
                {
                    var curModel = _dgModels[i];

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

        public void MakeUpdate(System.IO.FileInfo file)
        {
            var converter = new OrderConverterToModel();

            var datas = converter.Convert(file.FullName);

            var grouper = new KilokoGrouper(datas);
            var reals = grouper.GroupKilokok();

            var models = new SendOutDataModels()
            {
                Machines = Machine.Machines,
                Models = reals
            };

            models.Send();

            UpdateGUI(reals);
            OrdReader.OrderFile = file;

            UpdateORDFileInfos();
        }

        private void UpdateORDFileInfos()
        {
            UpdateLastTryDate();
            UpdateLastSuccessTryDate();
            UpdateLatestFileInfos();
        }

        private void UpdateLatestFileInfos()
        {
            Dispatcher.Invoke(() =>
            {
                Label_FileName.Content = OrdReader.OrderFile.Name;
                Label_FileLastModifiedDate.Content = OrdReader.OrderFile.LastWriteTime.ToString();
            });
        }

        private void UpdateLastTryDate()
        {
            Dispatcher.Invoke(() =>
            {
                Label_LastPDFUpdateDate.Content = DateTime.Now.ToString();
            });
        }
        private void UpdateLastSuccessTryDate()
        {
            Dispatcher.Invoke(() =>
            {
                Label_LastSuccessPDFUpdateDate.Content = DateTime.Now.ToString();
            });
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

        private async void Update(object sender, ElapsedEventArgs e)
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

            UpdateLastTryDate();
        }

        private void UpdateDataSource()
        {
            Dispatcher.Invoke(() =>
            {
                DataGrid_Kilokok.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<MachinesToGUIModel>(_dgModels);
            });
        }

        private void SetClientState(System.Net.Sockets.Socket e, bool state)
        {
            var ip = e.RemoteEndPoint.ToString().Split(':')[0];

            foreach (var item in _dgModels)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KilokoModel.Kilokok = new List<KilokoModel>();
            ItemNumberConverter.Lines = System.IO.File.ReadAllLines(@"K:\programs\DocShow\Common\pdfek.csv").ToList();

            var machineLoader = new MachineLoader(@"gépek.csv");
            Machine.Machines = machineLoader.Load();

            foreach (var item in Machine.Machines)
            {
                var newModel = new MachinesToGUIModel(item.KilokoNum, "", "", "", false);
                _dgModels.Add(newModel);
            }

            UpdateDataSource();

            var codeTableConv = new CodeTableConverter(@"K:\programs\DocShow\LAM_XCUT1\CodeTable.csv");
            codeTableConv.Convert();

            DocsShowServer.DocsShow = new DocsShowServer();
            DocsShowServer.DocsShow.Start(43213);

            DocsShowServer.DocsShow.Server.ClientConnected += Server_ClientConnected;
            DocsShowServer.DocsShow.Server.ClientDisconnected += Server_ClientDisconnected;
        }

        private async void Button_RestartTables_Click(object sender, RoutedEventArgs e)
        {
            await ManipulateMachines(sender as Button, new Action(MachineManager.Restart));
        }

        private async void Button_StopTables_Click(object sender, RoutedEventArgs e)
        {
            await ManipulateMachines(sender as Button, new Action(MachineManager.Shutdown));
        }

        private async void Button_StartTables_Click(object sender, RoutedEventArgs e)
        {
            await ManipulateMachines(sender as Button, new Action(MachineManager.Start));
        }

        async Task ManipulateMachines(Button btn, Action manipulator)
        {
            btn.IsEnabled = false;

            await Task.Run(() => manipulator.Invoke());

            btn.IsEnabled = true;
        }

        private async void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            if (DocsShowServer.DocsShow.Clients.Count > 0)
            {
                await Task.Run(() => {
                    OrdReader.CopyOrderFile();
                    if (OrdReader.OrderFile != default)
                    {
                        MakeUpdate(OrdReader.OrderFile);
                    }
                });
            }
        }
    }
}
