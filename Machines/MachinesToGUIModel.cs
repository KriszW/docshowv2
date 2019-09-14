using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (item.PDFs.Count == 1)
            {
                DocLeft = item?.PDFs[0]?.FileName;
            }
            else if(item.PDFs.Count == 2)
            {
                DocRight = item?.PDFs[1]?.FileName;
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
            var machine = MachineModel.Machines.Where(m=> m.KilokoNum==Kiloko).FirstOrDefault();

            return machine != default ? machine.IP : (default);
        }
    }
}
