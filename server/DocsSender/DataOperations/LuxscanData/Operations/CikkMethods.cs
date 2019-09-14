using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class CikkMethods
    {
        //*- egy karakter akármi lehet
        //%- korlátlan mennyiségű bármilyen karakter

        private static bool Useableitem(string cikkMask, string cikk)
        {
            //az egész cikken végig kell menni karakterenként
            for(int i = 0; i < cikkMask.Length; i++)
            {
                //ha a két karakter megegyezik akkor mehet tovább
                if(cikkMask[i] == cikk[i])
                {
                    continue;
                }

                //ha * akkor bármi megfelel
                //ha % akkor utána bármennyi karakter megjelenhet
                //a %-jel után is lehet írni
                if (cikkMask[i] == '*' || cikkMask[i] == '%')
                {
                    continue;
                }

                //ha egy karakter nem egyezik már akkor nem megfelelő a mask
                //ezért visszatér falseal
                if(cikkMask[i] != cikk[i])
                {
                    return false;
                }
            }

            //ha minden karakter megegyezett visszatér trueval
            return true;
        }

        //a mask összehasonlítása
        private static string GetMask(string cikk, List<string> cikkek)
        {
            string output = cikk;

            foreach (var item in cikkek)
            {
                //ha kisebb a cikkszám hossza mint a maské, és nem végződik %-ra tehát utána már nem lehet bármi akkor ugorjon
                if (item.Length < cikk.Length && !item.EndsWith("%"))
                {
                    string tmpCikk = cikk.Substring(0, 6);
                    if (item != tmpCikk)
                    {
                        continue;
                    }
                }

                //ha a hossza nagyobb akkor ugorjon
                if (item.Length > cikk.Length)
                {
                    continue;
                }

                //a mask eltávólítása
                if (Useableitem(item, cikk))
                {
                    //ha a mask alapján helyes akkor ezt állítja be outputnak
                    //így ha többnél van egyezés akkor az utolsót fogja használni
                    output = item;
                }
            }

            return output;
        }

        //a cikk maskolásának lekérése
        public static string GetCikk(string rawCikk, List<string> cikkek)
        {
            //előszőr az itemName részét vágom le
            string output = rawCikk.Split('=')[1].Split(',')[0];
            //utána feldarabolom a cikket, és az utolsó _ után lévő adatott hasznosítom
            output = output.Split('_').Last();
            //a mask alapján összehasonlítás
            output = GetMask(output, cikkek);

            return output;
        }

        public static List<string> GetCikkek()
        {
            //Ha a cikkek kiolvasása hogy könnyebb legyen összemérni a cikkeket
            List<string> rawCikkek = FileOperations.Read(CommonDatas.PathtoCikkek);

            //az output meghatározása
            List<string> output = new List<string>();

            //ha a cikkekből nem lehetet kiolvasni mert hiba adodott
            if(rawCikkek == null && CommonDatas.PDFDatas.Count==0)
            {
                //addig vizsgálja amíg nem jó
                DataChecking.CheckTheExcelIsOpened(CommonDatas.PathtoCikkek,true);

                //a cikkek újra olvasása
                rawCikkek = FileOperations.Read(CommonDatas.PathtoCikkek);
            }
            else
            {
                if (rawCikkek!=null)
                {
                    CommonDatas.PDFDatas = rawCikkek;
                }
                else
                {
                    DataChecking.CheckTheExcelIsOpened(CommonDatas.PathtoCikkek, false);
                }

                //végig megy a cikkeken és a cikket magát hozzáadja az outputhoz
                foreach(var item in CommonDatas.PDFDatas)
                {
                    output.Add(item.Split(';')[0]);
                }
            }

            return output;
        }
    }
}
