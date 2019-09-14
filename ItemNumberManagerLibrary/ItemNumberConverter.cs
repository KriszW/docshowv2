using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemNumberManager
{
    public class ItemNumberConverter
    {
        public static List<string> Lines { get; set; }

        static bool UsableItem(string itemNum, string mask)
        {
            //az egész cikken végig kell menni karakterenként
            for (int i = 0; i < mask.Length; i++)
            {
                var maskChar = mask[i];
                var matnumChar = itemNum[i];

                //ha a két karakter megegyezik akkor mehet tovább
                if (maskChar == matnumChar)
                {
                    continue;
                }

                //ha * akkor bármi megfelel
                if (maskChar == '*')
                {
                    continue;
                }

                //ha % akkor utána bármennyi karakter megjelenhet
                if (maskChar == '%')
                {
                    return true;
                }

                //ha egy karakter nem egyezik már akkor nem megfelelő a mask
                //ezért visszatér falseal
                if (maskChar != matnumChar)
                {
                    return false;
                }
            }

            //ha minden karakter megegyezett visszatér trueval
            return true;
        }

        static PDFekModel GetPDFek(string itemNum)
        {
            foreach (var item in Lines)
            {
                var mask = item.Split(';')[0];
                if (UsableItem(itemNum, mask))
                {
                    var datas = item.Split(";".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);
                    return new PDFekModel(datas[0],datas[1].Split(';'));
                }
            }

            return null;
        }

        static string GetName(string[] raws)
        {
            var output = "";

            for (int i = 0; i < raws.Length-1; i++)
            {
                output += raws[i]+" ";
            }

            return output.Trim(' ');
        }

        public static ItemNumber ConvertRawLine(string rawItemnum)
        {
            var raws = rawItemnum.Split('_');
            var itemNum = raws.Last().TrimEnd(',');
            var name = GetName(raws);
            var model = GetPDFek(itemNum);

            if (model != default)
            {
                return new ItemNumber(itemNum, name, model.Mask, model.Files);
            }

            return new ItemNumber(itemNum, name);
        }
    }
}
