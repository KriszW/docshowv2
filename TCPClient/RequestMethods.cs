using PositioningLib;
using SendedModels;
using System;

namespace TCPClient
{
    public class RequestMethods : IRequestMethods
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestMethods(Request request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public Request Request { get; private set; }

        private static object _posLock = new object();

        public byte[] DocsSend(PositionModel model)
        {
            System.Threading.Thread.Sleep(model.MonitorIndex * 5000);
            lock (_posLock)
            {
                Position(model);
            }

            return default;
        }

        private static void Position(PositionModel model)
        {
            var client = ClientStarter.Clients[model.MonitorIndex];

            client.Monitor.Position(model);
        }

        public byte[] MachineModelSet(MachineSetModel model)
        {
            return null;
        }
    }
}