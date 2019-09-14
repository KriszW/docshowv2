using KilokoModelLibrary;
using System.Collections.Generic;
using System.Threading;

namespace DocsShowServer
{
    public class CommonDatas
    {
        public static int NetworkSpareLevel { get; set; }

        public static bool HasLuxscanFile { get; set; }

        public static int LuxscanUpdateRate { get; set; }

        //a luxscan fájl helye
        public static string LuxscanDataPath { get; set; }
        //a info thread, ahol a frissítés történik
        public static Thread GetInfos { get; set; }
        //a pdfek helye
        public static string PathtoResources { get; set; }
        //a cikkeket tartalmazó fájl helye
        public static string PathtoCikkek { get; set; }

        public static string MarkBetweenTwoCikk { get { return "&&"; } }

        public static IKilokoModel[] Kilokok { get; set; }

        public static string LuxscanFilePath { get; set; }

        public static Tables GUI { get; set; }

        public static string GepInfoPath { get { return "gépek.csv"; } }

        public static string CodeTablePath { get { return "CodeTable.csv"; } }

        public static List<string> GepInfoData { get; set; } = new List<string>();

        public static List<string> PDFDatas { get; set; } = new List<string>();

    }
}
