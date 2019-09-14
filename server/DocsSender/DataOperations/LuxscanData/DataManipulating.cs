using IOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DocsShowServer
{
    class DataManipulating
    {
        public List<string> CodeTable { get; set; } = new List<string>();

        private DataChecking DataChecking { get; set; }

        public DataManipulating(DataChecking cheking)
        {
            DataChecking = cheking;
        }
        
        //az order fájl lemásolása
        public void CopyOrderFile()
        {
            //egy startinfo beállítása a megfelelő paraméterekkel
            ProcessStartInfo copyInfo = new ProcessStartInfo()
            {
                FileName = "Copy_ord.cmd",
                WorkingDirectory = System.IO.Directory.GetCurrentDirectory(),
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            //a process inizialicálása
            Process copyProcess = new Process()
            {
                StartInfo=copyInfo,
                EnableRaisingEvents=true
            };

            try
            {
                //a process elindítása
                copyProcess.Start();

                //megvárja, hogy a másolás kilépjen
                copyProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                string logText = $"Valószínűleg nem található a Copy_ord.cmd fájl";

                //valószínűleg nem található a Copy_ord.cmd fájl
                Logger.MakeLog($"{logText} hibakód: {ex.Message}");
                Console.WriteLine($"{DateTime.Now.ToString()}: {logText}");
            }
        }

        //a hosszú sorból a item számának meghatározása
        public string GetItemNum(string itemline)
        {
            //az indexRaw amit tükrőzve kapunk meg
            string indexRaw = "";
            //az index amit visszadunk
            string index = "";
            //a végéről indulunk, mert a végén van az index, ezért gyorsabb
            for (int i = itemline.Length - 1; i > 0; i--)
            {
                //ha szám akkor adjuk hozzá az indexRawhoz
                if(char.IsDigit(itemline[i]))
                {
                    indexRaw += itemline[i];
                }
                else
                {
                    break;
                }
            }

            //az indexRaw tükrőzése
            for(int i = indexRaw.Length - 1; i >= 0; i--)
            {
                index += indexRaw[i];
            }

            return index;
        }

        public string GetAsztalSzama(string line)
        {
            string path = CommonDatas.CodeTablePath;

            //a code lekérése a sorból
            string codeValue = line.Split(',')[15];

            //a codetábla beolvasása
            List<string> codeTable = FileOperations.Read(path);

            //ha nem sikerül akkor addig amíg nem sikerül
            //utána újraolvasás
            if (codeTable == null && codeTable.Count==0) {
                DataChecking.CheckTheExcelIsOpened(path, true);

                codeTable = FileOperations.Read(path);
            }

            if (codeTable!=null)
            {
                CodeTable = codeTable;
            }
            else
            {
                DataChecking.CheckTheExcelIsOpened(path, false);
            }

            //amit a függvény visszaadd értékként
            string kilokoSzam = "";

            //végig megyünk a codetáblán
            foreach (var item in CodeTable)
            {
                //a mit beolvastunk code azt feldaraboljuk
                //adatstruktúra: code;kombináció1;kombináció2;kombináció3

                string[] splitter = new string[0];

                if (item.Contains(";"))
                {
                    splitter = item.Split(';');
                }
                else if (item.Contains("\t"))
                {
                    splitter = item.Split('\t');
                }

                //ha a code megegyezik akkor jó
                if (splitter[0] == codeValue)
                {
                    //a 2. elemtől indulunk, mert az 1. a code értéke
                    for (int i = 1; i < splitter.Length; i++)
                    {
                        //ha az 0, akkor ugorjunk tovább
                        if (splitter[i] == "0")
                        {
                            continue;
                        }

                        //ha nem üres akkor adjuk hozzá
                        //a separátór ';' karakter is hozzáadásra kerül
                        if (splitter[i] != "")
                        {
                            kilokoSzam += splitter[i] + ";";
                        }
                    }
                }
            }

            //ha a kiokoszam hossza kevesebb mint 2, tehát nem talált semmit
            //akkor üres stringet adunk vissza
            if (kilokoSzam.Length < 2)
            {
                return "";
            }

            //a kilokoszam vissza adása viszont levágjuk a végéről a nem szükséges ';' karaktert
            return kilokoSzam.Remove(kilokoSzam.Length - 1, 1);
        }

        //az active indexek kiolvasása kilőkőkkel
        public List<Tuple<int, string>> GetActiveIndexes(List<string> query)
        {
            //az output inicializálása
            List<Tuple<int, string>> ActiveIndexes = new List<Tuple<int, string>>();

            //a queryn végigmenni
            foreach (var item in query)
            {
                //ha nem üres, és betűvel kezdődik, és használható sor akkor mehet tovább
                if(!OperationModel.Waiting.StepForwardIfOk(item))
                {
                    continue;
                }

                //ha cikk sor akkor tovább
                if (item.StartsWith("ItemName"))
                {
                    continue;
                }

                //ha active sor
                if (DataChecking.IsActive(item.Split('=')[1]))
                {
                    //az index kiolvasása
                    int index = int.Parse(GetItemNum(item.Split('=')[0]));
                    //a kilőkőszám kiolvasása
                    string KilokoKod = GetAsztalSzama(item);

                    //ha kilőkőkod üres, akkor tovább
                    if (KilokoKod=="")
                    {
                        continue;
                    }

                    //hozzáadás az outputhoz mind az item indexet és a kilőkőkódot is
                    ActiveIndexes.Add(Tuple.Create(index, KilokoKod));
                }
            }

            return ActiveIndexes;
        }
    }
}
