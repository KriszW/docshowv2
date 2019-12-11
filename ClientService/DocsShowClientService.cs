using PositioningLib;
using Settings.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TCPClient;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ClientService
{
    public partial class DocsShowClientService : ServiceBase
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DocsShowClientService()
        {
            InitializeComponent();

            var path = "clientSettings.json";

            _logger.Info($"A kliens beállítások kiolvasása a {path} fájlból");

            if (System.IO.File.Exists(path))
            {
                try
                {
                    _logger.Debug($"A {path} fájl beolvasása...");
                    var lines = System.IO.File.ReadAllText(path);
                    _logger.Debug($"A {path} fájl beolvasása kész");

                    _logger.Debug($"A {path} fájl lefordítása objektumá...");
                    var settings = new JavaScriptSerializer().Deserialize<ClientSettings>(lines);
                    _logger.Debug($"A {path} fájl lefordítása objektumá kész");

                    _logger.Debug($"A kliens adatok paramétereinek incializálása...");
                    //a szükséges paraméterek betöltése
                    InItClientProgram.InitMainProgram.SetUpParams(settings);
                    _logger.Debug($"A kliens adatok paramétereinek incializálása kész");
                }
                catch (Exception ex)
                {
                    _logger.Fatal($"Hiba történt a kliens adatok beolvasása közben a {path} fájlnál",ex);
                }

                //az összes adobe process bezárása
                Positioning.CloseAllAdobeProcess();
            }
        }

        protected override void OnStart(string[] args)
        {
            ConnectallClients();

            foreach (var item in ClientStarter.Clients)
            {
                item.Client.OnDisconnect += Client_OnDisconnect;
            }
        }

        private static void ConnectallClients()
        {
            _logger.Info($"{Datas.CountOfMonitors} kliens létrehozássa...");
            ClientStarter.StartClients(Datas.CountOfMonitors);
            _logger.Info($"{Datas.CountOfMonitors} kliens létrehozássa kész");
        }

        private void Client_OnDisconnect(object sender, EasyTcp.Client.EasyTcpClient e)
        {
            ConnectallClients();
        }

        private static void DisconnectAllClients()
        {
            _logger.Debug($"Az összes kliens lecsatlakoztatása...");
            foreach (var item in ClientStarter.Clients)
            {
                item.Disconnect();
            }
            _logger.Debug($"Az összes kliens lecsatlakoztatása kész");
        }

        protected override void OnStop()
        {
            DisconnectAllClients();
        }
    }
}
