using CommonDataModel;
using IOs;
using System.Net.Sockets;

namespace Client
{
    class ConnectProtocol
    {
        //az saját ip cím elküldése
        static void SendIP()
        {
            if (DocsShowClient.ClientIP!="")
            {
                SendingOperations.WriteMSG(DocsShowClient.ClientIP);
            }
            else
            {
                DocsShowClient.ClientIP = FileOperations.GetLocalIPAddress();

                SendingOperations.WriteMSG(DocsShowClient.ClientIP);
            }
        }

        //a szerverre való csatlakozási protocol megkezdése
        static void TryConnectToServer()
        {
            string msg = ReadingMethods.ReadMSG();

            if (msg != "/ok") {
                throw new SocketException(10061);
            }
        }

        static void SetMaxCikkCount()
        {
            string msg = ReadingMethods.ReadMSG();

            bool res = int.TryParse(msg, out int maxCikk);

            if (res)
            {
                Datas.MaxCikkCount = maxCikk;
            }
            else
            {
                Datas.MaxCikkCount = 2;
            }

            SendingOperations.WriteMSG("/ok");
        }

        //a szerverre való csatlakozási protocol
        public static void MakeConnectProtocol()
        {
            TryConnectToServer();

            SendIP();

            SetMaxCikkCount();

            LifeChecker.CheckConnection();

            AdobeClosingMethod.CloseAllAdobe();
        }
    }
}
