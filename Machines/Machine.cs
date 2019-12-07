using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Machines
{
    public class Machine
    {
        public static List<Machine> Machines { get; set; }

        public static IEnumerable<Machine> Configure(string path)
        {
            if (path.EndsWith(".json"))
            {
                var text = System.IO.File.ReadAllText(path);
                return new JavaScriptSerializer().Deserialize<List<Machine>>(text);
            }
            else
            {
                throw new ApplicationException("A konfigurációs fájlnak JSON-nek kell lennie");
            }
        }
        public Machine(string iD, int monitorID, string kilokoNum)
        {
            IP = iD ?? throw new ArgumentNullException(nameof(iD));
            MonitorIndex = monitorID;
            KilokoNum = int.Parse(kilokoNum);
        }

        public string IP { get; private set; }
        public int MonitorIndex { get; private set; }
        public int KilokoNum { get; private set; }
        public List<IKilokoModel> Kiloko { get; set; } 

        public bool IsSame(Machine other)
        {
            return other.MonitorIndex == this.MonitorIndex && other.IP == this.IP;
        }
    }
}