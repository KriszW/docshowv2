using LuxScanOrdModel;
using ItemNumberManager;
using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LuxScanRawItems
{
    class GetRawItems
    {
        public event OnNewLuxscanItems NewItems;

        public string[] RawLines { get; private set; }

        public List<LuxScanItem> RawItems { get; private set; }

        private IDConverter _converter;

        public GetRawItems(string[] lines)
        {
            RawLines = lines;
            RawItems = new List<LuxScanItem>();
        }

        public List<LuxScanItem> Convert()
        {
            foreach (var item in RawLines)
            {
                if (item.StartsWith("Item") || IsUsableItem(item))
                {
                    Debug.WriteLine("Converter létrehozása");
                    _converter = new IDConverter(item);
                    Debug.WriteLine("ID megszerzése");
                    var id = _converter.GetID();

                    Debug.WriteLine($"Az LuxScanItem megszerzése a {id}-hoz");
                    var luxscanItem = RawItems.Where(r => r.ID == id).FirstOrDefault();

                    Debug.WriteLine($"Az Itemtípus megszerzése a {id}-hoz");
                    var itemType = GetItemType(item);

                    switch (itemType)
                    {
                        case "Item":
                            if (IsActiveItem(item))
                            {
                                Debug.WriteLine($"Az {id} aktív");
                                var kilokoGetter = new KilokoGetter(item);

                                Debug.WriteLine($"Az Kilőkők megszerzése a {id}-hoz");

                                var kilokoNum = kilokoGetter.GetKilokoNum();
                                var kiloko = KilokoGetter.GetKilokoFromCodeTable(kilokoNum);

                                Debug.WriteLine($"Az új LuxScanItem hozzáadása");

                                RawItems.Add(new LuxScanItem(id, kiloko));
                            }

                            break;
                        case "ItemName":

                            var rawItem = RawItems.Where(r => r.ID == id).FirstOrDefault();

                            if (rawItem != default)
                            {
                                Debug.WriteLine($"Az új cikk hozzáadása az {id}-hoz");
                                rawItem.AddItem(item.Split('=')[1]);
                            }

                            break;
                    }
                }
            }

            NewItems?.Invoke(this, RawItems);

            return RawItems;
        }
        public string GetItemType(string rawLine)
        {
            var output = "";

            foreach (var c in rawLine)
            {
                if (char.IsDigit(c) == false)
                {
                    output += c;
                }
                else
                {
                    break;
                }
            }

            return output;
        }

        public bool IsUsableItem(string rawLine)
        {
            var usableItems = new string[] {"ItemName" };

            foreach (var item in usableItems)
            {
                if (rawLine.StartsWith(item))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsActiveItem(string rawLine)
        {
            var activeMark = "F";
            var activePos = 7;

            var texts = rawLine.Split(',');

            return texts[activePos] == activeMark;
        }
    }
}
