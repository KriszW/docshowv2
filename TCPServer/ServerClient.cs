using Machines;
using System.Net.Sockets;
using System.Linq;

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
            var ip = socket.RemoteEndPoint.ToString().Split(':')[0];
            Machine = Machine.Machines.FirstOrDefault(m=> m.IP == ip);
        }
    }
}