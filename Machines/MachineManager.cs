using System;
using System.Diagnostics;

namespace Machines
{
    public static class MachineManager
    {
        public const string ShutdownPath = "shutdown.bat";
        public const string StartPath = "TC_Wakeup.cmd";
        public const string RestartPath = "restart.bat";

        public static void Start()
        {
            Process.Start(StartPath);
        }

        public static void Shutdown()
        {
            Process.Start(ShutdownPath);
        }

        public static void Restart()
        {
            Process.Start(RestartPath);
        }
    }
}