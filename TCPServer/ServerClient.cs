using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using KilokoModelLibrary;
using Machines;

namespace TCPServer
{
    public class ServerClient
    {
        public Socket ClientSocket { get; set; }
        public DocsShowServer Server { get; private set; }

        public MachineModel Machine { get; set; }

        public ServerClient(DocsShowServer server, Socket socket)
        {
            Server = server;
            ClientSocket = socket;
        }
    }
}
