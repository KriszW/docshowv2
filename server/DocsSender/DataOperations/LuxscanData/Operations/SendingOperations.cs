using IOs;
using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class SendingOperations
    {
        public IKilokoModel[] PrevModels { get; set; } = new IKilokoModel[CommonDatas.Kilokok.Length];

        bool _first = true;

        //egy kiloko model alapján kiküldi az adatokat, egy cikket
        void SendCikkToMachine(IKilokoModel kiloko)
        {
            try {
                string cikk = (from cikkRaw in kiloko.Cikkek where cikkRaw != null select cikkRaw).First();

                LuxScanDatas.SendDatasToClient(cikk, kiloko.Kiloko);
            }
            catch (ApplicationException ex) {
                Console.WriteLine($"{DateTime.Now.ToString()}:{ex.Message}");
                Logger.MakeLog(ex.Message);
            }
            catch (Exception ex) {
                Logger.MakeLog(ex.Message);
            }
        }

        //egy kiloko model alapján kiküldi az adatokat, két cikket
        void SendDoubleCikkToMachine(IKilokoModel kiloko)
        {
            try
            {
                if (kiloko.Cikkek.Length>2)
                {
                    string[] cikkek = new string[2] { kiloko.Cikkek[0], kiloko.Cikkek[1] };

                    OperationModel.DoubleLineOperations.SendDoubleStandardToOneAsztal(kiloko.Kiloko,cikkek);
                }
                else
                {
                    OperationModel.DoubleLineOperations.SendDoubleStandardToOneAsztal(kiloko.Kiloko, kiloko.Cikkek.ToArray());
                }
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:{ex.Message}");
                Logger.MakeLog(ex.Message);
            }
            catch (Exception ex)
            {
                if (kiloko.Client!=null)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}: Ismeretlen hiba lépett fel a cikkek elküldése közben a {kiloko.Client?.ClientIP.ToString()} kliensre: {ex.Message}");
                    Logger.MakeLog($"Ismeretlen hiba lépett fel a cikkek elküldése közben a {kiloko.Client?.ClientIP.ToString()} kliensre: {ex.Message}");
                }
            }
        }

        //a kilőkő model alapján megállapítja, hogy hány cikket kell kiküldeni
        public void ManageCikks(IKilokoModel kiloko)
        {
            var cikkek = (from cikk in kiloko.Cikkek where cikk != null select cikk).ToArray();

            switch (cikkek?.Length)
            {
                case 0:
                    CommonDatas.GUI.DGVManager.ReplaceInfo(kiloko.Kiloko, "A kilökő üres", "A kilökő üres", "A kilökő nem aktív");
                    break;
                case 1:
                    SendCikkToMachine(kiloko);
                    break;
                case 2:
                    SendDoubleCikkToMachine(kiloko);
                    break;
                default:
                    SendDoubleCikkToMachine(kiloko);
                    break;
            }
        }

        //elküldi az összes adatot
        void SendAllData()
        {
            //inicialízálja az új kilőkőmodeleket
            OperationModel.LineMaker.SetLineModel(null);

            //Ha a Gábornak kellneke az előző értekek a hibás .ord fájl után, akkor ide kell bemásolni a 
            if (CommonDatas.LuxscanFilePath!="")
            {
                SendOutDatasToClients();
            }

            _first = false;
        }

        void SendOutDatasToClients()
        {
            List<Task> sendingTasks = new List<Task>();

            //végig megy a listán és mindegyiknél eldönti hogy kell küldeni a cikkeket
            for (int i = 0; i < CommonDatas.Kilokok.Length; i++)
            {
                //az elöző model lekérése
                IKilokoModel prevModel = PrevModels[i];
                //az új model lekérése
                IKilokoModel newModel = CommonDatas.Kilokok[i];

                if (CommonDatas.NetworkSpareLevel > 10)
                {
                    if (!_first)
                    {
                        if (prevModel.Cikkek == newModel.Cikkek)
                        {
                            continue;
                        }
                    }
                }

                ManageCikks(newModel);
            }

            Task.WaitAll(sendingTasks.ToArray());
        }

        public void SendOutDatas()
        {
            //lemásolja az .ord fájlt egy bat elindításával
            OperationModel.DataManipulating.CopyOrderFile();

            //az összes adat elküldése
            SendAllData();

            if (CommonDatas.LuxscanFilePath!="")
            {
                CommonDatas.GUI.LabelSetter.SetOrdFileDatas(new System.IO.FileInfo(CommonDatas.LuxscanFilePath));

                //beállítja a mostani időt, amikor frissítve lett a szerver adatai
                CommonDatas.GUI.LabelSetter.SetOrdUpdateTime();
            }
        }
    }
}
