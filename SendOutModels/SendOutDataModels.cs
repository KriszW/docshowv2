using KilokoModelLibrary;
using Machines;
using SendedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using TCPServer;

namespace SendOutModels
{
    public class SendOutDataModels
    {
        public SendOutDataModels(List<KilokoModel> models, List<Machine> machines)
        {
            Models = models;
            Machines = machines;
        }

        public List<KilokoModel> Models { get; set; }

        public List<Machine> Machines { get; set; }

        public IEnumerable<SendOutModel> GetModelsFromKilokok(List<KilokoModel> kilokok)
        {
            var output = new List<SendOutModel>();

            foreach (var item in kilokok)
            {
                output.Add(GetModel(item));
            }

            return output;
        }

        private IEnumerable<SendOutModel> GetModels()
        {
            var output = new List<SendOutModel>();

            foreach (var item in Models)
            {
                output.Add(GetModel(item));
            }

            return output;
        }

        private SendOutModel GetModel(KilokoModel item)
        {
            var machine = GetMachine(item.Kiloko);

            if (machine != default)
            {
                var model = GetModelFromMachine(new SendOutKilokoModel(machine, item));

                return new SendOutModel(machine, model);
            }

            return default;
        }

        private PositionModel GetModelFromMachine(SendOutKilokoModel soModel)
        {
            var model = new PositionModel(soModel.Machine.MonitorIndex);

            foreach (var material in soModel.Kiloko.Items)
            {
                foreach (var pdf in material.PDFs)
                {
                    var pdfModel = new PDFModelOverTCP(pdf.FileName, pdf.Position);
                    pdfModel.MonitorIndex = soModel.Machine.MonitorIndex;
                    model.PDF.Add(pdfModel);
                }
            }

            return model;
        }

        public void SendDatasOutToClient(SendOutModel model)
        {
            var machine = model.Machine;
            var data = Serializer.Serialize((object)model.Data);

            var client = (from tcpClient in DocsShowServer.DocsShow.Clients where tcpClient.Machine.IsSame(machine) select tcpClient).SingleOrDefault();

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

        private Machine GetMachine(int kilokoNum)
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