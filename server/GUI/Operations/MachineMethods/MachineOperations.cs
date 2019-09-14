using IOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DocsShowServer
{
    class MachineOperations
    {
        public static void Restart(string asztalszam = "")
        {
            RestartOperations restart = new RestartOperations();

            List<string> ips = MachineDataMethods.GetIPs(asztalszam);

            restart.SendRestart(ips, true);
        }

        public static void Shutdown(string asztalszam = "")
        {
            List<string> ips = MachineDataMethods.GetIPs(asztalszam);

            ShutdownOperations.ShutdownByIP(ips);
        }

        public static void StartAll()
        {
            //az ébresztő fájl helye
            string path = "TC_Wakeup.cmd";

            try
            {
                //ha létezik
                if(File.Exists(path))
                {
                    //indítsa el
                    Process.Start(path);

                    string logText = $"Az összes gépet bekapcsolták";

                    Logger.MakeLog(logText);
                    Console.WriteLine($"{DateTime.Now.ToString()}:{logText}");
                }
                else
                {
                    //logolja a hiányt
                    Logger.MakeLog($"Nem létezik a {path} indító cmd fájl");
                    Console.WriteLine($"{DateTime.Now.ToString()}:Nem létezik a {path} indító cmd fájl");
                }
            }
            catch(Exception)
            {
                //loggolja a hiányt
                Logger.MakeLog($"Nem sikerült elindítani a {path} indító cmdt");
                Console.WriteLine($"{DateTime.Now.ToString()}:Nem sikerült elindítani a {path} indító cmdt");
            }
        }
    }
}
