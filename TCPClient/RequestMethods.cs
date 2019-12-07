using SendedModels;
using System;

namespace TCPClient
{
    public class RequestMethods : IRequestMethods
    {
        public RequestMethods(Request request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public Request Request { get; private set; }

        private static object _printLock = new object();

        public byte[] DocsSend(PositionModel model)
        {
            System.Threading.Thread.Sleep(model.MonitorIndex * 5000);
            lock (_printLock)
            {
                Position(model);
            }

            return default;
        }

        private static void Position(PositionModel model)
        {
            var client = ClientStarter.Clients[model.MonitorIndex];

            client.Monitor.AddNewModel(model);

            client.Monitor.Position();
        }

        public byte[] MachineModelSet(MachineSetModel model)
        {
            return null;
        }
    }
}