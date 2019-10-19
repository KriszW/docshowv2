using KilokoModelLibrary;
using SendedModels;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PositioningLib
{
    public class Positioning
    {
        public static void PositionModel(PositionModel model, Screen screen)
        {
            int index = 1;

            var rect = screen.Bounds;

            foreach (var item in model.PDF)
            {
                if (model.Postion == KilokoPosition.None)
                {
                    var pos = (KilokoPosition)index;

                    SplitForOneSide(item, rect, pos);

                    index++;
                }
                else
                {
                    SplitForOneSide(item, rect, model.Postion);
                }
            }
        }

        #region display manager

        private static bool SplitForOneSide(PDFModel model, Rectangle screen, KilokoPosition pos)
        {
            //ha a standard empty vagy nyitva van akkor nem kell csinálnia semmit
            if (model.PDFFileName.EndsWith(".pdf") == false)
            {
                model.PDFFileName += ".pdf";
            }

            //az adobehez az argumentumok beállítása
            string args = ProcessOperations.SetArguments(model.PDFFileName);

            //az adobe program elindítása a megadott argumentumokkal
            Process adobeProc = ProcessOperations.StartAdobe(args);

            //a megfelelő helyre állítása a megadott monitoron a megadott helyre
            SetForGoodPosition(screen, pos);

            //a readermódba állító program elindítása
            ProcessOperations.StartReader();

            return true;
        }

        private static IntPtr GetMainWindowHandle(Process process)
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

        private static void SetForGoodPosition(Rectangle screen, KilokoPosition pos)
        {
            //az előtérben lévő program pointerének megszerzése
            IntPtr hwWindowHandle = NativeMethods.GetForegroundWindow();
            //IntPtr hwWindowHandle = GetMainWindowHandle(adobe);

            var halfWidth = screen.Width / 2;

            switch (pos)
            {
                case KilokoPosition.Left:
                    NativeMethods.SetWindowPos(hwWindowHandle, HW.Top, screen.X, screen.Y, halfWidth, screen.Height, SWD.SHOWWINDOW);
                    break;

                case KilokoPosition.Right:
                    NativeMethods.SetWindowPos(hwWindowHandle, HW.Top, screen.X + halfWidth, screen.Y, halfWidth, screen.Height, SWD.SHOWWINDOW);
                    break;

                default:
                    NativeMethods.SetWindowPos(hwWindowHandle, HW.Top, screen.X, screen.Y, halfWidth, screen.Height, SWD.SHOWWINDOW);
                    break;
            }

            //előtérbe állítása ha valami beugrott volna közben
            NativeMethods.SetForegroundWindow(hwWindowHandle);
        }

        #endregion display manager
    }
}