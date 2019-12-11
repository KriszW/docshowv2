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

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ServerWPFGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DocsShowServerGUI : Window
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            _logger.Debug("A server beállításainak betöltése");
            ServerSettings.CurrentSettings = LoadSettings("Settings\\settings.json");
            _logger.Debug("A server beállításainak betöltve");

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
                _logger.Fatal($"A {path} settings fájl betöltése közben hiba lépett fel: {ex.Message}",ex);
                Close();
            }

            return default;
        }

        private Timer CreateTimer(double interval)
        {
            _logger.Debug($"A Order fájl updatelő inicializálása {interval/1000} másodpercenként");
            var output = new Timer();
            output.Interval = interval;
            output.Elapsed += Update;
            output.Start();
            return output;
        }

        private void UpdateGUI(List<KilokoModel> models)
        {
            _logger.Debug("A felhasználói felűlet frissítése");
            foreach (var newModel in models)
            {
                for (int i = 0; i < _dgModels.Count; i++)
                {
                    var curModel = _dgModels[i];

                    if (curModel.Kiloko == newModel.Kiloko)
                    {
                        _logger.Debug($"A {curModel.Kiloko} asztal frissítése, és új model létrehozása:");
                        var newGUIModel = new MachinesToGUIModel(newModel);
                        _logger.Debug($"A {curModel.Kiloko} új model létrehozva:");
                        curModel.Itemnumber = newGUIModel.Itemnumber;
                        _logger.Debug($"A {curModel.Kiloko} új cikk beállítva: {newGUIModel.Itemnumber}-re");
                        curModel.DocLeft = newGUIModel.DocLeft;
                        _logger.Debug($"A {curModel.Kiloko} új bal doksi beállítva: {newGUIModel.DocLeft}-re");
                        curModel.DocRight = newGUIModel.DocRight;
                        _logger.Debug($"A {curModel.Kiloko} új jobb doksi beállítva: {newGUIModel.DocRight}-re");
                        break;
                    }
                }
            }

            UpdateDataSource();
        }

        public void MakeUpdate(System.IO.FileInfo file)
        {
            _IsUpdating = true;
            _logger.Debug("A luxscan frissítés elkezdése");
            var converter = new OrderConverterToModel();

            _logger.Debug("Az order fájl lefordítása a megfelelő alap Kilőkő adatokra");
            var datas = converter.Convert(file.FullName);
            _logger.Debug("Az order fájl lefordítása a megfelelő alap Kilőkő adatokra kész");

            _logger.Debug("Az order fájl lefordított adatait gépekre lebontása");
            var grouper = new KilokoGrouper(datas);
            ActiveKilokok = grouper.GroupKilokok();
            _logger.Debug("Az order fájl lefordított adatait gépekre lebontása kész");

            ActiveOrder = new SendOutDataModels(ActiveKilokok, Machine.Machines);

            _logger.Debug("Az order fájl lefordított adatait kiküldés a gépekre");
            ActiveOrder.Send();
            _logger.Debug("Az order fájl lefordított adatait kiküldés a gépekre kész");

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
            var res = OrdReader.OrderFile == default || args.NewFile.LastWriteTime > OrdReader.OrderFile.LastWriteTime;
            _logger.Debug($"Az order fájl sikeresen felmásolva, most kell-e frissítést kiküldeni: {res}");
            if (res)
            {
                await Task.Run(() =>
                {
                    _logger.Info($"Az order fájl sikeresen felmásolva, frissítés kiküldése...");
                    MakeUpdate(args.NewFile);
                    _logger.Info($"Az order fájl sikeresen felmásolva, frissítés kiküldése kész");
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
            _logger.Debug("A datagrid adatainak frissítése");
            Dispatcher.Invoke(() =>
            {
                DataGrid_Kilokok.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<MachinesToGUIModel>(_dgModels);
            });
        }

        private void SetClientState(string ip, bool state)
        {
            _logger.Debug($"{ip} címmel a kliens státuszának beállítása {(state ? "felcsatlakozvára" : "lecsatlakozvára")}");
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
            var ip = e.RemoteEndPoint.ToString().Split(':')[0];

            _logger.Info($"{ip} címmel a kliens lecsatlakozott");
            DocsShowServer.DocsShow.RemoveClient(e);

            SetClientState(ip, false);
        }

        private void SendFirst(string ip)
        {
            _logger.Info($"Az alap doksik kiküldése a {ip}-re");
            _logger.Debug($"Az {ip}-re a kilőkők megszerzése a GUIról");
            var kilokok = GetKilokok(ip);

            _logger.Debug($"Az {ip}-re a kilőkők megszerzése az IP alapján");
            var reals = GetKilokok(kilokok);

            _logger.Debug($"Az {ip}-re a kiküldendő doksik megszerzése");
            var datas = ActiveOrder.GetModelsFromKilokok(reals);

            foreach (var item in datas)
            {
                _logger.Debug($"Az {ip}-re a {item.Machine.KilokoNum} doksiajinak kiküldése");
                ActiveOrder.SendDatasOutToClient(item);
            }
        }

        private List<KilokoModel> GetKilokok(List<MachinesToGUIModel> kilokok) => (from kiloko in kilokok
                                                                                   from activeKiloko in ActiveKilokok
                                                                                   where kiloko.Kiloko == activeKiloko.Kiloko
                                                                                   select activeKiloko).ToList();

        private void Server_ClientConnected(object sender, System.Net.Sockets.Socket e)
        {
            var ip = e.RemoteEndPoint.ToString().Split(':')[0];

            _logger.Info($"{ip} ipvel kliens csatlakozott fel a szerverre");

            DocsShowServer.DocsShow.CreateNewClient(e);

            SetClientState(ip, true);

            if (ActiveOrder != default)
            {
                SendFirst(ip); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _logger.Info("Szerver inicializálása...");
            KilokoModel.Kilokok = new List<KilokoModel>();
            _logger.Info($"PDF linkek betöltése a {ServerSettings.CurrentSettings.ItemNumberFile} fájlból...");
            ItemNumberConverter.Lines = System.IO.File.ReadAllLines(ServerSettings.CurrentSettings.ItemNumberFile).ToList();
            _logger.Info($"PDF linkek betöltése a {ServerSettings.CurrentSettings.ItemNumberFile} fájlból kész");

            _logger.Info($"Gép adatok betöltése a {ServerSettings.CurrentSettings.Machines} fájlból...");
            var machineLoader = new MachineLoader(ServerSettings.CurrentSettings.Machines);
            Machine.Machines = machineLoader.Load();
            _logger.Info($"Gép adatok betöltése a {ServerSettings.CurrentSettings.Machines} fájlból kész");

            foreach (var item in Machine.Machines)
            {
                var newModel = new MachinesToGUIModel(item.KilokoNum, "", "", "", false);
                _dgModels.Add(newModel);
            }

            UpdateDataSource();

            _logger.Info($"Kód tábla betöltése a {ServerSettings.CurrentSettings.CodeTableFile} fájlból kész");
            var codeTableConv = new CodeTableConverter(ServerSettings.CurrentSettings.CodeTableFile);
            codeTableConv.Convert();
            _logger.Info($"Kód tábla betöltése a {ServerSettings.CurrentSettings.CodeTableFile} fájlból kész");

            _logger.Info($"A TCP szerver inicializálása a {ServerSettings.CurrentSettings.ServerPort} porton");
            DocsShowServer.DocsShow = new DocsShowServer();
            DocsShowServer.DocsShow.Start(ServerSettings.CurrentSettings.ServerPort);
            _logger.Info($"A TCP szerver a {ServerSettings.CurrentSettings.ServerPort} porton elindítva, és várja a klienseket");

            DocsShowServer.DocsShow.Server.ClientConnected += Server_ClientConnected;
            DocsShowServer.DocsShow.Server.ClientDisconnected += Server_ClientDisconnected;

            ReaderTimer = CreateTimer(ServerSettings.CurrentSettings.Interval * 1000);

            _logger.Info("Legelső futásnál, az alapadatok betöltése");
            InstantUpdate();
        }

        private async void Button_RestartTables_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info($"A {(TextBox_TableID.Text == "" ? "Összes" : TextBox_TableID.Text)} asztal újraindítása");
            await ManipulateMachines(sender as Button, new Action<string>(MachineManager.Restart),TextBox_TableID.Text);
            _logger.Info($"A {(TextBox_TableID.Text == "" ? "Összes" : TextBox_TableID.Text)} asztal újraindítva");
        }

        private async void Button_StopTables_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info($"A {(TextBox_TableID.Text == "" ? "Összes" : TextBox_TableID.Text)} asztal leállítása");
            await ManipulateMachines(sender as Button, new Action<string>(MachineManager.Shutdown), TextBox_TableID.Text);
            _logger.Info($"A {(TextBox_TableID.Text == "" ? "Összes" : TextBox_TableID.Text)} asztal leállítva");
        }

        private async void Button_StartTables_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info($"A {(TextBox_TableID.Text == "" ? "Összes" : TextBox_TableID.Text)} asztal indítása");
            await ManipulateMachines(sender as Button, new Action<string>(MachineManager.Start), TextBox_TableID.Text);
            _logger.Info($"A {(TextBox_TableID.Text == "" ? "Összes" : TextBox_TableID.Text)} asztal indítva");
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
                _logger.Info($"Manuális frissítés indítása");
                await Task.Run(() => InstantUpdate());
                _logger.Info($"Manuális frissítés lefutva");
            }
        }

        private void InstantUpdate()
        {
            _logger.Debug($"Az ord fájl másolása");
            OrdReader.CopyOrderFile();
            if (OrdReader.OrderFile != default)
            {
                _logger.Debug($"Az ord fájl lemásolva sikeresen, a frissítés inditása");
                MakeUpdate(OrdReader.OrderFile);
            }
            else
            {
                _logger.Debug($"Az ord fájl lemásolva sikertelenül, a frissítés inditásának megszakítása");
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
