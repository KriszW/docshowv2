using ItemNumberManager;
using KilokoModelLibrary;
using LuxScanOrdModel;
using System.Collections.Generic;
using System.Linq;

namespace LuxScanRawItems
{
    public class KilokoGrouper
    {
        public List<LuxScanItem> RawDatas { get; set; }

        public KilokoGrouper(List<LuxScanItem> rawDatas)
        {
            RawDatas = rawDatas;
        }

        public MonitorPosition GetPosition(string raw)
        {
            if (raw.ToUpper().EndsWith("B"))
            {
                return MonitorPosition.Left;
            }
            else if (raw.ToUpper().EndsWith("J"))
            {
                return MonitorPosition.Right;
            }
            else
            {
                return MonitorPosition.None;
            }
        }

        public List<KilokoModel> GroupKilokok()
        {
            var output = new List<KilokoModel>();

            foreach (var item in RawDatas)
            {
                foreach (var kilokoItem in item.Models)
                {
                    var model = output.FirstOrDefault(m=> m.Kiloko == kilokoItem.Kiloko);

                    if (model == default)
                    {
                        model = new KilokoModel(kilokoItem.Kiloko.ToString());
                        output.Add(model);
                    }

                    foreach (var material in item.Items)
                    {
                        model.AddNewCikk(material,GetPosition(kilokoItem.RawKiloko));
                    }
                }
            }

            return output;
        }
    }
}