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
        private const int WaintingTimeForClient = 1000;
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            _logger.Info($"A {e.ClientIP} ipről kérés érkezett, feldolgozás megkezdése...");
            Task.Run(() =>
            {
                var requestData = Serializer.Deserialize(e.Data);

                if (requestData is Request request)
                {
                    _logger.Debug($"A {request.RequestID} kérés feldolgozásának megkezdése...");
                    var requestManager = new RequestManager(request, new RequestMethods(request));

                    _logger.Debug($"A {request.RequestID} kérés feldolgozása...");
                    var reply = requestManager.ManageRequest();
                    _logger.Debug($"A {request.RequestID} kérés feldolgozva");

                    if (reply != default && reply.Length > 0)
                    {
                        _logger.Debug($"A {request.RequestID} kéréshez a válasz küldése...");
                        Server.Send(e.Socket, reply);
                        _logger.Debug($"A {request.RequestID} kéréshez a válasz elküldve");
                    }

                    _logger.Debug($"A {request.RequestID} kérés feldolgozása befejezve");
                }
                else if(requestData is GetPDFModel getPDFModel)
                {
                    _logger.Debug($"A {e.ClientIP} nak a {getPDFModel.FileName} elküldése...");
                    ManageGETPDFModel(e, getPDFModel);
                    _logger.Debug($"A {e.ClientIP} nak a {getPDFModel.FileName} elküldve");
                }

                _logger.Info($"A {e.ClientIP} ipről kérés feldolgozása befejezve");
            });
        }

        private void ManageGETPDFModel(Message e, GetPDFModel getPDFModel)
        {
            Server.DataReceived -= DataReceived;

            SendPDF(e, getPDFModel);

            Server.DataReceived += DataReceived;
        }

        private void SendPDF(Message e, GetPDFModel getPDFModel)
        {
            var filePath = System.IO.Path.Combine(ServerSettings.CurrentSettings.Resources, getPDFModel.FileName);

            if (System.IO.File.Exists(filePath))
            {
                _logger.Info($"A {e.ClientIP}-nak a {filePath} elküldése....");
                var data = System.IO.File.ReadAllBytes(filePath);

                SendFile(e.Socket, data);
                _logger.Info($"A {e.ClientIP}-nak a {filePath} elküldve");
            }
            else
            {
                _logger.Error($"A {e.ClientIP} érvénytelen fájlt kért, a {filePath} nem létezik");
            }
        }

        private void SendFile(Socket soc, byte[] data)
        {
            //mennyit bájtot küldött el
            var bytesSent = 0;
            //mennyi maradt
            var bytesLeft = data.Length;
            _logger.Debug($"A {soc.RemoteEndPoint.ToString()} kliensnek a {data.Length} fájl mérétének elküldése...");

            Server.Send(soc, data.Length);

            _logger.Debug($"A {soc.RemoteEndPoint.ToString()} kliensnek a {data.Length} fájl mérétének elküldve");

            _logger.Debug($"A {soc.RemoteEndPoint.ToString()} kliensnél {WaintingTimeForClient} ms várása, amíg a {soc.RemoteEndPoint.ToString()} kliens kezeli a megkapott adatot...");
            System.Threading.Thread.Sleep(WaintingTimeForClient);
            _logger.Debug($"A {soc.RemoteEndPoint.ToString()} kliensnél {WaintingTimeForClient} ms megvárva a {soc.RemoteEndPoint.ToString()} kliensnél");
            //addig csinálja amíg maradt bájt
            while (bytesLeft > 0)
            {
                //a maradék lekérése
                var curDataSize = Math.Min(soc.ReceiveBufferSize - 2, bytesLeft);
                _logger.Debug($"Az elküldendő adat mennyiség eldőntése a {soc.RemoteEndPoint.ToString()} kliensnek: {curDataSize}");

                var actData = new byte[curDataSize];

                Array.Copy(data, bytesSent, actData, 0, curDataSize);

                _logger.Debug($"Az {curDataSize} byte adat elküldése a {soc.RemoteEndPoint.ToString()} kliensnek...");
                Server.Send(soc, actData);
                _logger.Debug($"Az {curDataSize} byte adat elküldve a {soc.RemoteEndPoint.ToString()} kliensnek");

                //az elküldötthöz hozzáadás
                bytesSent += curDataSize;
                //a maradból elvevés
                bytesLeft -= curDataSize;

                _logger.Debug($"A {soc.RemoteEndPoint.ToString()} kliensnél {WaintingTimeForClient} ms várása, amíg a kliens kezeli a megkapott adatot...");
                System.Threading.Thread.Sleep(WaintingTimeForClient);
                _logger.Debug($"A {soc.RemoteEndPoint.ToString()} kliensnél {WaintingTimeForClient} ms megvárva");
            }
        }

        public void Start(ushort port)
        {
            _logger.Info($"Szerver indítása a {port} porton...");
            Server.Start(System.Net.IPAddress.Any, port, 10, false, 10240);
            _logger.Info($"Szerver indítása a {port} porton elindítva");
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