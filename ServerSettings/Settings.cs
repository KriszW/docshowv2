using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ServerSettings
{
    public class Settings
    {
        public static Settings CurrentSettings { get; set; }

        public string ItemNumberFile { get; set; }
        public string CodeTableFile { get; set; }
        public string Resources { get; set; }
        public string Machines { get; set; }
        public string ConnString { get; set; }
        public int Interval { get; set; }

        public static Settings LoadSettings(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var text = System.IO.File.ReadAllText(path);
                return new JavaScriptSerializer().Deserialize<Settings>(text);
            }
            else
            {
                throw new ApplicationException(path + " settings fájl nem található!");
            }
        }
    }
}
