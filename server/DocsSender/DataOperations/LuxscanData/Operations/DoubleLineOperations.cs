using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class DoubleLineOperations
    {
        //két cikk egy asztalra küldés
        //asztalszám;ip;monitorszám adatstruktura
        public void SendDoubleStandardToOneAsztal(string asztalszam, string[] cikkek)
        {
            //a gép infók kiolvasása
            string[] gepRaw = DataOperations.GetSearchedData(asztalszam, CommonDatas.GepInfoPath);

            //ip kiszedése az adatstrukturából
            string ip = gepRaw[1];

            //a pdfraws kiolvasása és összeállítása egy listává, két cikk alapján
            List<string[]> pdfRaws = DataOperations.MakePDFRaws(cikkek);

            //a kliens streamjének megszerzése az IP cím alapján
            Client client= ClientMethods.GetClient(ip);

            //a pdfraws adatok ellenőrzése ha megfelelő, akkor visszaadja azokat, ha nem akkor pedig mást add vissza
            for (int i = 0; i < cikkek.Length; i++) {
                pdfRaws[i]= InfoChecker.ValidInfos(gepRaw[1],asztalszam,cikkek[i],pdfRaws[i]);
            }

            //a két pdfRawból egy összetett pdfRaw állít elő, veszi a két pdfRaw első két standardját és azt állítja össze
            string[] compliedPDFRaw = DataOperations.MakeCompliedPDFRaw(pdfRaws);

            if (compliedPDFRaw==null)
            {
                throw new ApplicationException($"A PDFek kiolvasása a {asztalszam} sikertelen volt, mert nem tudta kiolvasni a {CommonDatas.PathtoCikkek} fájlból az adatokat");
            }

            //az elküldés elindítása egy másik szálon, hogy gyorsabban menjen a folyamat
            Task.Run(() => {
                    ServerSendingProtocol.SendDatas(client, gepRaw, compliedPDFRaw);
            });

            //az adatok GUIra való állítása

            if (pdfRaws[0][0]==null)
            {
                CommonDatas.GUI.DGVManager.ReplaceInfo(asztalszam, pdfRaws[0][1], pdfRaws[1][1], pdfRaws[1][0]);
            }
            else
            {
                CommonDatas.GUI.DGVManager.ReplaceInfo(asztalszam, pdfRaws[0][1], pdfRaws[1][1], pdfRaws[0][0] + CommonDatas.MarkBetweenTwoCikk + pdfRaws[1][0]);
            }
        }
    }
}
