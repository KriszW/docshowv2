using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LuxScanOrdReader
{
    public class FileReader
    {
        public FileInfo OrderFile { get; set; }

        public event OnLuxScanFileCopyError FileError;

        public event OnSuccessFileCopy CopySucceeded;

        //az order fájl lemásolása
        public void CopyOrderFile()
        {
            //egy startinfo beállítása a megfelelő paraméterekkel
            var copyInfo = new ProcessStartInfo()
            {
                FileName = "Copy_ord.cmd",
                WorkingDirectory = Directory.GetCurrentDirectory(),
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            //a process inizialicálása
            var copyProcess = new Process()
            {
                StartInfo = copyInfo,
                EnableRaisingEvents = true
            };

            try
            {
                //a process elindítása
                copyProcess.Start();

                //megvárja, hogy a másolás kilépjen
                copyProcess.WaitForExit();

                CheckOrdFileCount();
            }
            catch (ApplicationException)
            {
            }
            catch (Exception ex)
            {
                var logText = $"Nem található a Copy_ord.cmd fájl";

                //valószínűleg nem található a Copy_ord.cmd fájl
                //Logger.MakeLog($"{logText} hibakód: {ex.Message}");
                Console.WriteLine($"{DateTime.Now.ToString()}: {logText}");
            }
        }

        private void CheckOrdFileCount()
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).Where(f => f.EndsWith(".ord")).ToList();

            if (files.Count == 1)
            {
                var newFile = new FileInfo(files[0]);
                CopySucceeded?.Invoke(this, new SuccessFileCopyArgs(newFile));
            }
            else
            {
                FileError?.Invoke(this, new FileCountArgs(files.ToArray(), files.Count));
            }
        }
    }
}