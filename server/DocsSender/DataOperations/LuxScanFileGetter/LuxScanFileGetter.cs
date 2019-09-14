using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class LuxScanFileGetter
    {
        public static void SetLuxscanFilePath()
        {
            LuxScanFileOperations operations = new LuxScanFileOperations();

            //ha nem üres a datapath akkor onnan olvassa az adatot
            if (CommonDatas.LuxscanDataPath != "")
            {
                operations.ReadLuxScanFilePath(CommonDatas.LuxscanDataPath);
            }
            else
            {
                operations.ReadLuxScanFilePathFromExePath();
            }
        }
    }
}
