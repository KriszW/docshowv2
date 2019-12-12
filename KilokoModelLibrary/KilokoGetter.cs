using System.Collections.Generic;

namespace KilokoModelLibrary
{
    public class KilokoGetter
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string RawLine { get; private set; }

        public KilokoGetter(string line)
        {
            RawLine = line;
        }

        public string GetKilokoNum()
        {
            var kilokoPos = 15;

            var rawItemline = RawLine.Split('=')[1];

            var output = rawItemline.Split(',')[kilokoPos];

            _logger.Debug($"{RawLine} sorból az {output} kiolvasva");

            return output;
        }

        public static IKilokoModel GetKiloko(string number)
        {
            foreach (var item in KilokoModel.Kilokok)
            {
                if (item.RawKiloko == number)
                {
                    _logger.Debug($"A {number}-hez megtaláltuk a {item.RawKiloko} kilőkőt");
                    return item;
                }
            }

            _logger.Debug($"A {number}-hez nem találtunk megfelelő kilőkőt");
            return default;
        }

        public static List<IKilokoModel> GetKilokoFromCodeTable(string code)
        {
            var output = new List<IKilokoModel>();

            foreach (var item in CodeTableModel.Codes)
            {
                if (item.CodeName == code)
                {
                    _logger.Debug($"{code} kódhoz találtunk kilőkőket a kódtáblába...");

                    for (int i = 0; i < item.Kilokok.Count; i++)
                    {
                        _logger.Debug($"{code} kódhoz a {i+1}. {item.Kilokok[i].RawKiloko} kilőkő megtalálva");
                    }

                    return item.Kilokok;
                }
            }

            _logger.Debug($"A {code} kódhoz nem találtunk kilőkőt a kódtáblába");
            return output;
        }
    }
}