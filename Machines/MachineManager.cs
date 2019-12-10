using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Machines
{
    public static class MachineManager
    {
        public const string ShutdownPath = "shutdown.bat";
        public const string StartPath = "TC_Wakeup.cmd";
        public const string RestartPath = "restart.bat";

        public static string PsExecLocation {get; set;} = "PsExec.exe";

        private static void SetUpShFile(IEnumerable<Machine> machines)
        {
            var shutdowncommand = @"C:\Windows\System32\shutdown.exe /p /f";

            var rows = GetLines(machines,shutdowncommand);

            if (rows.Count > 0)
            {
                System.IO.File.WriteAllLines(ShutdownPath, rows);
            }
        }

        private static void SetUpRsFile(IEnumerable<Machine> machines)
        {
            var restartcommand = @"C:\Windows\System32\shutdown.exe /r /f";

            var rows = GetLines(machines, restartcommand);

            if (rows.Count > 0)
            {
                System.IO.File.WriteAllLines(RestartPath, rows);
            }
        }

        private static List<string> GetLines(IEnumerable<Machine> machines, string command)
        {
            var rows = new List<string>();

            foreach (var machine in machines)
            {
                string line = $"{PsExecLocation} \\\\{machine.IP} -u Admin -p Admin {command}";

                rows.Add(line);
            }

            return rows;
        }

        public static void Start(string asztal)
        {
            Process.Start(StartPath);
        }

        public static void Shutdown(string asztal)
        {
            var machines = GetMachinesByTable(asztal);

            SetUpShFile(machines.Distinct());

            Process.Start(ShutdownPath);
        }

        private static IEnumerable<Machine> GetMachinesByTable(string asztal)
        {
            if (asztal == "")
            {
                return Machine.Machines.Distinct();
            }

            var machines = new List<Machine>();

            foreach (var item in Machine.Machines)
            {
                if (item.KilokoNum.ToString() == asztal)
                {
                    machines.Add(item);
                }
            }

            return machines.Distinct();
        }

        public static void Restart(string asztal)
        {
            var machines = GetMachinesByTable(asztal);

            SetUpRsFile(machines.Distinct());

            Process.Start(RestartPath);
        }
    }
}