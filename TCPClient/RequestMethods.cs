using SendedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
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
            var client = ClientStarter.Clients[model.MonitorIndex];

            client.Monitor.AddNewModel(model);

            client.Monitor.Position();

            return default;
        }

        public byte[] MachineModelSet(MachineSetModel model)
        {
            return null;
        }
    }
}
