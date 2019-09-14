using InItClientProgram;
using PositioningLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
        }
    }
}
