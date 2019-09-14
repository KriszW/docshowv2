using System.Net;
using System.Net.Sockets;

namespace DocsShowServer
{
    public interface IClient
    {
        IPAddress ClientIP { get; set; }
        NetworkStream ClientStream { get; }
        TcpClient ClientTCP { get; set; }
        int MaxErrNum { get; }
        DataRead Reader { get; }
        DataSend Sender { get; }
        bool SendingDatas { get; set; }

        void Disconnect();
    }
}