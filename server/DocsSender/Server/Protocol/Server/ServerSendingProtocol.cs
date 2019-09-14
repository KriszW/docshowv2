using System;
using System.IO;
using System.Diagnostics;

namespace DocsShowServer
{
    class ServerSendingProtocol
    { 
        public static void SendDatas(Client client, string[] gepRaw, string[] pdfRaw)
        {
            if (client==null)
            {
                return;
            }

            SendingCheckings.MakeChecks(client,pdfRaw);

            //stopper
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                //a sending datas beállítása
                client.SendingDatas = true;

                //a paraméterek elküldése
                SendingDataMethods.SendParams(client, gepRaw, pdfRaw);

                //küldje el a pdf infókat
                SendingDataMethods.SendPDFDatas(client, pdfRaw);

                //küldje el a protokol végét
                SendingDataMethods.SendEndOfData(client);

                //a sending datas falsera beállítása
                client.SendingDatas = false;
            }
            catch (ApplicationException)
            {

            }

            stopwatch.Stop();

            //a stopper adatainak leállítása
            Console.WriteLine($"{DateTime.Now.ToString()}:A dokumentomok elküldésének az ideje {stopwatch.ElapsedMilliseconds} ezred másodperc volt a {client.ClientIP.ToString()} kliensre");
        }
    }
}
