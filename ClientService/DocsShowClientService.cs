using PositioningLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TCPClient;

namespace ClientService
{
    public partial class DocsShowClientService : ServiceBase
    {
        public DocsShowClientService()
        {
            InitializeComponent();

            //az összes adobe process bezárása
            Positioning.CloseAllAdobeProcess();

            //a shortcut managing elintézése
            InItClientProgram.ShortcutOperations.SetStartUp();

            //ha kell akkor az ablak elrejtése
            InItClientProgram.InitMainProgram.Hide();

            //a szükséges paraméterek betöltése
            InItClientProgram.InitMainProgram.SetUpParams();
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
