using System;

namespace DocsShowServer
{
    static class SendingCheckings
    {
        public static void CheckPDFInfos(Client client, string docsName)
        {
            //vár a válaszra
            string msg = client.Reader.ReadMSG();

            //ha nem /okDocs
            if (!msg.Contains("/okDocs"))
            {
                client.SendingDatas = false;
                throw new ApplicationException($"A {client.ClientIP.ToString()} kliens visszautasította a dokumentum nevének elfogadását, egy érvénytelen dokumentum név lett elküldve: \"{docsName}\"!");
            }
        }

        public static void CheckPDFGetted(Client client, string docsName)
        {
            //várj a válaszra
            string msg = client.Reader.ReadMSG();

            //ha nem /okDoc
            if (!msg.Contains("/okDoc"))
            {
                client.SendingDatas = false;
                throw new ApplicationException($"A kliens visszautasította a PDF adatok teljességének elfogadását a \"{docsName}\" doksinál!");
            }
        }

        public static void MakeChecks(Client client, string[] pdfRaw)
        {
            //a fájlok létezését vizsgálja
            FileChecking.CheckFileExists(pdfRaw);

            //ha most éppen adatokat küldenek, akkor azt megvárja és csak utána küldi el
            ClientOperations.WaitForEndDataSending(client);
        }
    }
}
