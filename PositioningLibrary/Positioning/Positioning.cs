using SendedModels;
using ItemNumberManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PositioningLib
{
    public class Positioning
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Positioning(PositionModel model, Screen screen)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Screen = screen ?? throw new ArgumentNullException(nameof(screen));
            Adobes = new List<Process>();
        }

        public PositionModel Model { get; set; }
        public Screen Screen { get; set; }

        public List<Process> Adobes { get; set; }

        public static void CloseAllAdobeProcess()
        {
            _logger.Info($"Az összes Adobe PDF reader bezárása");
            foreach (var item in Process.GetProcessesByName("AcroRd32"))
            {
                try
                {
                    item.Kill();
                }
                catch (Exception) { }
            }
        }

        public void CloseAllAdobe() 
        {
            _logger.Info($"Az összes PDF reader bezárása, amit a program indított el");
            foreach (var item in Adobes)
            {
                try
                {
                    item.Kill();
                }
                catch (Exception) { }
            }

            Adobes.Clear();
        }

        public void PositionModel()
        {
            _logger.Info($"A PDF model pozicionálása a {Screen.DeviceName} képernyőre");
            int index = 1;

            foreach (var item in Model.PDF)
            {
                _logger.Debug($"A {item.PDFFileName} pdf a {item.MonitorIndex} pozicióra került a {item.ItemName} Cikk számmal");
                if (item.Position != MonitorPosition.None)
                {
                    _logger.Debug($"A {item.PDFFileName} pdf {item.Position.ToString()} oldalra került");
                    SplitForOneSide(item, item.Position);
                }
                else
                {
                    var pos = (MonitorPosition)index;
                    _logger.Debug($"A {item.PDFFileName} pdfnek nem volt megadva pozició. {pos.ToString()} oldalra került");

                    SplitForOneSide(item, pos);

                    index++;
                }
            }
        }

        private bool SplitForOneSide(PDFModelOverTCP model, MonitorPosition pos)
        {
            _logger.Info($"A {model.PDFFileName} pdf poziciónálása a {model.MonitorIndex} monitorra, a {pos.ToString()} oldalára");
            //ha a standard empty vagy nyitva van akkor nem kell csinálnia semmit
            if (model.PDFFileName.EndsWith(".pdf") == false)
            {
                model.PDFFileName += ".pdf";
            }

            //az adobehez az argumentumok beállítása
            try
            {
                _logger.Debug($"A {model.PDFFileName} pdfhez az argumentumok beállítása");
                string args = ProcessOperations.SetArguments(model.PDFFileName);

                _logger.Debug($"A {model.PDFFileName} pdfhez a PDF olvasó beállítása a {args} argumentumokkal");
                //az adobe program elindítása a megadott argumentumokkal
                Process adobeProc = ProcessOperations.StartAdobe(args);
                Adobes.Add(adobeProc);

                _logger.Debug($"A {model.PDFFileName} pdfhez a PDF olvasó beállítása a {args} argumentumokkal beállítva");
                _logger.Debug($"A {model.PDFFileName} pdfhez a poziciónálása megkezdése");
                //a megfelelő helyre állítása a megadott monitoron a megadott helyre
                SetForGoodPosition(pos);

                _logger.Debug($"A {model.PDFFileName} pdfhez a PDF reader utáni script futtatása");
                //a readermódba állító program elindítása
                ProcessOperations.StartReader();

                return true;
            }
            catch (PDFNotFoundException)
            {

            }
            catch (Exception)
            {

            }

            return false;
        }

        private IntPtr GetMainWindowHandle(Process process)
        {
            IntPtr output = IntPtr.Zero;

            const int maxTestCount = 100;

            int counter = 0;

            while (counter < maxTestCount)
            {
                Thread.Sleep(100);

                process.Refresh();

                output = process.MainWindowHandle;

                if (output != IntPtr.Zero)
                {
                    return output;
                }

                counter++;
            }

            if (output == IntPtr.Zero)
            {
                output = NativeMethods.GetForegroundWindow();
            }

            return output;
        }

        private void SetForGoodPosition(MonitorPosition pos)
        {
            var screen = Screen.Bounds;

            _logger.Debug($"A fő ablak Pointerrének a lékérése");
            //az előtérben lévő program pointerének megszerzése
            IntPtr hwWindowHandle = NativeMethods.GetForegroundWindow();
            //IntPtr hwWindowHandle = GetMainWindowHandle(adobe);
            _logger.Debug($"A fő ablak Pointerrének a lékérése sikeres volt: {hwWindowHandle}");

            var halfWidth = screen.Width / 2;

            switch (pos)
            {
                case MonitorPosition.Left:
                    _logger.Debug($"A poziónálás a bal oldalra");
                    NativeMethods.SetWindowPos(hwWindowHandle, HW.Top, screen.X, screen.Y, halfWidth, screen.Height, SWD.SHOWWINDOW);
                    break;

                case MonitorPosition.Right:
                    _logger.Debug($"A poziónálás a jobb oldalra");
                    NativeMethods.SetWindowPos(hwWindowHandle, HW.Top, screen.X + halfWidth, screen.Y, halfWidth, screen.Height, SWD.SHOWWINDOW);
                    break;

                default:
                    _logger.Debug($"A poziónálás a bal oldalra");
                    NativeMethods.SetWindowPos(hwWindowHandle, HW.Top, screen.X, screen.Y, halfWidth, screen.Height, SWD.SHOWWINDOW);
                    break;
            }

            _logger.Debug($"A poziciónált ablak előtérbe helyezése");
            //előtérbe állítása ha valami beugrott volna közben
            NativeMethods.SetForegroundWindow(hwWindowHandle);
        }
    }
}