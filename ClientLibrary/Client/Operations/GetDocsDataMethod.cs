using IOs;
using PositioningLib;
using System;

namespace Client
{
    class GetDocsDataMethod
    {
        public static void GetDocsData(int i, int monitorIndex, string[] splitter)
        {
            //a doksi nevének megszerzése
            string docsName = ReadingMethods.ReadMSG();

            PDFNameChecking.CheckIsValidPDFName(docsName);

            //ha a doksi üres akkor tovább ugrás
            if (docsName == "/empty" || docsName == "/non")
            {
                if (DocsShowClient.StandardsForGoodPosition[monitorIndex, i] != docsName)
                    ClosingOperations.CloseOneStandard(monitorIndex, i);

                DocsShowClient.Standards.Add(docsName);
                DocsShowClient.StandardsForGoodPosition[monitorIndex, i] = docsName;

                return;
            }

            try
            {
                //ha nem akkor a doksi adatok megszerzése
                FileManipulationOperations.ManageDocs(docsName, i, monitorIndex, splitter);
            }
            catch (IndexOutOfRangeException)
            {
                string msg = $"A {i}. monitor nincs bekötve, amelyre doksit akartak küldeni";

                Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
                Logger.MakeLog(msg);
            }
        }
    }
}
