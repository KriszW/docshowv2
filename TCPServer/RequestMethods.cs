using SendedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class RequestMethods : IRequestMethods
    {
        public RequestMethods(Request request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }
        public Request Request { get; private set; }

        public byte[] DocsSend(PositionModel model)
        {
            return default;
        }

        public byte[] MachineModelSet(MachineSetModel model)
        {
            var client = (from c in DocsShowServer.DocsShow.Clients
                          where c.ClientSocket.RemoteEndPoint.ToString().StartsWith(model.IP) && 
                          c.ClientSocket.RemoteEndPoint.ToString().EndsWith(model.Port.ToString())
                          select c).SingleOrDefault();

            if (client != null)
            {
                var machine = (from machineModel in Machines.MachineModel.Machines where machineModel.IP == model.IP && machineModel.MonitorIndex == model.MonitorIndex select machineModel).SingleOrDefault();

                if (machine != default)
                {
                    client.Machine = machine;
                }
            }

            return default;
        }
    }
}
