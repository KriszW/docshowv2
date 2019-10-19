using KilokoModelLibrary;
using System;
using System.Collections.Generic;

namespace SendedModels
{
    [Serializable]
    public class PositionModel
    {
        public PositionModel(KilokoPosition postion, int monitorID)
        {
            Postion = postion;
            MonitorIndex = monitorID;
            PDF = new List<PDFModel>();
        }

        public KilokoPosition Postion { get; private set; } //bal vagy jobb
        public int MonitorIndex { get; private set; }
        public List<PDFModel> PDF { get; set; }
    }
}