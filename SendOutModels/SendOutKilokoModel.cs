using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KilokoModelLibrary;
using Machines;

namespace SendOutModels
{
    public class SendOutKilokoModel
    {
        public SendOutKilokoModel(Machine machine, KilokoModel kiloko)
        {
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
            Kiloko = kiloko ?? throw new ArgumentNullException(nameof(kiloko));
        }

        public Machines.Machine Machine { get; set; }
        public KilokoModelLibrary.KilokoModel Kiloko { get; set; }
    }
}
