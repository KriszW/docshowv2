using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositioningLib
{
    public class Datas
    {
        public static string ZoomScale { get; set; }
        public static string PathToPDFReader { get; set; }
        public static string PDFsPath { get; set; }
        public static int WaitingTime { get; set; }
        public static string ServerIP { get; set; }
        public static int CountOfMonitors { get; set; }
        public static ushort Port { get; set; }
        public static MonitorInfo[] MonitorInfos { get; set; }
    }
}
