using Machines;
using System.Net.Sockets;

namespace TCPServer
{
    public class ServerClient
    {
        public Socket ClientSocket { get; set; }
        public DocsShowServer Server { get; private set; }

        public Machine Machine { get; set; }

        public ServerClient(DocsShowServer server, Socket socket)
        {
            Server = server;
            ClientSocket = socket;
        }
    }
}