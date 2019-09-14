using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemNumberManager
{
    public class PDFModel
    {
        public PDFModel(string name)
        {
            FileName = name ?? throw new ArgumentNullException(nameof(name));
            FilePath = $@"K:\programs\DocShow\Resources\{name}.pdf";

            Datas = System.IO.File.ReadAllBytes(FilePath);
        }

        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public byte[] Datas { get; private set; }

        public override string ToString()
        {
            return FileName;
        }
    }
}
