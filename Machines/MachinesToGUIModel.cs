using KilokoModelLibrary;
using System;
using System.Linq;

namespace Machines
{
    public class MachinesToGUIModel
    {
        public MachinesToGUIModel()
        {
        }

        public MachinesToGUIModel(KilokoModel model)
        {
            Kiloko = model.Kiloko;
            var item = model.GetItemnamesModel();

            Itemnumber = item.Material;

            var left = item.PDFs.FirstOrDefault(p => p.Position == ItemNumberManager.MonitorPosition.Left);
            var right = item.PDFs.FirstOrDefault(p => p.Position == ItemNumberManager.MonitorPosition.Right);

            if (left != default)
            {
                DocLeft = left.FileName;
            }

            if (right != default)
            {
                DocRight = right.FileName; 
            }
        }

        public MachinesToGUIModel(int kiloko, string itemnumber, string docLeft, string docRight, bool active)
        {
            Kiloko = kiloko;
            Itemnumber = itemnumber ?? throw new ArgumentNullException(nameof(itemnumber));
            DocLeft = docLeft ?? throw new ArgumentNullException(nameof(docLeft));
            DocRight = docRight ?? throw new ArgumentNullException(nameof(docRight));
            Active = active;
        }

        public int Kiloko { get; set; }
        public string Itemnumber { get; set; }
        public string DocLeft { get; set; } = "Nincs PDF";
        public string DocRight { get; set; } = "Nincs PDF";
        public bool Active { get; set; }

        public string GetIP()
        {
            var machine = Machine.Machines.Where(m => m.KilokoNum == Kiloko).FirstOrDefault();

            return machine != default ? machine.IP : (default);
        }
    }
}