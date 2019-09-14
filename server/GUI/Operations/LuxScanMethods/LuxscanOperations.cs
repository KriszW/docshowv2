using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocsShowServer
{
    class LuxscanOperations
    {
        //csinálja meg a luxscanupdatet
        public static void MakeLuxscanUpdate()
        {
            //beállítja a filet
            LuxScanFileGetter.SetLuxscanFilePath();

            //várja meg a csatlakozásokat
            ClientOperations.WaitForEndConnecting();

            //küldje el az új adatokat
            OperationModel.SendingOperations.SendOutDatas();
        }
    }
}
