using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class DocsDataOperations
    {
        public static string[] GetDatasSplitter()
        {
            //az alap infók megkapása
            string msg = ReadingMethods.ReadMSG();

            if (msg=="" || msg == "/non")
            {
                return null;
            }

            string[] splitter = msg.Split(';');

            if (splitter.Length!=3 && splitter[0] != "/non")
            {
                SendingOperations.WriteMSG("/InvalidDatas");

                throw new ApplicationException($"A szerver nem létező adatokat küldőtt! Az asztalszáma;monitorszáma;cikkszám helyett: \"{msg}\"!");
            }

            DocsShowClient.InPDF = true;

            return splitter;
        }

        public static int GetMonitorIndex(string[] splitter)
        {
            //a monitorindex beállítása, ha érvénytelen az egész újrakezdése
            int monitorIndex = 0;
            try
            {
                monitorIndex = int.Parse(splitter[1]);
            }
            catch(Exception)
            {
                SendingOperations.WriteMSG("/InvalidDatas");

                throw new ApplicationException($"A szerver érvénytelen adatot küldőtt a monitorszámra: \"{splitter[1]}\"!");
            }

            Console.WriteLine($"{DateTime.Now.ToString()}:Monitorindex: { monitorIndex + 1 } ");

            //a fejléc adatokat sikeresen megkaptam (a asztalszám,monitorszám,cikkszám)
            SendingOperations.WriteMSG("/okDatas");

            return monitorIndex;
        }

        public static void SendEndOfDocs()
        {
            //a müvelet végének elküldése
            string msg = ReadingMethods.ReadMSG();

            SendingOperations.WriteMSG("/ok");

            DocsShowClient.InPDF = false;
        }
    }
}
