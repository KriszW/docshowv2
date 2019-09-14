using System;
using System.Threading;

namespace DocsShowServer
{
    class WaitingMethods
    {
        //ellenőrzi az adatokat, és ha kell akkor abban a részben megáll és vár
        public bool ContinueWaiting()
        {
            //várjon ha connectel egy kliens
            if(Server.ClientConnecting)
            {
                Thread.Sleep(1000);
                return true;
            }

            //ha nincs beállítva luxscanfile akkor várjon
            if (!CommonDatas.HasLuxscanFile) {
                Thread.Sleep(100);
                LuxScanFileGetter.SetLuxscanFilePath();
                return true;
            }

            return false;
        }

        //ha az értékek jók, akkor mehet tovább a luxscanfile olvasása
        public bool StepForwardIfOk(string item)
        {
            DataChecking checking = new DataChecking();

            if (item == "")
                return false;
            if (!char.IsLetter(item[0]))
                return false;
            if (!checking.IsUseAbleLine(item))
                return false;

            return true;
        }
    }
}
