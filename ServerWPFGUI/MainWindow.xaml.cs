using ItemNumberManager;
using KilokoModelLibrary;
using LuxScanOrdReader;
using LuxScanRawItems;
using Machines;
using SendOutModels;
using Settings.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
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
        public SendOutDataModels ActiveOrder { get; set; }
        public List<KilokoModel> ActiveKilokok { get; set; }
        
        public IReadOnlyCollection<MachinesToGUIModel> GUIModels { get => _dgModels.AsReadOnly(); }

        private bool _IsUpdating;

        public DocsShowServerGUI()
        {
            InitializeComponent();

            ServerSettings.CurrentSettings = LoadSettings("Settings\\settings.json");

            OrdReader = new FileReader();

            OrdReader.CopySucceeded += OrdReader_CopySucceeded;

            _dgModels = new List<MachinesToGUIModel>();
        }

        public ServerSettings LoadSettings(string path)
        {
            try
            {
                return ServerSettings.LoadSettings(path);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message,"Settings betöltése",MessageBoxButton.OK,MessageBoxImage.Error);
                Close();
            }

            return default;
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
            _IsUpdating = true;
            var converter = new OrderConverterToModel();

            var datas = converter.Convert(file.FullName);

            var grouper = new KilokoGrouper(datas);
            ActiveKilokok = grouper.GroupKilokok();

            ActiveOrder = new SendOutDataModels(ActiveKilokok, Machine.Machines);

            ActiveOrder.Send();

            UpdateGUI(ActiveKilokok);
            OrdReader.OrderFile = file;

            UpdateORDFileInfos();
            _IsUpdating = false;
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

        private void SetClientState(string ip, bool state)
        {
            var kilokok = GetKilokok(ip);

            foreach (var item in kilokok)
            {
                item.Active = state;
            }

            UpdateDataSource();
        }

        private List<MachinesToGUIModel> GetKilokok(string ip)
        {
            var output = new List<MachinesToGUIModel>();

            foreach (var item in _dgModels)
            {
                if (item.GetIP() == ip)
                {
                    output.Add(item);
                }
            }

            return output;
        }

        private void Server_ClientDisconnected(object sender, System.Net.Sockets.Socket e)
        {
            DocsShowServer.DocsShow.RemoveClient(e);

            var ip = e.RemoteEndPoint.ToString().Split(':')[0];

            SetClientState(ip, false);
        }

        private void SendFirst(string ip)
        {
            var kilokok = GetKilokok(ip);

            var reals = GetKilokok(kilokok);

            var datas = ActiveOrder.GetModelsFromKilokok(reals);

            foreach (var item in datas)
            {
                ActiveOrder.SendDatasOutToClient(item);
            }
        }

        private List<KilokoModel> GetKilokok(List<MachinesToGUIModel> kilokok) => (from kiloko in kilokok
                                                                                   from activeKiloko in ActiveKilokok
                                                                                   where kiloko.Kiloko == activeKiloko.Kiloko
                                                                                   select activeKiloko).ToList();

        private void Server_ClientConnected(object sender, System.Net.Sockets.Socket e)
        {
            DocsShowServer.DocsShow.CreateNewClient(e);

            var ip = e.RemoteEndPoint.ToString().Split(':')[0];

            SetClientState(ip, true);

            if (ActiveOrder != default)
            {
                SendFirst(ip); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KilokoModel.Kilokok = new List<KilokoModel>();
            ItemNumberConverter.Lines = System.IO.File.ReadAllLines(ServerSettings.CurrentSettings.ItemNumberFile).ToList();

            var machineLoader = new MachineLoader(ServerSettings.CurrentSettings.Machines);
            Machine.Machines = machineLoader.Load();

            foreach (var item in Machine.Machines)
            {
                var newModel = new MachinesToGUIModel(item.KilokoNum, "", "", "", false);
                _dgModels.Add(newModel);
            }

            UpdateDataSource();

            var codeTableConv = new CodeTableConverter(ServerSettings.CurrentSettings.CodeTableFile);
            codeTableConv.Convert();

            DocsShowServer.DocsShow = new DocsShowServer();
            DocsShowServer.DocsShow.Start(43213);

            DocsShowServer.DocsShow.Server.ClientConnected += Server_ClientConnected;
            DocsShowServer.DocsShow.Server.ClientDisconnected += Server_ClientDisconnected;

            ReaderTimer = CreateTimer(ServerSettings.CurrentSettings.Interval * 1000);

            InstantUpdate();
        }

        private async void Button_RestartTables_Click(object sender, RoutedEventArgs e)
        {
            await ManipulateMachines(sender as Button, new Action<string>(MachineManager.Restart),TextBox_TableID.Text);
        }

        private async void Button_StopTables_Click(object sender, RoutedEventArgs e)
        {
            await ManipulateMachines(sender as Button, new Action<string>(MachineManager.Shutdown), TextBox_TableID.Text);
        }

        private async void Button_StartTables_Click(object sender, RoutedEventArgs e)
        {
            await ManipulateMachines(sender as Button, new Action<string>(MachineManager.Start), TextBox_TableID.Text);
        }

        async Task ManipulateMachines(Button btn, Action<string> manipulator, string param)
        {
            btn.IsEnabled = false;

            await Task.Run(() => manipulator.Invoke(param));

            btn.IsEnabled = true;
        }

        private async void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            if (DocsShowServer.DocsShow.Clients.Count > 0)
            {
                await Task.Run(() => InstantUpdate());
            }
        }

        private void InstantUpdate()
        {
            OrdReader.CopyOrderFile();
            if (OrdReader.OrderFile != default)
            {
                MakeUpdate(OrdReader.OrderFile);
            }
        }

        private void TextBox_TableID_TextChanged(object sender, TextChangedEventArgs e)
        {
            var defRestart = "asztal újraindítása";
            var defStop = "asztal indítása";
            var defStart = "asztal leállítása";

            if (TextBox_TableID.Text == "")
            {
                Button_RestartTables.Content = "Összes ";
                Button_StartTables.Content = "Összes ";
                Button_StopTables.Content = "Összes ";
            }
            else
            {
                Button_RestartTables.Content = TextBox_TableID.Text + ". ";
                Button_StopTables.Content = TextBox_TableID.Text + ". ";
                Button_StartTables.Content = TextBox_TableID.Text + ". ";
            }

            Button_RestartTables.Content += defRestart;
            Button_StopTables.Content += defStop;
            Button_StartTables.Content += defStart;
        }

        private void TextBox_TableID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {

            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {

            }
            else if (e.Key == Key.Back || e.Key == Key.Enter)
            {

            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
