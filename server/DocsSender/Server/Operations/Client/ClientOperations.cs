using System.Threading;

namespace DocsShowServer
{
    class ClientOperations
    {
        //létezik-e a client
        public static bool ValidClient(string ip)
        {
            Client client = ClientMethods.GetClient(ip);

            if (client==null) {
                return false;
            }
            else {
                return true;
            }
        }

        //várja meg amíg connectel valaki
        public static void WaitForEndConnecting()
        {
            while (Server.ClientConnecting) {
                Thread.Sleep(500);
            }
        }

        //várja meg amíg küld valaki adatokat és csak utána menjen tovább
        public static void WaitForEndDataSending(Client client)
        {
            while (client.SendingDatas) {
                Thread.Sleep(100);
            }
        }
    }
}
