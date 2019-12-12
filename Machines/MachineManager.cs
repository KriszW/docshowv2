using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Machines
{
    public static class MachineManager
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string ShutdownPath = "shutdown.bat";
        public const string StartPath = "TC_Wakeup.cmd";
        public const string RestartPath = "restart.bat";

        public static string PsExecLocation {get; set;} = "PsExec.exe";

        private static void SetUpShFile(IEnumerable<Machine> machines)
        {
            var shutdowncommand = @"C:\Windows\System32\shutdown.exe /p /f";

            _logger.Debug($"A gépekhez a kikapcsolo script létrehozássa...");
            var rows = GetLines(machines,shutdowncommand);
            _logger.Debug($"A gépekhez a kikapcsolo script létrehozva");

            if (rows.Count > 0)
            {
                try
                {
                    _logger.Debug($"A gépekhez a kikapcsoló script létrehozássa..");
                    System.IO.File.WriteAllLines(ShutdownPath, rows);
                    _logger.Debug($"A gépekhez a kikapcsoló script létrehozva");
                }
                catch (Exception ex)
                {
                    _logger.Error($"A {ShutdownPath} kikapcsoló script létrehozása közben váratlan hiba lépett fel", ex);
                }
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
                _logger.Debug($"A {machine.IP} hozzáadása a scripthez, a {command} parancsal");
                string line = $"{PsExecLocation} \\\\{machine.IP} -u Admin -p Admin {command}";

                rows.Add(line);
            }

            return rows;
        }

        public static void Start(string asztal)
        {
            try
            {
                _logger.Debug($"A gép indító script indítása...");
                Process.Start(StartPath);
                _logger.Debug($"A gép indító script sikeresen lefutott");
            }
            catch (Exception ex)
            {
                _logger.Error($"A gép indító script közben váratlan hiba lépett fel",ex);
            }
        }

        public static void Shutdown(string asztal)
        {
            try
            {
                var machines = GetMachinesByThisTable(asztal);

                _logger.Debug($"Az {asztal} asztalhoz a kikapcsoló script létrehozássa...");
                SetUpShFile(machines.Distinct());
                _logger.Debug($"Az {asztal} asztalhoz a kikapcsoló script létrehozva");
            }
            catch (Exception ex)
            {
                _logger.Error($"A gép leállító inditó script létrehozása közben váratlan hiba lépett fel", ex);
            }

            try
            {
                Process.Start(ShutdownPath);
            }
            catch (Exception ex)
            {
                _logger.Error($"A gép leálító script közben váratlan hiba lépett fel", ex);
            }
        }

        private static IEnumerable<Machine> GetMachinesByThisTable(string asztal)
        {
            _logger.Debug($"Az {asztal} asztalhoz a gépek lekérése...");
            var machines = GetMachinesByTableData(asztal);
            _logger.Debug($"Az {asztal} asztalhoz a gépek lekérve");
            return machines;
        }

        private static IEnumerable<Machine> GetMachinesByTableData(string asztal)
        {
            if (asztal == "")
            {
                _logger.Debug($"Mivel nem volt meghatározva az asztal szám, ezért az összes asztalt visszaadjuk");
                return Machine.Machines.Distinct();
            }

            var machines = new List<Machine>();

            foreach (var item in Machine.Machines)
            {
                if (item.KilokoNum.ToString() == asztal)
                {
                    _logger.Debug($"A {item.IP} van hozzárendelve az {asztal} asztalhoz");
                    machines.Add(item);
                }
            }

            return machines.Distinct();
        }

        public static void Restart(string asztal)
        {
            try
            {
                var machines = GetMachinesByThisTable(asztal);

                _logger.Debug($"Az {asztal} asztalhoz az újraindító script létrehozássa...");
                SetUpRsFile(machines.Distinct());
                _logger.Debug($"Az {asztal} asztalhoz az újraindító script létrehozva");
            }
            catch (Exception ex)
            {
                _logger.Error($"A gép újra inditó script létrehozása közben váratlan hiba lépett fel", ex);
            }

            try
            {
                Process.Start(RestartPath);
            }
            catch (Exception ex)
            {
                _logger.Error($"A gép újra inditó script közben váratlan hiba lépett fel", ex);
            }
        }
    }
}