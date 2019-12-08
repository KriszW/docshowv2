using KilokoModelLibrary;
using LuxScanOrdModel;
using System.Collections.Generic;
using System.Linq;

namespace LuxScanRawItems
{
    public class KilokoGrouper
    {
        public List<LuxScanItem> RawDatas { get; set; }

        private List<KilokoModel> Models { get; set; }

        public KilokoGrouper(List<LuxScanItem> rawDatas)
        {
            RawDatas = rawDatas;
            Models = new List<KilokoModel>();
        }

        private KilokoModel GetModel(string number)
        {
            var model = Models.FirstOrDefault(m=> m.Kiloko.ToString() == number);

            if (model == default)
            {
                model = new KilokoModel(number);

                Models.Add(model);
            }

            return model;
        }

        public List<KilokoModel> GroupKilokok()
        {
            foreach (var item in RawDatas)
            {
                foreach (var kilokoItem in item.Models)
                {
                    var kilokoNum = kilokoItem.GetKilokoNumFromRaw(); 
                    var model = GetModel(kilokoNum);

                    foreach (var material in item.Items)
                    {
                        model.AddNewCikk(material);
                    }
                }
            }

            return Models;
        }
    }
}