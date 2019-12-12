using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemNumberManager
{
    public class ItemNumberConverter
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<string> Lines { get; set; }

        private static bool UsableItem(string itemNum, string mask)
        {
            _logger.Debug($"A {itemNum} cikkhez a {mask} ellenőrzése");
            //az egész cikken végig kell menni karakterenként
            for (int i = 0; i < mask.Length; i++)
            {
                if (i >= itemNum.Length)
                {
                    _logger.Debug($"A {itemNum} cikkhez a {mask} mask invalid");
                    return false;
                }

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
                    _logger.Debug($"A {itemNum} cikkhez a {mask} mask a valid");
                    return true;
                }

                //ha egy karakter nem egyezik már akkor nem megfelelő a mask
                //ezért visszatér falseal
                if (maskChar != matnumChar)
                {
                    _logger.Debug($"A {itemNum} cikkhez a {mask} mask invalid");
                    return false;
                }
            }

            _logger.Debug($"A {itemNum} cikkhez a {mask} mask invalid");
            //ha minden karakter megegyezett visszatér trueval
            return true;
        }

        private static PDFekModel GetPDFek(string itemNum)
        {
            _logger.Debug($"A {itemNum} cikkhez a PDF megszerzése");
            foreach (var item in Lines)
            {
                var mask = item.Split(';')[0];
                _logger.Debug($"A {itemNum} cikkhez a PDF mask {mask} összehasonlítása");
                if (UsableItem(itemNum, mask))
                {
                    _logger.Debug($"A {itemNum} cikkhez a PDF mask {mask} volt a helyes, a PDF model létrehozása");
                    var datas = item.Split(";".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);
                    return new PDFekModel(datas[0], datas[1].Split(';'));
                }
            }

            return null;
        }

        private static string GetName(string[] raws)
        {
            var output = "";

            for (int i = 0; i < raws.Length - 1; i++)
            {
                output += raws[i] + " ";
            }

            return output.Trim(' ');
        }

        public static ItemNumber ConvertRawLine(string rawItemnum)
        {
            _logger.Debug($"A {rawItemnum} lefordítása Cikk modelre");
            var raws = rawItemnum.Split('_');
            var itemNum = raws.Last().TrimEnd(',');
            var name = GetName(raws);
            _logger.Debug($"A {rawItemnum}-ból a cikk név: {name}");
            var model = GetPDFek(itemNum);
            _logger.Debug($"A {rawItemnum}-ból a PDF model: {model.Mask}");

            if (model != default)
            {
                return new ItemNumber(itemNum, name, model.Mask, model.Files);
            }

            return new ItemNumber(itemNum, name);
        }
    }
}