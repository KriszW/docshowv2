using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Settings.Server
{
    public class ServerSettings
    {
        public static ServerSettings CurrentSettings { get; set; }

        public string ItemNumberFile { get; set; }
        public string CodeTableFile { get; set; }
        public string Resources { get; set; }
        public string Machines { get; set; }
        public string ConnString { get; set; }
        public string PsExecLocation { get; set; }
        public int Interval { get; set; }

        public static ServerSettings LoadSettings(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var text = System.IO.File.ReadAllText(path);
                return new JavaScriptSerializer().Deserialize<ServerSettings>(text);
            }
            else
            {
                throw new ApplicationException(path + " settings fájl nem található!");
            }
        }
    }
}
