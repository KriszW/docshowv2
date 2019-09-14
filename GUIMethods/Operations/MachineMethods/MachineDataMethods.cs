using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    static class MachineDataMethods
    {

        public static List<string> GetRawGepInfos()
        {
            //az összes gép infót szerezze meg
            List<string> gepInfos = FileOperations.Read(CommonDatas.GepInfoPath);

            //ha hibás akkor csinálja addig amíg nem lesz jó
            if (gepInfos == null)
            {
                DataChecking.CheckTheExcelIsOpened(CommonDatas.GepInfoPath);

                gepInfos = FileOperations.Read(CommonDatas.GepInfoPath);
            }

            return gepInfos;
        }

        public static List<string> GetIPs(string asztalszam)
        {
            //a nyers gép adatok megszerzése
            List<string> gepInfos = GetRawGepInfos();

            //a nyers adatokból ip cím
            List<string> ips = GetIPsFromRawData(gepInfos, asztalszam);

            return ips;
        }

        public static List<string> GetIPsFromRawData(List<string> gepInfos, string asztalszam)
        {
            List<string> output = new List<string>();

            //ha nincs megadva asztalszám akkor az összeset
            if (asztalszam == "")
            {
                output = (from info in gepInfos select info.Split(';')[1]).Distinct().ToList();
            }
            //ha megvan adva asztalszám akkor csak azokat
            else
            {
                output = (from info in gepInfos where info.Split(';')[0] == asztalszam select info.Split(';')[1]).Distinct().ToList();
            }

            return output;
        }

    }
}
