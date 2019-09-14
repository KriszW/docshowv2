using CommonDataModel;
using System;
using System.Threading;

namespace Client
{
    class ConnectOperations
    {
        public static void MakeConnect()
        {
            //a connect végrehajtása
            SetUpOperations.SetUpDefaultDatas();

            Console.WriteLine($"{DateTime.Now.ToString()}:A kliens várakozik a doksi kiszolgáló szerverre {Datas.CommandServerIP}:{Datas.CommandPort}");

            ConnectToServer();

            ConnectProtocol.MakeConnectProtocol();
        }

        static void ConnectToServer()
        {
            //a csatlakozás folytonos próbálkozása
            try
            {
                DocsShowClient.ClientTCP.Connect(DocsShowClient.ServerIP, Datas.CommandPort);
            }
            catch(Exception)
            {
                //ha nem sikerül újra
                Thread.Sleep(100);
                ConnectToServer();
            }
            //ha sikerült akkor a tcp paraméterek beállítsa
            SetUpTcpParams();
        }

        static void SetUpTcpParams()
        {
            //a stream elérésé
            DocsShowClient.ClientStream = DocsShowClient.ClientTCP.GetStream();


            const int timeout = 10000;

            DocsShowClient.ClientTCP.ReceiveTimeout = timeout;
            DocsShowClient.ClientTCP.SendTimeout = timeout;
        }
    }
}
