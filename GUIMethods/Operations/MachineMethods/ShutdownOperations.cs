using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace DocsShowServer
{
    class ShutdownOperations
    {
        static string PsExecLocation = ConfigurationManager.AppSettings["PsExecLocation"].ToString();
        static string ProcFilePath = "shutdown.bat";

        static List<string> GetIPs(List<string> rawData)
        {
            HashSet<string> output = new HashSet<string>();

            //menjen végig az egészen
            foreach (var item in rawData)
            {
                //darabolja fel az itemt
                string[] gepRaws = item.Split(';');

                output.Add(gepRaws[1]);
            }

            return output.ToList();
        }

        static void SetUpFile(List<string> ips)
        {
            string shutdowncommand = @"C:\Windows\System32\shutdown.exe /p /f";

            List<string> rows = new List<string>();

            foreach (var ip in ips)
            {
                string line = $"{PsExecLocation} \\\\{ip} -u Admin -p Admin {shutdowncommand}";

                rows.Add(line);

                string logText = $"A {ip} klienst manuálisan leállították";

                IOs.Logger.MakeLog(logText);
                Console.WriteLine(logText);
            }

            if (rows.Count > 0)
            {
                System.IO.File.WriteAllLines(ProcFilePath, rows);
            }
        }

        static void StartFile()
        {
            ProcessStartInfo shutdownProcInfo = new ProcessStartInfo()
            {
                FileName = ProcFilePath,
                WorkingDirectory = System.IO.Directory.GetCurrentDirectory(),
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process shutdownProc = Process.Start(shutdownProcInfo);

            shutdownProc.WaitForExit();

            System.IO.File.Delete(ProcFilePath);
        }

        public static void ShutdownByIP(List<string> ips)
        {
            SetUpFile(ips);

            StartFile();
        }

        
    }
}
