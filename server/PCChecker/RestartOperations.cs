using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DocsShowServer
{
    class RestartOperations
    {
        string PsExecLocation = System.Configuration.ConfigurationManager.AppSettings["PsExecLocation"].ToString();
        string ProcFilePath = "restart.bat";

        void SetUpFile(List<string> ips, bool force = false)
        {
            string restartCommand = @"C:\Windows\System32\shutdown.exe /r /f /t 0";

            List<string> rows = new List<string>();

            string logText = "";

            foreach (var ip in ips)
            {
                string line = $"{PsExecLocation} \\\\{ip} -u Admin -p Admin {restartCommand}";

                if (force)
                {
                    rows.Add(line);

                    logText= $"A(z) {ip} manuálisan újra lett indítva";
                }
                else
                {
                    if (PingSender.IsItOn(ip))
                    {
                        rows.Add(line);

                        logText = $"A(z) {ip} klienst újra kellett indítani, mert le volt csatlakozva a szerverről, de elérhető volt";
                    }
                    else
                    {
                        //bekapcsoljuk akkor ezt a gépet, ha nem elérhető?
                        logText = $"A(z) {ip} kliens nem volt elérhető, a pingre se válaszolt, ezért nem tudtuk újraindítani";
                    }
                }


                IOs.Logger.MakeLog(logText);
                Console.WriteLine($"{DateTime.Now.ToString()}:{logText}");
            }

            if (rows.Count>0)
            {
                System.IO.File.WriteAllLines(ProcFilePath, rows);
            }
        }

        public void SendRestart(List<string> ips)
        {
            SetUpFile(ips);

            StartRestartFile();
        }

        public void SendRestart(List<string> ips,bool force)
        {
            SetUpFile(ips,force);

            StartRestartFile();
        }

        void StartRestartFile()
        {
            ProcessStartInfo restartProcInfo = new ProcessStartInfo()
            {
                FileName = ProcFilePath,
                WorkingDirectory = System.IO.Directory.GetCurrentDirectory(),
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process restartProc = Process.Start(restartProcInfo);

            restartProc.WaitForExit();

            System.IO.File.Delete(ProcFilePath);
        }
    }
}
