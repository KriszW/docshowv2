using EndMethodsLibrary;
using InItClientProgram;
using PositioningLib;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPClient;

namespace DocsShowClient
{
    class Program
    { 
        static void Main(string[] args)
        {
            //a shortcut managing elintézése
            ShortcutOperations.SetStartUp();

            //ha kell akkor az ablak elrejtése
            InitMainProgram.Hide();

            //a szükséges paraméterek betöltése
            InitMainProgram.SetUpParams();

            Task.Delay(-1).Wait();
        }
    }
}
