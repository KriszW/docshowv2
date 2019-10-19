using System;

namespace SendedModels
{
    [Serializable]
    public class PDFModel
    {
        public PDFModel(string fileName)
        {
            PDFFileName = fileName;
            //Data = System.IO.File.ReadAllBytes(fileName);
        }

        public PDFModel(string fileName, byte[] data)
        {
            PDFFileName = fileName;
            Data = data;
        }

        public int MonitorIndex { get; set; } = -1;
        public string PDFFileName { get; set; }
        public string PDFName { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int PageNumber { get; set; } = 1;
        public byte[] Data { get; set; }
    }
}