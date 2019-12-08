using EasyTcp.Client;
using PositioningLib;
using SendedModels;
using System;
using System.Threading.Tasks;
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

        public DocsShowClient(string ip, ushort port, int monitorIndex)
        {
            ServerIP = ip;
            ServerPort = port;

            Client = new EasyTcpClient();

            MonitorIndex = monitorIndex;

            Monitor = new MonitorInfo(monitorIndex);
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
                var request = (Request)Serializer.Deserialize(e.Data);
                var requestManager = new RequestManager(request, new RequestMethods(request));

                var reply = requestManager.ManageRequest();

                if (reply != default && reply.Length > 0)
                {
                    Client.Send(reply);
                }
            });
        }
    }
}