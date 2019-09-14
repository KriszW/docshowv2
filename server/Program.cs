using System;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace DocsShowServer
{
    class Program
    {
        static void SetUpParams()
        {
            var converter = new LuxScanRawItems.OrderConverterToModel();

            var datas = converter.Convert("19x62_19x62.ord");

            CommonDatas.PathtoResources = ConfigurationManager.AppSettings["ResourcesPath"];
            CommonDatas.LuxscanDataPath = ConfigurationManager.AppSettings["LuxscanFolderPath"];
            CommonDatas.PathtoCikkek = ConfigurationManager.AppSettings["CikkekPath"];

            //ha nincs végén '\' akkkor tesz egyet
            if (!CommonDatas.PathtoResources.EndsWith("\\"))
            {
                CommonDatas.PathtoResources += '\\';
            }

            //inicializálja a threadet
            CommonDatas.GetInfos = new Thread(new ThreadStart(Infos.ReadInfos))
            {
                Name = "getInfos",
                IsBackground = true
            };
        }

        static void Main(string[] args)
        {
            //az infos-t inicialízálja
            Infos.InIt();

            //a paramétereket betölti
            SetUpParams();

            //indítja a GUIt
            RunGUI();
        }

        static void RunGUI()
        {
            //a GUI indítása
            try
            {
                Application.Run(new Tables());
                //ha kilépnék, akkor induljon újra vagy álljon le?
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:Valami hiba lépett fel a GUI futása közben ami miatt bezárodott: {ex.Message}");
                IOs.Logger.MakeLog($"valami hiba lépett fel a GUI futása közben ami miatt bezárodott: {ex.Message}");
                RunGUI();
            }
        }
    }
}