using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KilokoModelLibrary
{
    public class KilokoGetter
    {
        public string RawLine { get; private set; }

        public KilokoGetter(string line)
        {
            RawLine = line;
        }

        public string GetKilokoNum()
        {
            var kilokoPos = 15;

            var rawItemline = RawLine.Split('=')[1];

            return rawItemline.Split(',')[kilokoPos];
        }

        public static IKilokoModel GetKiloko(string number)
        {
            foreach (var item in KilokoModel.Kilokok)
            {
                if (item.RawKiloko == number)
                {
                    return item;
                }
            }

            return default;
        }

        public static List<IKilokoModel> GetKilokoFromCodeTable(string code)
        {
            var output = new List<IKilokoModel>();

            foreach (var item in CodeTableModel.Codes)
            {
                if (item.CodeName == code)
                {
                    return item.Kilokok;
                }
            }

            return output;
        }
    }
}
