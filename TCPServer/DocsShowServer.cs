using EasyTcp;
using EasyTcp.Server;
using SendedModels;
using Settings.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

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
            Task.Run(() =>
            {
                var requestData = Serializer.Deserialize(e.Data);

                if (requestData is Request request)
                {
                    var requestManager = new RequestManager(request, new RequestMethods(request));

                    var reply = requestManager.ManageRequest();

                    if (reply != default && reply.Length > 0)
                    {
                        Server.Send(e.Socket, reply);
                    } 
                }
                else if(requestData is GetPDFModel getPDFModel)
                {
                    Server.DataReceived -= DataReceived;

                    var filePath = System.IO.Path.Combine(ServerSettings.CurrentSettings.Resources,getPDFModel.FileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        var data = System.IO.File.ReadAllBytes(filePath);

                        SendFile(e.Socket, data);
                    }

                    Server.DataReceived += DataReceived;
                }
            });
        }

        private void SendFile(Socket soc, byte[] data)
        {
            //mennyit bájtot küldött el
            var bytesSent = 0;
            //mennyi maradt
            var bytesLeft = data.Length;

            Server.Send(soc, data.Length);

            System.Threading.Thread.Sleep(500);

            //addig csinálja amíg maradt bájt
            while (bytesLeft > 0)
            {
                //a maradék lekérése
                var curDataSize = Math.Min(soc.ReceiveBufferSize - 2, bytesLeft);

                var actData = new byte[curDataSize];

                Array.Copy(data, bytesSent, actData, 0, curDataSize);

                Server.Send(soc, actData);

                //az elküldötthöz hozzáadás
                bytesSent += curDataSize;
                //a maradból elvevés
                bytesLeft -= curDataSize;

                System.Threading.Thread.Sleep(500);
            }
        }

        public void Start(ushort port)
        {
            Server.Start(System.Net.IPAddress.Any, port, 10, false, 10240);
        }

        public void CreateNewClient(Socket soc)
        {
            var client = new ServerClient(this, soc);

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