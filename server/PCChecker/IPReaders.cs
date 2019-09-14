using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class IPReaders
    {
        public List<string> ReadOnlineIPs()
        {
            List<string> ips = new List<string>();

            foreach (var item in Server.Clients)
            {
                ips.Add(item.ClientIP.ToString());
            }

            return ips;
        }

        public List<string> ReadAllIPs()
        {
            //a machine adatok betöltése
            List<string> machines = MachineDataMethods.GetRawGepInfos();

            HashSet<string> ips = new HashSet<string>();

            foreach (var line in machines)
            {
                string[] splitter = line.Split(';');

                ips.Add(splitter[1]);
            }

            return ips.ToList();
        }
    }
}
