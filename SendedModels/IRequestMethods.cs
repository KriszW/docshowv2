using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendedModels
{
    public interface IRequestMethods
    {
        Request Request { get; }
        byte[] DocsSend(PositionModel model);
        byte[] MachineModelSet(MachineSetModel model);
    }
}
