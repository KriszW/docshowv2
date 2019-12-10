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
            Client.Disconnect(false);
        }

        public bool Connect()
        {
            Client.Connect(ServerIP, ServerPort, new TimeSpan(0, 0, 30), ushort.MaxValue);

            if (Client.IsConnected)
            {
                MyPort = ushort.Parse(Client.Socket.LocalEndPoint.ToString().Split(':')[1]);
                MyIP = Client.Socket.LocalEndPoint.ToString().Split(':')[0];

                Client.DataReceived += Client_DataReceived;
                Client.OnError += Client_OnError;

                SetupMachineData();
            }

            return Client.IsConnected;
        }

        private void Client_OnError(object sender, Exception e)
        {
            //logger implementálása
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
            Client.Send(Serializer.Serialize(CreateRequest()));
        }

        private void Client_DataReceived(object sender, EasyTcp.Message e)
        {
            Task.Run(() =>
            {
                CurrentRequest = (Request)Serializer.Deserialize(e.Data);
                var requestManager = new RequestManager(CurrentRequest, new RequestMethods(CurrentRequest));

                var reply = default(byte[]);

                try
                {
                     reply = requestManager.ManageRequest();
                }
                catch (Exception ex)
                {

                }

                if (reply != default && reply.Length > 0)
                {
                    Client.Send(reply);
                }

                CurrentRequest = default;
            });
        }

        private object _getpdfLock = new object();

        public void ManageNotFoundPDF(object sender, PDFNotFoundException e)
        {
            GetPDF(e.FileName);
        }

        public void GetPDF(string fileName)
        {
            var time = System.Text.Encoding.UTF8.GetBytes(fileName)[0] * new Random().Next(10);
            System.Threading.Thread.Sleep(time);
            lock (_getpdfLock)
            {
                Client.DataReceived -= Client_DataReceived;

                var data = Client.SendAndGetReply(Serializer.Serialize(new GetPDFModel() { FileName = fileName }), new TimeSpan(1, 0, 0));

                var bufferSize = Client.Socket.ReceiveBufferSize;

                var bytesLeft = data.GetInt;
                Debug.WriteLine($"{fileName} mérete: {bytesLeft} byte");
                var fileData = new List<byte>();
                var bytesRead = 0;

                while (bytesLeft > 0)
                {
                    var curDataSize = Math.Min(bufferSize, bytesLeft);

                    if (curDataSize > 0)
                    {
                        Debug.WriteLine($"{fileName} fájlhoz adat olvasásra készülés {curDataSize} méretben");

                        var msg = Client.SendAndGetReply("ok", new TimeSpan(1, 0, 0));

                        Debug.WriteLine($"{fileName} fájlhoz adat olvasva {curDataSize} méretben");

                        bytesLeft -= curDataSize;
                        bytesRead += curDataSize;

                        Debug.WriteLine($"{fileName} fájlhoz még {bytesLeft} byte maradt, {bytesRead} byte kiolvasva");

                        fileData.AddRange(msg.Data);
                    }
                }

                var path = System.IO.Path.Combine(Datas.PDFsPath, fileName);
                Debug.WriteLine($"{fileName} fájlhoz az összes adat kiolvasva {bytesRead} byte méretben, kiírás a lemezre, elérése: {path}");

                System.IO.File.WriteAllBytes(path, fileData.ToArray());

                Client.DataReceived += Client_DataReceived;
            }
        }
    }
}