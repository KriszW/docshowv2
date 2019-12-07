using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LanguageSelectorLibrary
{
    public class LanguageItem
    {
        public static List<LanguageItem> Items { get; set; }

        public string ID { get; set; }
        public string Value { get; set; }

        public static List<LanguageItem> Config(string path)
        {
            if (path.EndsWith(".json"))
            {
                var text = System.IO.File.ReadAllText(path);
                return new JavaScriptSerializer().Deserialize<List<LanguageItem>>(text);
            }
            else
            {
                throw new ApplicationException("The Language file must be a json file");
            }
        }
    }
}
