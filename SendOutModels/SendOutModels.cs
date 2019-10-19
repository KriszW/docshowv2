using KilokoModelLibrary;
using Machines;
using SendedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using TCPServer;

namespace SendOutModels
{
    public class SendOutModels
    {
        public List<KilokoModel> Models { get; set; }

        public List<MachineModel> Machines { get; set; }

        public SendOutModels()
        {
        }

        private Dictionary<MachineModel, PositionModel> GetModels()
        {
            var output = new Dictionary<MachineModel, PositionModel>();

            foreach (var item in Models)
            {
                var machine = GetMachine(item.Kiloko);

                if (machine != default)
                {
                    var model = GetModelFromMachine(machine, item);

                    output.Add(machine, model);
                }
            }

            return output;
        }

        private PositionModel GetModelFromMachine(MachineModel machine, KilokoModel item)
        {
            var model = new PositionModel(item.Position, machine.MonitorIndex);

            foreach (var material in item.Items)
            {
                foreach (var pdf in material.PDFs)
                {
                    var pdfModel = new PDFModel(pdf.FileName);
                    pdfModel.MonitorIndex = machine.MonitorIndex;
                    model.PDF.Add(pdfModel);
                }
            }

            return model;
        }

        private void SendDatasOutToClient(KeyValuePair<MachineModel, PositionModel> item)
        {
            var machine = item.Key;
            var data = Serializer.Serialize((object)item.Value);

            var client = (from tcpClient in DocsShowServer.DocsShow.Clients where tcpClient.Machine.Equals(machine) select tcpClient).SingleOrDefault();

            if (client != default)
            {
                var request = new Request();
                request.RequestID = request.GenerateID();
                request.CreatedDate = DateTime.Now;
                request.Data = data;
                request.Command = RequestType.DocsSend;
                request.State = RequestState.Created;

                var requestData = Serializer.Serialize(request);

                DocsShowServer.DocsShow.Server.Send(client.ClientSocket, requestData);
            }
        }

        public void Send()
        {
            var models = GetModels();

            foreach (var item in models)
            {
                SendDatasOutToClient(item);
            }
        }

        private MachineModel GetMachine(int kilokoNum)
        {
            foreach (var item in Machines)
            {
                if (item.KilokoNum == kilokoNum)
                {
                    return item;
                }
            }

            return default;
        }
    }
}