using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Linq;

namespace DocsShowServer
{
    class DataChecking
    {
        private static Task readerTask = null;

        static void TryGetOrdFile()
        {
            string directoryPath = CommonDatas.LuxscanDataPath;

            if (directoryPath == "")
            {
                directoryPath = Directory.GetCurrentDirectory();
            }

            //a fájlok lekérdezése
            string[] LuxscanDataPaths = Directory.GetFiles(directoryPath);

            //linq query készítés
            var pathCount = (from path in LuxscanDataPaths
                        where path.EndsWith(".ord")
                        select path).Count();

            //addig csinálja amíg nem csak egy darab .ord fájl van
            while (pathCount != 1)
            {
                OperationModel.DataManipulating.CopyOrderFile();

                //a fájlok újra olvasása
                LuxscanDataPaths = Directory.GetFiles(directoryPath);

                //új query
                pathCount = (from path in LuxscanDataPaths
                        where path.EndsWith(".ord")
                        select path).Count();

                Thread.Sleep(100);
            }

            //a hasluxscanfile igazra állítása
            CommonDatas.HasLuxscanFile = true;

            //az elérésiút beállítása
            CommonDatas.LuxscanFilePath = (from path in LuxscanDataPaths
                                          where path.EndsWith(".ord")
                                          select path).First();
        }

        //egy olvasásra megnyitó stream és egy sor olvasásával nézzem hogy a fájl elérhető-e
        static void TryReadWhileNotOpened(string path,string errMSG)
        {
            if (errMSG!="")
            {
                CommonDatas.GUI.SetFileError(errMSG);

                //ez azért kell, mert ha nem olvasható lefagy a GUI, mert a GUI szálat várakoztatja ez 
                if (!CommonDatas.GUI.btn_updateLux.Enabled)
                {
                    return;
                }

                bool isOpened = true;

                StreamReader tmpReader = null;

                while (isOpened)
                {
                    try
                    {
                        tmpReader = new StreamReader(path);

                        string line = tmpReader.ReadLine();

                        isOpened = false;
                    }
                    catch (Exception)
                    {
                    }

                    Thread.Sleep(100);
                }

                tmpReader?.Close();
            }
            else if (path=="" || path.EndsWith(".ord"))
            {
                TryGetOrdFile();
            }

            //a ha már olvasható akkor az error üzenet eltüntetése
            CommonDatas.GUI.SetFileErrorDone();
        }

        //ha a fájl nyitva akkor megadott időközönként figyelje hogy elérhető-e
        public static void CheckTheExcelIsOpened(string path,bool forceRead)
        {
            string errMSG = "";

            //megnézni hogy a .ord fájl vagy valami más adatot tartalmozzó-e

            if (path != "")
            {
                if (!path.EndsWith(".ord"))
                {
                    errMSG = $"A {path} excel fájl meg van nyitva valahol, valószínüleg egy Excelben\nAmíg az nincs bezárva addig a program nem tud tovább müködni, vagy a régi verzióval dolgozik";
                }
            }

            if (!CommonDatas.GUI.btn_updateLux.Enabled)
            {
                forceRead = false;
            }

            //az olvasás próbálgatása, ha kell vissza kapcsolni
            if (forceRead)
            {
                if (readerTask==null)
                {
                    readerTask = Task.Run(() => {
                        TryReadWhileNotOpened(path, errMSG);
                    });

                    readerTask?.Wait();
                    readerTask = null;
                }
            }
            else
            {
                if (readerTask == null)
                {
                    readerTask = Task.Run(() => {
                        TryReadWhileNotOpened(path, errMSG);
                        readerTask = null;
                    });
                }
            }
        }

        //hogy ez a sor használható-e, tehát cikket tartalmaz-e vagy kilőkőt
        public bool IsUseAbleLine(string line)
        {
            string[] useAbleDatas = "Item;ItemName".Split(';');

            string lineStartWithThis = "";

            for (int i = 0; i < line.Length; i++) {
                if (Char.IsDigit(line[i])) {
                    break;
                }
                else {
                    lineStartWithThis += line[i];
                }
            }

            foreach (var useAbleData in useAbleDatas) {
                if (lineStartWithThis == useAbleData) {
                    return true;
                }
            }

            return false;
        }

        //a luxscan fájlban active-e ez a sor
        public bool IsActive(string line)
        {
            string[] lineDatas = line.Split(',');
            if (lineDatas[7].EndsWith("L")) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
