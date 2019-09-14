using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxScanRawItems
{
    class IDConverter
    {
        public string RawLine { get; private set; }
        public IDConverter(string line)
        {
            RawLine = line;
        }

        public int GetID()
        {
            var longID = RawLine.Split('=')[0];
            return GetIDFromRaw(longID);
        }

        private int GetIDFromRaw(string longID)
        {
            var tempLine = "";

            foreach(var item in longID)
            {
                if (char.IsDigit(item))
                {
                    tempLine += item;
                }
            }

            return int.Parse(tempLine);
        }
    }
}
