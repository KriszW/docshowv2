using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace PositioningLib
{
    public class ProcessOperations
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ReaderManager start

        //a reader alkalmazás elindítása, ami reader módba teszi az adobét
        public static void StartReader()
        {
            //a reader.exe helyi mappában kell hogy legyen
            ProcessStartInfo procInfo = new ProcessStartInfo
            {
                FileName = "reader.exe",
                UseShellExecute = false
            };
            Process ProcMakePos = new Process();
            try
            {
                //a program elindítása
                ProcMakePos = Process.Start(procInfo);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A redear.exe (Ami eltűnteti a felesleges dolgokat és reader módba állítja a pdfet) nem tudott elindulni");
                return;
            }
            catch (Win32Exception)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A redear.exe (Ami eltűnteti a felesleges dolgokat és reader módba állítja a pdfet) nem található");
            }
            catch (Exception)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A redear.exe (Ami eltűnteti a felesleges dolgokat és reader módba állítja a pdfet) nem tudott elindulni");
                return;
            }

            try
            {
                //ha még nem futott volna le akkor várjon
                while (!ProcMakePos.HasExited)
                {
                    Thread.Sleep(100);
                }
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
            }
        }

        #endregion ReaderManager start

        #region adobe things

        public static event EventHandler<PDFNotFoundException> OnPDFNotFound;

        //az adobéhez a az indítása argumentumok megadása
        public static string SetArguments(string fileName)
        {
            var filePath = Datas.PDFsPath + fileName;
            _logger.Info($"A {filePath} PDF kiolvasának megpróbálása");

            if (System.IO.File.Exists(filePath) == false)
            {
                _logger.Info($"A {filePath} PDF nem létezett a {Datas.PDFsPath} helyen, ezért a szervertől lekérése");
                var ex = new PDFNotFoundException($"A {filePath} PDF nem található", fileName);

                OnPDFNotFound?.Invoke(default,ex);

                if (System.IO.File.Exists(filePath) == false)
                {
                    _logger.Error($"A {filePath} PDF nem létezett a {Datas.PDFsPath} helyen, a szerver lekérése után sem",ex);
                    throw ex;
                }
            }

            _logger.Info($"{filePath} PDF sikeresen lekérve");
            return $"/n /A \"page=1&zoom={Datas.ZoomScale}&scrollbar=0&view=FitH\" \"{filePath}\"";
        }

        //az adobe program elindítása
        public static Process StartAdobe(string args)
        {
            _logger.Info($"Az PDF reader indítása a {args} argumentel");
            Process process;

            ProcessStartInfo adobeStartInfo = new ProcessStartInfo()
            {
                FileName = Datas.PathToPDFReader,
                Arguments = args,
            };

            try
            {
                _logger.Debug($"Az PDF reader indításának kisérlete a {args} argumentekkel");
                //az adobe program elindítása
                process = Process.Start(adobeStartInfo);
                _logger.Debug($"Az PDF reader indításának kisérlete a {args} argumentekkel sikeresen elindult");
            }
            catch (Exception ex)
            {
                _logger.Error($"Az PDF reader nem tudott elindulni", ex);
                return default;
            }

            //adobe megvárása amíg betölti az alap GUIját
            _logger.Debug($"Az PDF reader megvárása amíg betölti az alap guiát");
            process.WaitForInputIdle();

            _logger.Debug($"Az PDF reader elindítása után várás {Datas.WaitingTime*1000} másodpercig");
            //az elindítás után várjon ennyit, ez bevárja a lasabb gépeket
            Thread.Sleep(Datas.WaitingTime * 1000);

            return process;
        }

        #endregion adobe things
    }
}