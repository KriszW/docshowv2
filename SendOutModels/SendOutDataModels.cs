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
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SendOutDataModels(List<KilokoModel> models, List<Machine> machines)
        {
            Models = models;
            Machines = machines;
        }

        public List<KilokoModel> Models { get; set; }

        public List<Machine> Machines { get; set; }

        public IEnumerable<SendOutModel> GetModelsFromKilokok(List<KilokoModel> kilokok)
        {
            _logger.Debug($"A {kilokok.Count} darab kilőkőhöz a megfelelő modellek lekérése...");
            var output = new List<SendOutModel>();

            foreach (var item in kilokok)
            {
                output.Add(GetModel(item));
            }

            _logger.Debug($"A {kilokok.Count} darab kilőkőhöz a megfelelő modellek lekérve, {output.Count} darab model");
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
            _logger.Debug($"A {item.Kiloko} asztalhoz a gép adatok lekérése....");
            var machine = GetMachine(item.Kiloko);
            _logger.Debug($"A {item.Kiloko} asztalhoz a gép adatok lekérve");

            if (machine != default)
            {
                _logger.Debug($"A {item.Kiloko} asztalhoz a gép adat valid volt, a model készítése...");
                var model = GetModelFromMachine(new SendOutKilokoModel(machine, item));
                _logger.Debug($"A {item.Kiloko} asztalhoz a gép adat valid volt, a model elkészítve");

                return new SendOutModel(machine, model);
            }

            _logger.Error($"A {item.Kiloko} asztalhoz a gép adat invalid volt");
            return default;
        }

        private PositionModel GetModelFromMachine(SendOutKilokoModel soModel)
        {
            _logger.Debug($"A kiküldendő model elkészítése a {soModel.Machine.IP} címmel és a {soModel.Machine.MonitorIndex}. monitorra");
            var model = new PositionModel(soModel.Machine.MonitorIndex);

            foreach (var material in soModel.Kiloko.Items)
            {
                foreach (var pdf in material.PDFs)
                {
                    var pdfModel = new PDFModelOverTCP(pdf.FileName, pdf.Position);
                    pdfModel.MonitorIndex = soModel.Machine.MonitorIndex;
                    _logger.Debug($"A kiküldendő modelnek {pdfModel.PDFFileName} fájl hozzáadva a {soModel.Machine.IP} címmel és a {soModel.Machine.MonitorIndex}. monitorra");
                    model.PDF.Add(pdfModel);
                }
            }

            return model;
        }

        public void SendDatasOutToClient(SendOutModel model)
        {
            var machine = model.Machine;
            _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra az adatok bájtá alakítása...");
            var data = Serializer.Serialize((object)model.Data);
            _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra az adatok bájtá alakítva");

            _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a Socket lekérése");
            var client = (from tcpClient in DocsShowServer.DocsShow.Clients where (tcpClient.Machine != default ? tcpClient.Machine.IsSame(machine) : false) select tcpClient).FirstOrDefault();
            _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a Socket lekérve");

            if (client != default)
            {
                _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a PDF kezelő Request elkészítése");
                var request = CreateRequest(data);

                _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a request adatok bájtá alakítása...");
                var requestData = Serializer.Serialize(request);
                _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a request adatok bájtá alakítva");

                _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a PDF kezelő request elküldése...");
                DocsShowServer.DocsShow.Server.Send(client.ClientSocket, requestData);
                _logger.Debug($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a PDF kezelő request elküldve");
            }
            else
            {
                _logger.Error($"A {model.Machine.IP} címmel a {model.Machine.MonitorIndex}. monitorra a szerver nem tudott valós Socketet adni");
            }
        }

        private Request CreateRequest(byte[] data)
        {
            var request = new Request();
            request.RequestID = request.GenerateID();
            request.CreatedDate = DateTime.Now;
            request.Data = data;
            request.Command = RequestType.DocsSend;
            request.State = RequestState.Created;
            return request;
        }

        public void Send()
        {
            _logger.Debug($"Az összes géphez a modelek kiküldése");
            var models = GetModels();

            foreach (var item in models)
            {
                _logger.Debug($"A {item.Machine.KilokoNum} asztalhoz a PDFek kiküldése...");
                SendDatasOutToClient(item);
                _logger.Debug($"A {item.Machine.KilokoNum} asztalhoz a PDFek kiküldve");
            }
        }

        private Machine GetMachine(int kilokoNum)
        {
            foreach (var item in Machines)
            {
                if (item.KilokoNum == kilokoNum)
                {
                    _logger.Debug($"A {kilokoNum} asztalhoz az {item.IP} iphez a {item.MonitorIndex}. monitor gép lekérve");
                    return item;
                }
            }

            _logger.Error($"A {kilokoNum} asztalhoz nem tudtunk gépet lekérni a szervertől");
            return default;
        }
    }
}