using ItemNumberManager;
using System;
using System.Collections.Generic;

namespace KilokoModelLibrary
{
    [Serializable]
    public enum KilokoPosition
    {
        None,
        Left,
        Right
    }

    public class KilokoModel : IKilokoModel
    {
        public static List<KilokoModel> Kilokok { get; set; }

        public KilokoModel(string kiloko)
        {
            RawKiloko = kiloko;
            Kiloko = GetKilokoNumber();

            SetPosition();

            Items = new List<ItemNumber>();
        }

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

        private void SetPosition()
        {
            Position = KilokoPosition.None;

            var pos = RawKiloko[RawKiloko.Length - 1].ToString().ToUpper();

            if (pos == "B")
            {
                Position = KilokoPosition.Left;
            }
            else if (pos == "J")
            {
                Position = KilokoPosition.Right;
            }
        }

        public int MaxCikkCount { get { return 2; } }
        public int Kiloko { get; }
        public string RawKiloko { get; }

        public List<ItemNumber> Items { get; set; }

        public KilokoPosition Position { get; set; }

        public void AddNewCikk(string cikk, string name, string mask, string[] names) => AddNewCikk(new ItemNumber(cikk, name, mask, names));

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
                if (Position == KilokoPosition.None)
                {
                    output = new ItemNumber($"{Items[0].Material}&&{Items[1].Material}", $"{Items[0].MaterialName}&&{Items[1].MaterialName}", $"{Items[0].Mask}&&{Items[1].Mask}", new string[] { Items[0].PDFs[0].ToString(), Items[1].PDFs[0].ToString() });
                }
            }

            return output;
        }
    }
}