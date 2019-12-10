using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendedModels
{
    [Serializable]
    public class GetPDFModel
    {
        public string FileName { get; set; }
        public bool IsLast { get; set; }
        public int Offset { get; set; }
        public byte[] PayLoad { get; set; }
    }
}
