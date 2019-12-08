using ItemNumberManager;
using System;

namespace SendedModels
{
    [Serializable]
    public class PDFModelOverTCP
    {
        public PDFModelOverTCP(MonitorPosition position)
        {
            Position = position;
        }
        public PDFModelOverTCP(string fileName, MonitorPosition position) : this(position)
        {
            PDFFileName = fileName;
            //Data = System.IO.File.ReadAllBytes(fileName);
        }

        public PDFModelOverTCP(string fileName, byte[] data, MonitorPosition position) : this(position)
        {
            PDFFileName = fileName;
            Data = data;
        }

        public MonitorPosition Position { get; set; }
        public int MonitorIndex { get; set; } = -1;
        public string PDFFileName { get; set; }
        public string PDFName { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public byte[] Data { get; set; }
    }
}