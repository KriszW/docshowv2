using KilokoModelLibrary;
using SendedModels;
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
        public Positioning(PositionModel model, Screen screen)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Screen = screen ?? throw new ArgumentNullException(nameof(screen));
            Adobes = new List<Process>();
        }

        public PositionModel Model { get; set; }
        public Screen Screen { get; set; }

        public List<Process> Adobes { get; set; }

        public void CloseAllAdobe() 
        {
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
            int index = 1;

            foreach (var item in Model.PDF)
            {
                if (Model.Postion == KilokoPosition.None)
                {
                    var pos = (KilokoPosition)index;

                    SplitForOneSide(item, pos);

                    index++;
                }
                else
                {
                    SplitForOneSide(item, Model.Postion);
                }
            }
        }

        private bool SplitForOneSide(PDFModel Model, KilokoPosition pos)
        {
            //ha a standard empty vagy nyitva van akkor nem kell csinálnia semmit
            if (Model.PDFFileName.EndsWith(".pdf") == false)
            {
                Model.PDFFileName += ".pdf";
            }

            //az adobehez az argumentumok beállítása
            string args = ProcessOperations.SetArguments(Model.PDFFileName);

            //az adobe program elindítása a megadott argumentumokkal
            Process adobeProc = ProcessOperations.StartAdobe(args);
            Adobes.Add(adobeProc);

            //a megfelelő helyre állítása a megadott monitoron a megadott helyre
            SetForGoodPosition(pos);

            //a readermódba állító program elindítása
            ProcessOperations.StartReader();

            return true;
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

        private void SetForGoodPosition(KilokoPosition pos)
        {
            var screen = Screen.Bounds;

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
    }
}