using PositioningLib;
using System.Threading.Tasks;

namespace TCPClient
{
    public static class ClientStarter
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static DocsShowClient[] Clients { get; set; }

        public static async void StartClients(int monitorCount)
        {
            _logger.Info($"A {monitorCount} darab {(monitorCount == 1 ? "TCP kliens" : "TCP kliensek")}  indítása");
            Clients = new DocsShowClient[monitorCount];

            for (int i = 0; i < monitorCount; i++)
            {
                _logger.Info($"A {i+1}. kliens inicializálása a {Datas.ServerIP}:{Datas.Port} szerverre");
                Clients[i] = new DocsShowClient(Datas.ServerIP, Datas.Port, i);
                _logger.Info($"A {i + 1}. kliens indítása");
                await Task.Run(()=> TryConnect(i));
                _logger.Info($"A {i + 1}. kliens elindítva");
            }
        }

        private static bool TryConnect(int i)
        {
            var successConnect = false;

            while (successConnect == false)
            {
                _logger.Debug($"A {i+1}. kliens csatlakozásának próbálása");
                successConnect = Clients[i].Connect();
            }

            _logger.Debug($"A {i + 1}. kliens sikeresen csatlakozott");
            return successConnect;
        }
    }
}