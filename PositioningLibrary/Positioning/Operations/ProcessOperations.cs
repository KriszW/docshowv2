using IOs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PositioningLib
{
    class ProcessOperations
    {
        #region ReaderManager start

        //a reader alkalmazás elindítása, ami reader módba teszi az adobét
        public static void StartReader()
        {
            //a reader.exe helyi mappában kell hogy legyen
            ProcessStartInfo procInfo = new ProcessStartInfo {
                FileName = "reader.exe",
                UseShellExecute = false
            };
            Process ProcMakePos = new Process();
            try {
                //a program elindítása
                ProcMakePos = Process.Start(procInfo);
            }
            catch (InvalidOperationException){
                Console.WriteLine($"{DateTime.Now.ToString()}:A redear.exe (Ami eltűnteti a felesleges dolgokat és reader módba állítja a pdfet) nem tudott elindulni");
                return;
            }
            catch (Win32Exception) {
                Console.WriteLine($"{DateTime.Now.ToString()}:A redear.exe (Ami eltűnteti a felesleges dolgokat és reader módba állítja a pdfet) nem található");
            }
            catch (Exception) {
                Console.WriteLine($"{DateTime.Now.ToString()}:A redear.exe (Ami eltűnteti a felesleges dolgokat és reader módba állítja a pdfet) nem tudott elindulni");
                return;
            }

            try {
                //ha még nem futott volna le akkor várjon
                while (!ProcMakePos.HasExited) {
                    Thread.Sleep(100);
                }
            }
            catch (NullReferenceException) {

            }
            catch (Exception) {

            }
        }

        #endregion

        #region adobe things

        //az adobéhez a az indítása argumentumok megadása
        public static string SetArguments(string fileName)
        {
            var filePath = Datas.PDFsPath+fileName;

            var arg = $"/n /A \"page=1&zoom={Datas.ZoomScale}&scrollbar=0\" \"{filePath}\"";

            return arg;
        }

        //az adobe program elindítása
        public static Process StartAdobe(string args)
        {
            Process process;

            ProcessStartInfo adobeStartInfo = new ProcessStartInfo()
            {
                FileName = Datas.PathToPDFReader,
                Arguments = args,                
            };


            try {
                //az adobe program elindítása
                process = Process.Start(adobeStartInfo);
            }
            catch (Exception ex) {
                Console.WriteLine($"{DateTime.Now.ToString()}:A PDF olvasó nem található vagy valami más hiba lépett fel");
                Logger.MakeLog("A PDF olvasó elinditása közben hiba lépett fel: " + ex.Message);
                return null;
            }

            //adobe megvárása amíg betölti az alap GUIját
            process.WaitForInputIdle();

            //az elindítás után várjon ennyit, ez bevárja a lasabb gépeket
            Thread.Sleep(Datas.WaitingTime * 1000);

            return process;
        }

        #endregion
    }
}
