using ItemNumberManager;
using System;
using System.Collections.Generic;

namespace KilokoModelLibrary
{
    public class KilokoModel : IKilokoModel
    {
        public static List<KilokoModel> Kilokok { get; set; }

        public KilokoModel(string kiloko)
        {
            RawKiloko = kiloko;
            Kiloko = GetKilokoNumber();

            Items = new List<ItemNumber>();
        }

        public string GetKilokoNumFromRaw() => GetKilokoNumber().ToString();

        private int GetKilokoNumber()
        {
            var tempLine = "";

            foreach (var item in RawKiloko)
            {
                if (char.IsDigit(item))
                {
                    tempLine += item;
                }
            }

            return int.Parse(tempLine);
        }

        public int MaxCikkCount { get { return 2; } }
        public int Kiloko { get; }
        public string RawKiloko { get; }

        public List<ItemNumber> Items { get; set; }

        public void AddNewCikk(string cikk, string name, string mask, string[] names) => AddNewCikk(new ItemNumber(cikk, name, mask, names));

        public void AddNewCikk(ItemNumber item, MonitorPosition pos)
        {
            if (Items.Count < MaxCikkCount)
            {
                if (item != default)
                {
                    if (pos != MonitorPosition.None)
                    {
                        foreach (var pDF in item.PDFs)
                        {
                            pDF.Position = pos;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < item.PDFs.Count; i++)
                        {
                            item.PDFs[i].Position = (MonitorPosition)i;
                        }
                    }

                    Items.Add(item);
                }
            }
        }

        public void AddNewCikk(ItemNumber item)
        {
            if (Items.Count < MaxCikkCount)
            {
                if (item != default)
                {
                    Items.Add(item);
                }
            }
        }

        public void AddNewCikk(string rawLine) => AddNewCikk(ItemNumberConverter.ConvertRawLine(rawLine));

        public void DeleteAllCikk()
        {
            Items = new List<ItemNumber>();
        }

        public ItemNumber GetItemnamesModel()
        {
            var output = Items[0];

            if (Items.Count > 1)
            {
                output = new ItemNumber($"{Items[0].Material}&&{Items[1].Material}", $"{Items[0].MaterialName}&&{Items[1].MaterialName}", $"{Items[0].Mask}&&{Items[1].Mask}", new string[] { Items[0].PDFs[0].ToString(), Items[1].PDFs[0].ToString() });

                var index = 0;

                foreach (var item in output.PDFs)
                {
                    item.Position = (MonitorPosition)index;
                    index++;
                }
            }

            return output;
        }
    }
}