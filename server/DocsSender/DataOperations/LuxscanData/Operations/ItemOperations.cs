using IOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocsShowServer
{
    class ItemOperations
    {
        string MarkLeftSide = "b";
        string MarkRightSide = "j";

        //a kilőkő megszerzése az itemIndex alapján az ActiveIndexek közül
        string GetKiloko(List<Tuple<int, string>> ActiveIndexesWithKiloko, int itemIndex)
        {
            foreach(var activeTuple in ActiveIndexesWithKiloko)
            {
                if(itemIndex == activeTuple.Item1)
                {
                    return activeTuple.Item2;
                }
            }

            return "";
        }

        //a kilőkőmodel listának az értékeinek a frissitése
        public void GetNewItemsToModel(List<string> querys, List<string> kilokok, List<Tuple<int, string>> ActiveIndexesWithKiloko, List<int> activeIndexes)
        {
            int itemIndex = 0;

            //a queryn végig megy mégegyszer
            foreach (var item in querys)
            {
                //ha nem megfelelő sor vagy nem aktív akkor tovább lép
                if(!OperationModel.Waiting.StepForwardIfOk(item))
                {
                    continue;
                }

                //ha az item egy cikket tartalmaz, tehát a sor ItemName-el kezdődik akkor
                if (item.StartsWith("ItemName"))
                {

                    //a cikk maskjának megszerzése a kiolvasott cikk alapján
                    //pl: 110068*K
                    //pl: 110068-%
                    //pl: 110068-**
                    string cikk = CikkMethods.GetCikk(item, CikkMethods.GetCikkek());

                    //ha a cikk értelmetlen, akkor elmentése annak, hogy a cikkhez nincs cikkmask társítva
                    if(cikk=="")
                    {
                        Console.WriteLine($"{DateTime.Now.ToString()}: Nincs meghatározva ehhez a {cikk} cikkhez cikkmask a {CommonDatas.PathtoCikkek} fájlban");
                        Logger.MakeLog($"Nincs meghatározva ehhez a {cikk} cikkhez cikkmask a {CommonDatas.PathtoCikkek} fájlban");
                        continue;
                    }

                    //a kiolvasott cikk megszerzése az itemből, amit majd a GUIn fogunk megjeleníteni
                    string toGUI = item.Split('=')[1].Split(',')[0].Split('_').Last();

                    //az itemindex inté konvertálása, hogy használható legyen később feltételeknél illetve keyként
                    //meg a contains methodhoz
                    itemIndex = int.Parse(OperationModel.DataManipulating.GetItemNum(item.Split('=')[0]));

                    //ha a itemIndex szerepel az aktívak között
                    if (activeIndexes.Contains(itemIndex))
                    {
                        //a kiloko megszerzése a GetKiloko method alapján
                        string rawKiloko = GetKiloko(ActiveIndexesWithKiloko, itemIndex);

                        SetNewData(rawKiloko,kilokok,cikk,toGUI);
                    }
                }
            }
        }

        void SetNewData(string rawKiloko,List<string> kilokok,string cikk,string toGUI)
        {
            string kiloko = GetKilokoNum(rawKiloko);

            //ha a kiloko nincs benne akkor folytatódhat az olvasás
            if (kiloko == "")
            {
                return;
            }

            //ha a kiloko az egy combocode volt akkor darabolja fel
            string[] kilokokTMP = kiloko.Split(';');
            string[] rawKilokok = rawKiloko.Split(';');

            for (int i = 0; i < kilokokTMP.Length; i++)
            {
                string kilokoSzam = kilokokTMP[i];

                SetUpNewDatas(kilokoSzam, kilokok, cikk, toGUI, rawKilokok[i]);
            }
        }

        string GetKilokoNum(string raw)
        {
            string output = "";

            for (int i = 0; i < raw.Length; i++)
            {
                if (!char.IsLetter(raw[i]))
                {
                    output+=raw[i];
                }
            }

            return output;
        }

        void SetUpNewDatas(string kilokoSzam,List<string> kilokok,string cikk,string toGUI,string rawKiloko)
        {
            IKilokoModel kilokoModel = DataOperations.GetKilokoModel(kilokoSzam);

            if (kilokoModel!=null)
            {
                if (rawKiloko.Contains(MarkLeftSide))
                {
                    kilokoModel.AddNewCikk(cikk, toGUI, 0);
                }
                else if (rawKiloko.Contains(MarkRightSide))
                {
                    kilokoModel.AddNewCikk(cikk, toGUI, 1);
                }
                else
                {
                    SetNewCikksToModels(kilokok,kilokoSzam,cikk,toGUI);
                }
            }
        }

        void SetNewCikksToModels(List<string> kilokok,string kilokoSzam,string cikk, string toGUI )
        { 
            //ha a kilokok paraméter meg van határozva
            if (kilokok != null)
            {
                //ha benne van akkor frissítse azt a kilőkőmodel cikkjét
                if (kilokok.Contains(kilokoSzam))
                {
                    DataOperations.AddNewCikkToModel(cikk, toGUI, kilokoSzam);
                }
            }
            else
            {
                //ha nincs meghatározva akkor frissítse a kilőkőmodel cikket
                DataOperations.AddNewCikkToModel(cikk, toGUI, kilokoSzam);
            }
        }
    }
}
