using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class ConnectMethods
    {
        public static void SendRawMSG(NetworkStream clientStream,string msg)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);

                clientStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {

            }
        }

        public static string ReadRawMSG(NetworkStream clientStream, TcpClient client)
        {
            try
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];

                clientStream.Read(buffer, 0, buffer.Length);

                string msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0');

                return msg;
            }
            catch (Exception)
            {
                return "";
            }

        }

        //az IP megszerzése
        public static string GetIPadd(NetworkStream clientStream, TcpClient client)
        {
            //nyugta küldése
            SendRawMSG(clientStream,"/ok");

            //IP olvasása
            return ReadRawMSG(clientStream,client);
        }

        public static void SendOkDatas(NetworkStream clientStream, TcpClient client)
        {
            //a sikeres kapcsolodás nyugtázása a szervertől

            SendRawMSG(clientStream, "/okdatas");

            //válasz a sikeres kapcsolodásra

            string msg = ReadRawMSG(clientStream,client);

            if (msg != "/connected")
            {
                throw new ApplicationException($"A kliens visszautasította a sikeres kapcsolatot: {msg}");
            }
        }

        public static void SendMaxCikkCount(NetworkStream clientStream, TcpClient client)
        {
            string maxCikk = CommonDatas.Kilokok[0].MaxCikkCount.ToString();

            //a maxcikkszám elküldése
            SendRawMSG(clientStream,maxCikk);

            //nyugta olvasása
            string msg = ReadRawMSG(clientStream,client);

            if (msg != "/ok")
            {
                throw new ApplicationException($"A kliens visszautasította a maximum cikk szám értékét egy monitorra: {maxCikk}");
            }
        }
    }
}
