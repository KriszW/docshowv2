using IOs;
using System;
using System.Collections.Generic;

namespace DocsShowServer
{
    class MakeLinesFromData
    {
        //a kilokomodel listájának a kilőkőinek a cikkjeit frissíti
        public void SetLineModel(List<string> kilokok)
        {


            //az elöző modellek elmentése vizsgálatra
            Array.Copy(sourceArray: CommonDatas.Kilokok, destinationArray: OperationModel.SendingOperations.PrevModels, length: CommonDatas.Kilokok.Length);

            //előszőr törli az összeset
            DataOperations.DeleteAllCikkFromModels();

            //beállítja a luxscan file elérési útját
            LuxScanFileGetter.SetLuxscanFilePath();

            //kiolvassa a luxscan adatokat
            List<string> querys = FileOperations.Read(CommonDatas.LuxscanFilePath);

            //ha valami hiba miatt nullt add vissza akkor
            if (querys==null)
            {
                //addig próbálja beolvasni amíg nem sikerült, közben hibát ír ki
                DataChecking.CheckTheExcelIsOpened(CommonDatas.LuxscanFilePath,true);

                //majd az adatt újraolvasássa
                querys = FileOperations.Read(CommonDatas.LuxscanFilePath);

                if (querys==null)
                {
                    return;
                }
            }
            //beállítja az activeindexeket
            List<Tuple<int, string>> ActiveIndexesWithKiloko = OperationModel.DataManipulating.GetActiveIndexes(querys);

            //az új activeindexeket egy int listává konvertálja
            List<int> activeIndexes = new List<int>();

            foreach (var item in ActiveIndexesWithKiloko) {
                activeIndexes.Add(item.Item1);
            }

            //beállítja az új adatokat a megadott argumentumok alapján
            OperationModel.ItemOperations.GetNewItemsToModel(querys,kilokok,ActiveIndexesWithKiloko,activeIndexes);
        }
    }
}
