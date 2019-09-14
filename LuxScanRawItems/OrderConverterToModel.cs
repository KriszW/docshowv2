using LuxScanOrdModel;
using LuxScanOrdReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendOutModels;
using TCPServer;

namespace LuxScanRawItems
{
    public class OrderConverterToModel
    {
        public OrderConverterToModel()
        {

        }

        public List<LuxScanItem> Convert(string path)
        {
            var rawLines = System.IO.File.ReadAllLines(path);

            var getter = new GetRawItems(rawLines);

            return getter.Convert();
        }
    }
}
