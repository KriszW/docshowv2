using System;
using System.Threading;
using IOs;

namespace DocsShowServer
{
    class LuxScanDatas
    {
        //az adatok elküldése a klienseknek
        public static void SendDatasToClient(string cikk, string asztal)
        {
            string[] gepRaw;
            string[] pdfRaw;

            try {
                //paraméterek vizsgálása
                ParameterChecking.CheckParamValidate(cikk);
                ParameterChecking.CheckParamValidate(asztal);

                //a gepinfók beolvasása
                gepRaw = DataOperations.GetSearchedData(asztal, CommonDatas.GepInfoPath);
                //a cikk infók beolvasása
                pdfRaw = DataOperations.GetSearchedData(cikk, CommonDatas.PathtoCikkek);

                //a beolvasott adatok vizsgálata
                ParameterChecking.CheckgepInfosValidate(gepRaw, asztal);
                ParameterChecking.CheckpdfInfosValidate(pdfRaw, cikk, asztal);
            }
            catch(ApplicationException ex) {
                Console.WriteLine($"{DateTime.Now.ToString()}: {ex.Message}");
                Logger.MakeLog(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}: Ismeretlen hiba lépett fel: {ex.Message}");
                Logger.MakeLog($"Ismeretlen hiba lépett fel: {ex.Message}");
                return;
            }

            Client client = ClientMethods.GetClient(gepRaw[1]);

            //a kliens beállítása
            if (client != null)
            {
                //az adatok elküldése
                ServerSendingProtocol.SendDatas(client, gepRaw, pdfRaw);
            }

            //az adatok beállítása a GUIban
            CommonDatas.GUI.DGVManager.ReplaceInfo(asztal, pdfRaw[1], pdfRaw[2], cikk);
        }
    }
}
