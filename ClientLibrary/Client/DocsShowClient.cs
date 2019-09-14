using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using IOs;
using CommonDataModel;

namespace Client
{
    class DocsShowClient
    {
        #region fields and properties

        public static bool InPDF { get; set; }
        public static string ClientIP { get; set; }

        public static TcpClient ClientTCP { get; set; }
        public static NetworkStream ClientStream { get; set; }
        public static IPAddress ServerIP { get; set; }

        public static List<string> Standards { get; set; }
        public static string[,] StandardsForGoodPosition { get; set; }

        #endregion

        public static void Connect()
        {
            NativeDocsShowClientMethods.Connect();
        }
        public static void Disconnect()
        {
            //ha a kapcsolat közben hiba lépett fel
            ClientTCP.Close();

            string msg = $"{ClientIP} lecsatlakozott ekkor {DateTime.Now} innen { ServerIP}";

            Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
            Logger.MakeLog(msg);
        }


        public static void ReadMSG()
        {
            NativeDocsShowClientMethods.ReadMethod();
        }
    }
}
