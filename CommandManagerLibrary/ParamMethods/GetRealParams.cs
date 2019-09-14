using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagerLib
{
    public static class GetRealParams
    {
        //a paraméterekből azokat összeszedi ami egy "" jel közé van téve
        public static string[] GetRealParamsFromRawParams(string[] rawParams)
        {
            List<string> output = new List<string>();

            bool inQuitionMarks = false;

            string line = "";

            foreach (var item in rawParams)
            {
                if (item.StartsWith("\""))
                {
                    inQuitionMarks = true;
                }
                if (inQuitionMarks)
                {
                    line += item.Replace("\"", "")+" ";
                }
                if (item.EndsWith("\""))
                {
                    output.Add(line.TrimEnd(' '));

                    line = "";

                    inQuitionMarks = false;
                }
            }

            return output.ToArray();
        }
    }
}
