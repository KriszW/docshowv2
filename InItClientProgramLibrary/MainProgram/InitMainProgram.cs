using PositioningLib;
using Settings.Client;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace InItClientProgram
{
    public class InitMainProgram
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //a console abblak elrejtése
        public static void Hide()
        {
            //a console ablak handleje
            IntPtr Consolehandle = NativeMethods.GetConsoleWindow();

            //a configurációs fájlból a hide érték kiolvasása
            string hide = ConfigurationManager.AppSettings["HideConsole"];

            //ha a hide YES akkor el kell rejteni a console ablakot
            if (hide.ToUpper().TrimEnd().TrimStart() == "YES")
            {
                NativeMethods.ShowWindow(Consolehandle, 0);
            }
            //különben nem
            else
            {
                NativeMethods.ShowWindow(Consolehandle, 5);
            }
        }

        public static void SetUpParams(ClientSettings settings)
        {
            _logger.Info("A settingsek betöltése");
            _logger.Info($"A PDF readernek az elérési útja beállítva: {settings.PDFReaderLoc}");
            //az alap adatok betöltése
            Datas.PathToPDFReader = settings.PDFReaderLoc;
            _logger.Info($"A PDF-ek helyének az útja beállítva: {settings.PDFResourcesPath}");
            Datas.PDFsPath = settings.PDFResourcesPath;
            _logger.Info($"A Zoom mértéke beállítva: {settings.ZoomScale}");
            Datas.ZoomScale = settings.ZoomScale.ToString();
            _logger.Info($"A Várokazási idő mértéke beállítva: {settings.WaitingTime}");
            Datas.WaitingTime = settings.WaitingTime;
            _logger.Info($"A Monitorok száma beállítva: {Screen.AllScreens.Length}");
            Datas.CountOfMonitors = Screen.AllScreens.Length;
            _logger.Info($"A Szerver IP beállítva: {settings.ServerIP}");
            Datas.ServerIP = settings.ServerIP;
            _logger.Info($"A Szerver port száma beállítva: {settings.ServerPort}");
            Datas.Port = (ushort)settings.ServerPort;

            _logger.Info($"A MonitorInfók inicializálása {Datas.CountOfMonitors} monitorral");
            //a monitorinfos inicializálása
            Datas.MonitorInfos = new MonitorInfo[Datas.CountOfMonitors];

            _logger.Info("A settingsek betöltve");
        }

        public static void SetUpParams()
        {
            //az alap adatok betöltése
            Datas.PathToPDFReader = ConfigurationManager.AppSettings["PDFReaderLoc"];
            Datas.PDFsPath = ConfigurationManager.AppSettings["PDFResourcesPath"];
            Datas.ZoomScale = ConfigurationManager.AppSettings["ZoomScale"];
            Datas.WaitingTime = int.Parse(ConfigurationManager.AppSettings["WaitingTime"]);
            Datas.CountOfMonitors = Screen.AllScreens.Length;
            Datas.ServerIP = ConfigurationManager.AppSettings["ServerIP"];
            Datas.Port = ushort.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            //a monitorinfos inicializálása
            Datas.MonitorInfos = new MonitorInfo[Datas.CountOfMonitors];
        }
    }
}