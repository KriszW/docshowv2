using PositioningLib;

namespace TCPClient
{
    public static class ClientStarter
    {
        public static DocsShowClient[] Clients { get; set; }

        public static void StartClients(int monitorCount)
        {
            Clients = new DocsShowClient[monitorCount];

            var successConnect = false;

            for (int i = 0; i < monitorCount; i++)
            {
                Clients[i] = new DocsShowClient(Datas.ServerIP, Datas.Port, i);

                while (successConnect == false)
                {
                    successConnect = Clients[i].Connect();
                }
            }
        }
    }
}