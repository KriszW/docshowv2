using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemNumberManager;
using KilokoModelLibrary;

namespace LuxScanOrdModel
{
    public class LuxScanItem
    {
        public LuxScanItem(int iD, bool isActive, List<IKilokoModel> model, string rawItem)
        {
            ID = iD;
            IsActive = isActive;
            Models = model ?? throw new ArgumentNullException(nameof(model));
            Items = new List<ItemNumber>();
            AddItem(rawItem);
        }

        public LuxScanItem(int id, List<IKilokoModel> model, string rawItem)
        {
            ID = id;
            IsActive = true;
            Models = model ?? throw new ArgumentNullException(nameof(model));
            Items = new List<ItemNumber>();
            AddItem(rawItem);
        }

        public LuxScanItem(int id, List<IKilokoModel> model)
        {
            ID = id;
            IsActive = true;
            Models = model ?? throw new ArgumentNullException(nameof(model));
            Items = new List<ItemNumber>();
        }

        public void AddItem(string rawLine)
        {
            var item = ItemNumberConverter.ConvertRawLine(rawLine);
            Items.Add(item);
        }

        public int ID { get; private set; }
        public bool IsActive { get; private set; }
        public List<IKilokoModel> Models { get; private set; }
        public List<ItemNumber> Items { get; private set; }
    }
}
