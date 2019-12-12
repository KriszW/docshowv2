using KilokoModelLibrary;
using LuxScanOrdModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LuxScanRawItems
{
    internal class GetRawItems
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                    _logger.Debug("Converter létrehozása");
                    _converter = new IDConverter(item);
                    _logger.Debug("ID megszerzése");
                    var id = _converter.GetID();

                    _logger.Debug($"Az LuxScanItem megszerzése a {id}-hoz");
                    var luxscanItem = RawItems.Where(r => r.ID == id).FirstOrDefault();

                    _logger.Debug($"Az Itemtípus megszerzése a {id}-hoz");
                    var itemType = GetItemType(item);

                    switch (itemType)
                    {
                        case "Item":
                            if (IsActiveItem(item))
                            {
                                _logger.Debug($"Az {id} aktív");
                                var kilokoGetter = new KilokoGetter(item);

                                _logger.Debug($"Az Kilőkők megszerzése a {id}-hoz");

                                var kilokoNum = kilokoGetter.GetKilokoNum();
                                var kiloko = KilokoGetter.GetKilokoFromCodeTable(kilokoNum);

                                _logger.Debug($"Az új LuxScanItem hozzáadása");

                                RawItems.Add(new LuxScanItem(id, kiloko));
                            }

                            break;

                        case "ItemName":

                            var rawItem = RawItems.Where(r => r.ID == id).FirstOrDefault();

                            if (rawItem != default)
                            {
                                _logger.Debug($"Az új cikk hozzáadása az {id}-hoz");
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
            var usableItems = new string[] { "ItemName" };

            foreach (var item in usableItems)
            {
                if (rawLine.StartsWith(item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsActiveItem(string rawLine)
        {
            var activeMark = "F";
            var activePos = 7;

            var texts = rawLine.Split(',');

            return texts[activePos] == activeMark;
        }
    }
}