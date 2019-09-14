using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemNumberManager
{
    class PDFekModel
    {
        public PDFekModel(string mask, string[] files)
        {
            Mask = mask;
            Files = files;
        }

        public string Mask { get; private set; }
        public string[] Files { get; private set; }
    }
}
