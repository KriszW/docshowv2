using ItemNumberManager;
using System.Collections.Generic;

namespace KilokoModelLibrary
{
    public interface IKilokoModel
    {
        string RawKiloko { get; }
        int Kiloko { get; }

        List<ItemNumber> Items { get; set; }

        string GetKilokoNumFromRaw();

        void AddNewCikk(string cikk, string name, string mask, string[] names);

        void AddNewCikk(ItemNumber item);

        void AddNewCikk(string rawLine);

        void DeleteAllCikk();

        ItemNumber GetItemnamesModel();
    }
}