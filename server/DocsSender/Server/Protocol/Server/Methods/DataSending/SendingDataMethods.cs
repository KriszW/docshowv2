using System;
using System.IO;

namespace DocsShowServer
{
    class SendingDataMethods
    {
        //a pdf adatok elküldése a megadott kliensre
        public static void SendPDFDatas(Client client, string[] pdfRaw)
        {
            //addig menjen amíg van pdf
            for (int i = 1; i < pdfRaw.Length; i++)
            {
                byte[] datasOfFile = null;

                string docsName = pdfRaw[i];

                if (docsName == "")
                {
                    //ha a pdf üres akkor a /empty-t küldje el
                    docsName = "/empty";
                }
                else
                {
                    //ha nem üres akkor pedig olvassa be a fájl adatokat
                    string[] splitterForPage = docsName.Split('#');
                    datasOfFile = File.ReadAllBytes(CommonDatas.PathtoResources + splitterForPage[0] + ".pdf");
                }

                //küldje el a doksi nevét vagy azt hogy /empty
                client.Sender.SendMSG(docsName);

                //nézze meg hogy jól kapta-e meg a kliens
                SendingCheckings.CheckPDFInfos(client, docsName);

                //ha nem üres a fájl akkor küldje el a bájtjait
                if (datasOfFile != null)
                {
                    client.Sender.SendData(datasOfFile);
                }

                //ha empty volt a fájl akkor állítson be olvasható adatot
                if (docsName == "/empty")
                {
                    docsName = "Nincs semmi#1";
                }
                else
                {
                    SendingCheckings.CheckPDFGetted(client, docsName);
                }
            }
        }

        //elküldi a kliensnek a paraméterket
        //struktura: asztalszáma;monitorszáma;cikkszám
        public static void SendParams(Client client, string[] gepRaw, string[] pdfRaw)
        {
            string line = $"{gepRaw[0]};{gepRaw[2]};{pdfRaw[0]}";

            //elküldi az összetett adatot
            client.Sender.SendMSG(line);

            //a válasz várása
            string msg = client.Reader.ReadMSG();

            //ha nem jók voltak az adatok akkor
            if (msg != "/okDatas")
            {
                client.SendingDatas = false;
                throw new ApplicationException($"A {gepRaw[1]} kliens érvénytelen adatokat kapott az asztalszáma;monitorszáma;cikkszám helyett: {line}!");
            }
        }

        public static void SendEndOfData(Client client)
        {
            //küldje el a protokol végét
            client.Sender.SendMSG("/endOfDatas");

            //várja meg a választ
            string msg = client.Reader.ReadMSG();

            //ha nem jó akkor írja ki miért nem
            if (msg != "/ok")
            {
                client.SendingDatas = false;
                throw new ApplicationException($"A {client.ClientIP.ToString()} kliens visszautasította a kommunikáció lezárását: {msg}");
            }
        }
    }
}
