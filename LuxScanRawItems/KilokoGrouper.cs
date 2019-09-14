using KilokoModelLibrary;
using LuxScanOrdModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        KilokoModel GetModel(string number)
        {
            var model = (from kiloko in Models where kiloko.RawKiloko == number select kiloko).FirstOrDefault();

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
                    var model = GetModel(kilokoItem.RawKiloko);

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
