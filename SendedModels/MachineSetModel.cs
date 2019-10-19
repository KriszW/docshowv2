using System;

namespace SendedModels
{
    [Serializable]
    public class MachineSetModel
    {
        public MachineSetModel(int monitorIndex, string iP, ushort port)
        {
            MonitorIndex = monitorIndex;
            IP = iP ?? throw new ArgumentNullException(nameof(iP));
            Port = port;
        }

        public int MonitorIndex { get; set; }
        public string IP { get; set; }
        public ushort Port { get; set; }
    }
}