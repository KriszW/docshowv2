using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Reflection;
using File = System.IO.File;

namespace InItClientProgram
{
    public class ShortcutOperations
    {
        private static void CreateShortcut()
        {
            //a shortcut beállításai és létrehozzása
            string shortcutTargetLoc = Directory.GetCurrentDirectory() + "\\" + Assembly.GetExecutingAssembly().FullName.Split(',')[0] + ".exe";

            WshShell wshShell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\KijelzoClient.lnk");

            shortcut.WorkingDirectory = Directory.GetCurrentDirectory();
            shortcut.Description = "Parancsikon a kliens automata inditásához";
            shortcut.TargetPath = shortcutTargetLoc;
            shortcut.Save();
        }

        public static void SetStartUp()
        {
            //ha nincs shortcut akkor létrehozza
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\KijelzoClient.lnk"))
            {
                CreateShortcut();
            }
        }
    }
}