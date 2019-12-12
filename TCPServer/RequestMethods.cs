using SendedModels;
using System;
using System.Linq;

namespace TCPServer
{
    public class RequestMethods : IRequestMethods
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            _logger.Debug($"A {model.IP+":"+model.Port} klienshez egy asztal hozzárendelése");

            var client = (from c in DocsShowServer.DocsShow.Clients
                          where c.ClientSocket.RemoteEndPoint.ToString().StartsWith(model.IP) &&
                          c.ClientSocket.RemoteEndPoint.ToString().EndsWith(model.Port.ToString())
                          select c).FirstOrDefault();



            if (client != null)
            {
                var machine = (from machineModel in Machines.Machine.Machines where machineModel.IP == model.IP && machineModel.MonitorIndex == model.MonitorIndex select machineModel).SingleOrDefault();

                if (machine != default)
                {
                    _logger.Debug($"A {model.IP + ":" + model.Port} klienshez {machine.KilokoNum} hozzárendelése a {model.MonitorIndex}. monitorhoz");
                    client.Machine = machine;
                }
                else
                {
                    _logger.Error($"A {model.IP + ":" + model.Port} klienshez egy asztal hozzárendelése sikertlen volt, mert nem létezett megfelelő asztal a kliens monitroszámához, és IP címéhez");
                }
            }
            else
            {
                _logger.Error($"A {model.IP + ":" + model.Port} klienshez egy asztal hozzárendelése sikertlen volt, mert nem létezett megfelelő kliens szerverre felcsatlakozva, ellenőrízd a gép setting fájlt");
            }

            return default;
        }
    }
}