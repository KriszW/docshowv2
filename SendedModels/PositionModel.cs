using ItemNumberManager;
using KilokoModelLibrary;
using System;
using System.Collections.Generic;

namespace SendedModels
{
    [Serializable]
    public class PositionModel
    {
        public PositionModel(int monitorID)
        {
            MonitorIndex = monitorID;
            PDF = new List<PDFModelOverTCP>();
        }

        public int MonitorIndex { get; private set; }
        public List<PDFModelOverTCP> PDF { get; set; }
    }
}