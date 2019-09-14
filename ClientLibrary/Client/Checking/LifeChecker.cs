using CommonDataModel;
using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class LifeChecker
    {
        //a csatlakozás elvégézése
        public static void CheckConnection()
        {
            string msg = ReadingMethods.ReadMSG();

            if (msg == "/okdatas") {
                //sikeres kapcsolodás
                Console.WriteLine($"{DateTime.Now.ToString()}:Sikeres kapcsolodás a {Datas.CommandServerIP}:{Datas.CommandPort} doksi kiszolgáló szerverhez");
                SendingOperations.WriteMSG("/connected");
            }
            else if (msg == "/inUse") {
                //a szerverre már felcsatlakoztak ezzel az IPvel
                throw new SocketException(10048);
            }
            else {
                //a válasz nem volt értelmezhető
                DocsShowClient.Disconnect();
                throw new ApplicationException("A szerver visszautasította az adatok kezelését");
            }
        }
    }
}
