using KilokoModelLibrary;
using System;
using System.Collections.Generic;

namespace Machines
{
    public class MachineModel
    {
        public static List<MachineModel> Machines { get; set; }

        public MachineModel(string iD, int monitorID, string kilokoNum)
        {
            IP = iD ?? throw new ArgumentNullException(nameof(iD));
            MonitorIndex = monitorID;
            KilokoNum = int.Parse(kilokoNum);
        }

        public string IP { get; private set; }
        public int MonitorIndex { get; private set; }
        public int KilokoNum { get; private set; }
        public List<IKilokoModel> Kiloko { get; set; }

        public override bool Equals(object obj)
        {
            var machineModel = (MachineModel)obj;

            return machineModel.MonitorIndex == this.MonitorIndex && machineModel.IP == this.IP;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}