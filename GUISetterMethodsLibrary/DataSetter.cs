using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocsShowServer;

namespace GUISetterMethodsLibrary
{
    public static class DataSetter
    {
        public static void SetData(string asztal,string stand1,string stand2,string cikk)
        {
            CommonDatas.InfoTable.DGVManager.ReplaceInfo(asztal,stand1,stand2,cikk);
        }
    }
}
