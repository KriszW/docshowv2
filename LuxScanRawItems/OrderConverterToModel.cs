using LuxScanOrdModel;
using System.Collections.Generic;

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