using CommonDataModel;
using IOs;
using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class SetUpOperations
    {
        public static void SetUpDefaultDatas()
        {
            //az alap adatok beállítása
            DocsShowClient.ClientIP = FileOperations.GetLocalIPAddress();
            DocsShowClient.ClientTCP = new TcpClient();

            try
            {
                //a a szerver IP beállítása
                DocsShowClient.ServerIP = IPAddress.Parse(Datas.CommandServerIP);
            }
            catch (FormatException ex)
            {
                // érvénytelen IP cím, akkor a program leállítása
                Console.WriteLine($"{ DateTime.Now.ToString()}:{ ex.Message}");
                Console.WriteLine("Nyomj entert a kilépéshez...");

                Console.ReadLine();
                Environment.Exit(20);
            }
        }
    }
}
