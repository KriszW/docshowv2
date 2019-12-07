using Machines;
using SendedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendOutModels
{
    public class SendOutModel
    {
        public SendOutModel(Machine machine, PositionModel data)
        {
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public Machines.Machine Machine { get; set; }
        public PositionModel Data { get; set; }
    }
}
