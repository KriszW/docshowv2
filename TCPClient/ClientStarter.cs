using PositioningLib;

namespace TCPClient
{
    public static class ClientStarter
    {
        public static DocsShowClient[] Clients { get; set; }

        public static void StartClients(int monitorCount)
        {
            Clients = new DocsShowClient[monitorCount];

            for (int i = 0; i < monitorCount; i++)
            {
                Clients[i] = new DocsShowClient(Datas.ServerIP, Datas.Port, i);
                var res = TryConnect(i);
            }
        }

        private static bool TryConnect(int i)
        {
            var successConnect = false;

            while (successConnect == false)
            {
                successConnect = Clients[i].Connect();
            }

            return successConnect;
        }
    }
}