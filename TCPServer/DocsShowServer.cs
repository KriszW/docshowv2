using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EasyTcp;
using EasyTcp.Server;
using Machines;
using SendedModels;

namespace TCPServer
{
    public class DocsShowServer
    {
        public static DocsShowServer DocsShow { get; set; }
        public EasyTcpServer Server { get; private set; }

        public List<ServerClient> Clients { get; set; }

        public DocsShowServer()
        {
            Server = new EasyTcpServer();
            Server.DataReceived += DataReceived;
            Clients = new List<ServerClient>();
        }

        private void DataReceived(object sender, Message e)
        {
            Task.Run(()=> {
                var request = (Request)Serializer.Deserialize(e.Data);
                var requestManager = new RequestManager(request, new RequestMethods(request));

                var reply = requestManager.ManageRequest();

                if (reply != default && reply.Length > 0)
                {
                    Server.Send(e.Socket,reply);
                }
            });
        }

        public void Start(ushort port)
        {
            Server.Start(System.Net.IPAddress.Any, port, 10, false, 10240);
        }

        public void CreateNewClient(Socket soc)
        {
            var client = new ServerClient(this,soc);

            Clients.Add(client);
        }

        public void RemoveClient(Socket soc)
        {
            var client = Clients.Where(c => c.ClientSocket == soc).FirstOrDefault();

            if (client != default)
            {
                Clients.Remove(client);
            }
        }
    }
}
