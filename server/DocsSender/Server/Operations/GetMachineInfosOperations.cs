using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocsShowServer
{
    class GetMachineInfosOperations
    {
        //az asztal megszerzése
        public static List<string> GetAsztalokFromIP(string IP)
        {
            string path = CommonDatas.GepInfoPath;

            //output
            List<string> asztalok = new List<string>();

            //a gépek adatain beolvasása
            List<string> gepInfos = FileOperations.Read(path);

            //ha null akkor addig próbálja amíg nem sikerül neki
            if (gepInfos == null && CommonDatas.GepInfoData.Count==0)
            {
                DataChecking.CheckTheExcelIsOpened(path, true);

                //utána menjen végig mégegyszer és azt adja vissza
                return GetAsztalokFromIP(IP);
            }

            if (gepInfos!=null)
            {
                CommonDatas.GepInfoData = gepInfos;
            }
            else
            {
                DataChecking.CheckTheExcelIsOpened(path, false);
            }


            //végigmegy a queryn
            foreach (var item in CommonDatas.GepInfoData)
            {
                //az adatstruktúra
                //asztal;ip;monitorSzám
                string[] tmp = item.Split(';');

                //ha az gépIP = IP
                if (tmp[1] == IP)
                    asztalok.Add(tmp[0]);
            }

            //ha nincs asztal akkor adjon vissza nullt
            if(asztalok.Count==0)
            {
                return null;
            }

            //az output visszaadása
            return asztalok;
        }
    }
}
