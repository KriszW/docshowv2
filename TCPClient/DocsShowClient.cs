using EasyTcp.Client;
using PositioningLib;
using SendedModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using TCPServer;

namespace TCPClient
{
    public class DocsShowClient
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EasyTcpClient Client { get; set; }

        public int MonitorIndex { get; set; }

        public string ServerIP { get; set; }
        public ushort ServerPort { get; set; }

        public ushort MyPort { get; set; }
        public string MyIP { get; set; }

        public MonitorInfo Monitor { get; set; }
        public Request CurrentRequest { get; private set; }

        public DocsShowClient(string ip, ushort port, int monitorIndex)
        {
            ServerIP = ip;
            ServerPort = port;

            Client = new EasyTcpClient();

            MonitorIndex = monitorIndex;

            Monitor = new MonitorInfo(monitorIndex);
            ProcessOperations.OnPDFNotFound += ManageNotFoundPDF;
        }

        public void Disconnect()
        {
            _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szevernnél, a {MonitorIndex}. monitorhoz csatolt kliens lecsatlakozott");
            Client.Disconnect(false);
        }

        public bool Connect()
        {
            _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliens csatlakozásának elkezdése");
            Client.Connect(ServerIP, ServerPort, new TimeSpan(0, 0, 30), ushort.MaxValue);

            if (Client.IsConnected)
            {
                _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliens sikeresen csatlakozott");
                _logger.Debug($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliens incializálása");
                MyPort = ushort.Parse(Client.Socket.LocalEndPoint.ToString().Split(':')[1]);
                MyIP = Client.Socket.LocalEndPoint.ToString().Split(':')[0];

                Client.DataReceived += Client_DataReceived;
                Client.OnError += Client_OnError;

                SetupMachineData();
            }
            else
            {
                _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliens nem tudott csatlakozni");
            }

            return Client.IsConnected;
        }

        private void Client_OnError(object sender, Exception e)
        {
            _logger.Fatal($"Hiba történt a {Datas.ServerIP}:{Datas.Port} szeverrel, a {MonitorIndex}. monitorhoz csatolt kliensnél a kommunikáció közben",e);
        }

        private Request CreateRequest()
        {
            var machineModel = new MachineSetModel(MonitorIndex, MyIP, MyPort);

            var request = new Request();
            request.RequestID = request.GenerateID();
            request.Data = Serializer.Serialize(machineModel);
            request.Command = RequestType.MachineModelSet;
            request.CreatedDate = DateTime.Now;
            request.State = RequestState.Created;

            return request;
        }

        private void SetupMachineData()
        {
            _logger.Debug($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliens gép adatainak elküldése");
            Client.Send(Serializer.Serialize(CreateRequest()));
        }

        private void Client_DataReceived(object sender, EasyTcp.Message e)
        {
            Task.Run(() =>
            {
                _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliensre adat érkezett a szevertől");
                CurrentRequest = (Request)Serializer.Deserialize(e.Data);
                var requestManager = new RequestManager(CurrentRequest, new RequestMethods(CurrentRequest));

                try
                {
                    _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliensnek a {CurrentRequest.RequestID} kérés teljesítésének elkezdése");
                    var reply = requestManager.ManageRequest();

                    SendReply(reply);
                }
                catch (Exception ex)
                {
                    _logger.Error($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliensnek a {CurrentRequest.RequestID} kérés teljesítésének közben hiba lépett fel", ex);
                }

                _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliensnek a {CurrentRequest.RequestID} kérés lefutott");

                CurrentRequest = default;
            });
        }

        private void SendReply(byte[] reply)
        {
            if (reply != default && reply.Length > 0)
            {
                _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliensnek a {CurrentRequest.RequestID} kéréshez a válasz visszaküldése...");
                Client.Send(reply);
                _logger.Info($"A {Datas.ServerIP}:{Datas.Port} szeverhez, a {MonitorIndex}. monitorhoz csatolt kliensnek a {CurrentRequest.RequestID} kéréshez a válasz visszaküldése sikeres volt");
            }
        }

        private object _getpdfLock = new object();

        public void ManageNotFoundPDF(object sender, PDFNotFoundException e)
        {
            GetPDF(e.FileName);
        }

        public void GetPDF(string fileName)
        {
            var time = System.Text.Encoding.UTF8.GetBytes(fileName)[0] * new Random().Next(10);
            _logger.Debug($"{time} ms várakozása a PDF lekérése előtt a {MonitorIndex}. kliensre");
            System.Threading.Thread.Sleep(time);

            lock (_getpdfLock)
            {
                _logger.Debug($"{MonitorIndex}. kliens adatfogadásának leáálítása");
                Client.DataReceived -= Client_DataReceived;

                _logger.Debug($"{MonitorIndex}. kliens PDF lekérő request elküldése");
                var data = Client.SendAndGetReply(Serializer.Serialize(new GetPDFModel() { FileName = fileName }), new TimeSpan(1, 0, 0));

                var bufferSize = Client.Socket.ReceiveBufferSize;

                var bytesLeft = data.GetInt;
                _logger.Debug($"{MonitorIndex}. kliens {fileName} PDF mérete: {bytesLeft} byte");

                var fileData = new List<byte>();
                var bytesRead = 0;

                while (bytesLeft > 0)
                {
                    var curDataSize = Math.Min(bufferSize, bytesLeft);

                    if (curDataSize > 0)
                    {
                        _logger.Debug($"A {MonitorIndex}. kliensnél {fileName} fájlhoz adat olvasásra készülés {curDataSize} méretben");

                        var msg = Client.SendAndGetReply("ok", new TimeSpan(1, 0, 0));

                        _logger.Debug($"{MonitorIndex}. kliensnél {fileName} fájlhoz adat olvasva {curDataSize} méretben");

                        bytesLeft -= curDataSize;
                        bytesRead += curDataSize;

                        _logger.Debug($"{MonitorIndex}. kliensnél {fileName} fájlhoz még {bytesLeft} byte maradt, {bytesRead} byte kiolvasva");

                        fileData.AddRange(msg.Data);
                    }
                }

                var path = System.IO.Path.Combine(Datas.PDFsPath, fileName);
                _logger.Debug($"{MonitorIndex}. kliensnél {fileName} fájlhoz az összes adat kiolvasva {bytesRead} byte méretben, kiírás a lemezre, elérése: {path}");

                System.IO.File.WriteAllBytes(path, fileData.ToArray());

                Client.DataReceived += Client_DataReceived;
            }
        }

        public override string ToString() => $"{MyIP}:{MyPort}";
    }
}