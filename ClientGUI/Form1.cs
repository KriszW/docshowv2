using InItClientProgram;
using PositioningLib;
using System;
using System.Windows.Forms;
using TCPClient;

namespace ClientGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //a shortcut managing elintézése
            ShortcutOperations.SetStartUp();

            //ha kell akkor az ablak elrejtése
            InitMainProgram.Hide();

            //a szükséges paraméterek betöltése
            InitMainProgram.SetUpParams();
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
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
    }
}