using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LuxScanOrdReader
{
    public class FileReader
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public FileInfo OrderFile { get; set; }

        public event OnLuxScanFileCopyError FileError;

        public event OnSuccessFileCopy CopySucceeded;

        //az order fájl lemásolása
        public void CopyOrderFile()
        {
            _logger.Debug($"A másoló script inicializálása...");
            //egy startinfo beállítása a megfelelő paraméterekkel
            var copyInfo = new ProcessStartInfo()
            {
                FileName = "Copy_ord.cmd",
                WorkingDirectory = Directory.GetCurrentDirectory(),
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            _logger.Debug($"A másoló script inicializálása kész a {copyInfo.FileName} scriptre");

            _logger.Debug($"A másoló script process inicializálása...");
            //a process inizialicálása
            var copyProcess = new Process()
            {
                StartInfo = copyInfo,
                EnableRaisingEvents = true
            };
            _logger.Debug($"A másoló script process inicializálása kész");

            try
            {
                _logger.Debug($"A másoló script indítása");
                //a process elindítása
                copyProcess.Start();

                _logger.Debug($"A másoló script lefutássának befejezésének megvárása");
                //megvárja, hogy a másolás kilépjen
                copyProcess.WaitForExit();

                _logger.Debug($"A másoló script eredményének kezelése");
                CheckOrdFileCount();
            }
            catch (ApplicationException ex)
            {
                _logger.Error("A másolás közben váratlan hiba lépett fel",ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"A másolás közben váratlan hiba lépett fel, lehet nem található a {copyInfo.FileName} fájl", ex);
            }
        }

        private void CheckOrdFileCount()
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).Where(f => f.EndsWith(".ord")).ToList();

            if (files.Count == 1)
            {
                _logger.Debug($"A másolás sikeres volt, a feldolgozás elindítása");
                var newFile = new FileInfo(files[0]);
                CopySucceeded?.Invoke(this, new SuccessFileCopyArgs(newFile));
            }
            else
            {
                _logger.Error($"A másolás végeredménye használhatatlan, mert {files.Count} fájl lett elmásolva az 1 helyett");
                FileError?.Invoke(this, new FileCountArgs(files.ToArray(), files.Count));
            }
        }
    }
}