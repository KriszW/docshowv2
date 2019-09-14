using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KilokoModelLibrary
{
    public class CodeTableModel
    {
        public static List<CodeTableModel> Codes { get; set; }
        public CodeTableModel(string code, List<IKilokoModel> models)
        {
            CodeName = code;
            Kilokok = models;
        }

        public string CodeName { get; private set; }

        public List<IKilokoModel> Kilokok { get; private set; }
    }
}
