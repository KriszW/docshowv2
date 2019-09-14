using IOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DocsShowServer
{
    public class Client : IClient
    {
        #region props and fields

        public bool SendingDatas { get; set; }

        public NetworkStream ClientStream { get; private set; }

        public int MaxErrNum { get { return 5; } }
        public IPAddress ClientIP { get; set; }
        public TcpClient ClientTCP { get; set; }

        public DataSend Sender { get; private set; }
        public DataRead Reader { get; private set; }

        public Thread LifeCheckerThread { get; private set; }

        public bool Checking { get; set; }

        #endregion

        #region constructor and deconstructor(disconnect)

        public Client(TcpClient client, string IP)
        {
            ClientIP = IPAddress.Parse(IP);

            ClientTCP = client;

            int timeout = 10000;

            ClientTCP.ReceiveTimeout = timeout;
            ClientTCP.SendTimeout = timeout;

            ClientStream = client.GetStream();
            Sender = new DataSend(this);
            Reader = new DataRead(this);

            if (CommonDatas.NetworkSpareLevel>100)
            {
                Checking = false;
            }
            else
            {
                Checking = true;
            }


            LifeCheckerThread = new Thread(CheckLife) {
                Name=$"checker_{ClientIP.ToString()}",
                IsBackground=true,
            };

            LifeCheckerThread.Start();

        }

        void CheckLife()
        {
            bool res = int.TryParse(System.Configuration.ConfigurationManager.AppSettings["OnlineActivityCheckRate"],out int timeout);

            if (res)
            {
                if (timeout<500)
                {
                    timeout = 500;
                }
            }
            else
            {
                timeout = 500;
            }

            while (Checking)
            {
                try
                {
                    LifeChecker.CheckPCLife(this);
                }
                catch (Exception)
                {
                    Disconnect();
                    break;
                }

                Thread.Sleep(timeout);
            }
        }

        public void Disconnect()
        {
            //már nem küld semmi adatot
            SendingDatas = false;

            //a kliens kiszedése a kliens listából
            ClientMethods.RemoveClient(this);

            try
            {
                //a kapcsolat bezárása
                ClientTCP.Close();
                //a stream bezárása
                ClientStream.Close();
            }
            catch (Exception)
            {
            }

            Checking = false;

            DisconnectOperations.SetKilokosOffline(ClientIP.ToString());
        }

        #endregion
    }
}
