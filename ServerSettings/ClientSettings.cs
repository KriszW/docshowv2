using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Client
{
    public class ClientSettings
    {
        public static ClientSettings CurrentSettings { get; set; }
        public string PDFReaderLoc { get; set; }
        public int WaitingTime { get; set; }
        public string PDFResourcesPath { get; set; }
        public int ZoomScale { get; set; }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
    }
}
