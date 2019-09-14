using IOs;
using System;

namespace DocsShowServer
{
    class InfoChecker
    {
        //megnézzi, hogy a gépekből nem-e jött vissza fake adat
        public static bool ValidMachine(string[] gepRaw,string asztalszam)
        {
            if (gepRaw == null) {
                Console.WriteLine($"{ DateTime.Now.ToString()}: Az asztalszámhoz nincs gép társítva: " + asztalszam);
                Logger.MakeLog($"Az asztalszámhoz nincs gép társítva: {asztalszam}");
                return false;
            }
            return true;
        }

        //megnézzi, hogy a pdfekből nem-e jött vissza fake adat
        public static string[] ValidInfos(string ip, string asztalszam, string cikk, string[] pdfRaw)
        {
            if (pdfRaw == null) {
                Console.WriteLine($"{ DateTime.Now.ToString()}: A cikk számhoz nincs pdf társítva: {cikk}");
                Logger.MakeLog($"A cikk számhoz nincs pdf társítva: {cikk}");

                return new string[2] { cikk, "" };
            }

            return pdfRaw;
        }
    }
}