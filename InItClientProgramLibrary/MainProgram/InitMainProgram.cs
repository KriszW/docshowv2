using PositioningLib;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace InItClientProgram
{
    public class InitMainProgram
    {
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