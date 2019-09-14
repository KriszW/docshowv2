using PositioningLib;
using System;
using System.IO;
using System.Text;

namespace Client
{
    class FileManipulationOperations
    {
        public static void DeleteFiles()
        {
            //a resources mappa fájljainak törlése
            string[] ResourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Resources");

            foreach (var item in ResourceFiles) {
                try {
                    File.Delete(item);
                }
                catch (IOException) {

                }
            }
        }

        //a pdf adatok megszerzése és aztán pedig kiírása a hddra
        static bool GetDatasAndWriteToHDD(string PDFName)
        {
            //a fájl méretének elküldése
            byte[] fileSizeBytes = new byte[4];
            int bytes = DocsShowClient.ClientStream.Read(fileSizeBytes, 0, 4);
            int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

            DocsShowClient.ClientStream.Write(Encoding.UTF8.GetBytes("/ok"), 0, 3);

            //alap maradék byteok hosszának beállítása
            int bytesLeft = dataLength;
            //egy akkora byte tömb létrehozássa mint maga a fájl a memóriában
            byte[] data = new byte[dataLength];

            //a buffersize beállítása
            int bufferSize = DocsShowClient.ClientTCP.ReceiveBufferSize;
            //a kiolvasott bájtok hosszának beállítása az elején 0
            int bytesRead = 0;

            //addig csinálja ami 0 bájt marad
            while (bytesLeft > 0) {
                //a kiolvasott adatok kivonása
                int curDataSize = Math.Min(bufferSize, bytesLeft);
                //ha a kivont kisebb minta mennyi marad akkor az az érték beállítása (buffer 0ázás elkerülése)
                if (DocsShowClient.ClientTCP.Available < curDataSize)
                    curDataSize = DocsShowClient.ClientTCP.Available;

                //az adatok olvasása
                bytes = DocsShowClient.ClientStream.Read(data, bytesRead, curDataSize);

                //az olvasott adatokhoz hozzáadás
                bytesRead += curDataSize;
                //a maradt bájtokból kivonás
                bytesLeft -= curDataSize;
            }

            try {
                //ha sikerül kiírni a bájtokat, akkor nincs nyitva tehát nincs használatban
                File.WriteAllBytes($@"Resources\{PDFName}.pdf", data);
                return false;
            }
            catch (Exception) {
                //ha bármilyen hibát dob akkor nyitva van a fájl tehát használatban van
                return true;
            }
        }

        public static void ManageDocs(string docsName, int i, int monitorIndex, string[] splitter)
        {
            //ha nyitva van éppen akkor isopened true ha nem akkor false
            bool isOpened = GetDatasAndWriteToHDD(docsName);

            if (isOpened)
            {
                //ha nem megfelelő standard van a helyén akkor lecserélés
                if (DocsShowClient.StandardsForGoodPosition[monitorIndex, i] != docsName) {
                    DocsShowClient.Standards.Add(docsName);

                    //a jó pozició beállítása
                    DocsShowClient.StandardsForGoodPosition[monitorIndex, i] = docsName;
                    //ami épp kint van annak a bezárása
                    ClosingOperations.CloseOneStandard(monitorIndex, i);
                    //és a standard megnyitása
                    Positioning.SplitForOneSide(DocsShowClient.Standards[i], i, monitorIndex, splitter[0], splitter[2]);
                }
                else
                {
                    //ha jó akkor isopened hozzáadása
                    DocsShowClient.Standards.Add("/isOpened");
                }
                //a jó pozició beállítása
                DocsShowClient.StandardsForGoodPosition[monitorIndex, i] = docsName;
            }
            else
            {
                //standardokhoz adás
                DocsShowClient.Standards.Add(docsName);
                //a jó pozició beállítása
                DocsShowClient.StandardsForGoodPosition[monitorIndex, i] = docsName;
                //kiírni hogy melyik fájlt kapta meg
                Console.WriteLine($"{DateTime.Now.ToString()}:{(i+1)}.fájl: { DocsShowClient.Standards[i] }");
                //ami éppen kint van azt bezárni
                ClosingOperations.CloseOneStandard(monitorIndex, i);
                //megnyitni a standardot
                Positioning.SplitForOneSide(DocsShowClient.Standards[i], i, monitorIndex, splitter[0], splitter[2]);
            }

            SendingOperations.WriteMSG("/okDoc");
        }
    }
}
