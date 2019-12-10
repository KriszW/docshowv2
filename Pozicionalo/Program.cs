using System;
using System.Web.Script.Serialization;
using EasyTcp.Client;
using InItClientProgram;
using PositioningLib;
using Settings.Client;
using TCPClient;

namespace DocsShowClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = "clientSettings.json";

            var settings = new JavaScriptSerializer().Deserialize<ClientSettings>(System.IO.File.ReadAllText(path));

            //az összes adobe process bezárása
            Positioning.CloseAllAdobeProcess();

            //a shortcut managing elintézése
            ShortcutOperations.SetStartUp();

            //ha kell akkor az ablak elrejtése
            InitMainProgram.Hide();

            //a szükséges paraméterek betöltése
            InitMainProgram.SetUpParams(settings);

            ClientStarter.StartClients(Datas.CountOfMonitors);

            foreach (var item in ClientStarter.Clients)
            {
                item.Client.OnDisconnect += Client_OnDisconnect;
            }

            Console.ReadLine();
        }

        private static void Client_OnDisconnect(object sender, EasyTcpClient e)
        {
            ClientStarter.StartClients(Datas.CountOfMonitors);
        }
    }
}