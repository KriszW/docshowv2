using System;
using System.Threading;
using IOs;
using System.Configuration;

namespace DocsShowServer
{
    public class Infos
    {
        //az adatok inicializálása
        public static void InIt()
        {
            //luxscanfile igazra
            CommonDatas.HasLuxscanFile = true;

            //a luxscan frissítési időt beállítani
            bool result= int.TryParse(ConfigurationManager.AppSettings["MakeLuxscanUpdateRate"], out int tmpRate);

            //ha result akkor ha true
            //akkor tmpRate
            //ha false
            //60
            CommonDatas.LuxscanUpdateRate = result ? tmpRate : 60;
        }

        public static void ReadInfos()
        {
            while (true)
            {

                try
                {

                    //várjon addig amíg nem mehet tovább
                    if (OperationModel.Waiting.ContinueWaiting())
                    {
                        continue;
                    }

                    //a pdfek elküldése
                    OperationModel.SendingOperations.SendOutDatas();

                    //ennyit fog várni a következő lekérdezéssel
                    Thread.Sleep(CommonDatas.LuxscanUpdateRate * 1000);
                }
                catch (NullReferenceException)
                {

                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:{ex.Message} paraméter neve: {ex.ParamName}");
                    Logger.MakeLog(ex.Message + "paraméter neve: " + ex.ParamName);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:{ex.Message} paraméter neve: {ex.ParamName}");
                    Logger.MakeLog(ex.Message + "paraméter neve: " + ex.ParamName);
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:{ex.Message}");
                    Logger.MakeLog(ex.Message, 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}: Ismeretlen hiba lépett fel a doksik küldése közben");
                    Logger.MakeLog($"Ismeretlen hiba lépett fel a doksik küldése közben: {ex.Message}");
                }
                Thread.Sleep(1000);
            }
        }
    }
}