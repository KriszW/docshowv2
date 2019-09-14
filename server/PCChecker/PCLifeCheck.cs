using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DocsShowServer
{
    class PCLifeCheck
    {
        IPReaders IPReader { get; set; }    
        RestartOperations RestartOperations { get; set; }

        public bool IsChecking { get; private set; }

        public PCLifeCheck()
        {
            IPReader = new IPReaders();
            RestartOperations = new RestartOperations();
        }

        public void Check()
        {
            IsChecking = true;

            List<string> allIP = IPReader.ReadAllIPs();
            List<string> onlineIPs = IPReader.ReadOnlineIPs();

            List<string> difference = allIP.Except(onlineIPs).ToList();

            if (difference.Count>0)
            {
                try
                {
                    RestartOperations.SendRestart(difference);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}Hiba lépett fel az újraindításnál, ezért nem lettek újra indítva a gépek");
                    Logger.MakeLog($"Hiba lépett fel az újraindításnál: {ex.Message}");
                }

            }

            IsChecking = false;
        }

        
    }
}
