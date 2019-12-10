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

namespace ClientService
{
    public partial class DocsShowClientService : ServiceBase
    {
        public DocsShowClientService()
        {
            InitializeComponent();

            var path = "clientSettings.json";

            if (System.IO.File.Exists(path))
            {
                var settings = new JavaScriptSerializer().Deserialize<ClientSettings>(System.IO.File.ReadAllText(path));

                //az összes adobe process bezárása
                Positioning.CloseAllAdobeProcess();

                //a szükséges paraméterek betöltése
                InItClientProgram.InitMainProgram.SetUpParams(settings);
            }
        }

        protected override void OnStart(string[] args)
        {
            ClientStarter.StartClients(Datas.CountOfMonitors);

            foreach (var item in ClientStarter.Clients)
            {
                item.Client.OnDisconnect += Client_OnDisconnect;
            }
        }

        private void Client_OnDisconnect(object sender, EasyTcp.Client.EasyTcpClient e)
        {
            ClientStarter.StartClients(Datas.CountOfMonitors);
        }

        protected override void OnStop()
        {
            foreach (var item in ClientStarter.Clients)
            {
                item.Disconnect();   
            }
        }
    }
}
