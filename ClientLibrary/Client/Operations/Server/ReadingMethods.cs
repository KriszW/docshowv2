using CommandManagerLib;
using IOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class ReadingMethods
    {
        public static void StartRead()
        {
            //a pdfek várásának elindítása

            while(true)
            {
                //a szerverre való csatlakozás inditása
                DocsShowClient.Connect();

                //a pdfek olvasássa
                DocsShowClient.ReadMSG();
            }
        }

        //egy üzenet kiolvasása és utána válaszolja azt ami az üzenetben van
        public static string ReadMSG()
        {
            string readedMSG = "/non";

            byte[] buffer = new byte[DocsShowClient.ClientTCP.ReceiveBufferSize];

            DocsShowClient.ClientStream.Read(buffer, 0, buffer.Length);

            readedMSG = Encoding.UTF8.GetString(buffer).TrimEnd('\0');

            if (readedMSG=="")
            {
                throw new IOException("A kapcsolat megszakadt a szerverrel ismeretlen hiba által");
            }

            if (readedMSG.Contains("/check"))
            {
                readedMSG = readedMSG.Replace("/check","");

                SendingOperations.WriteMSG("/ok");
            }

            if (readedMSG.StartsWith("/"))
            {
                string[] datas = readedMSG.Split(' ');

                try
                {
                    CommandManager.Manager(datas[0], GetRealParams.GetRealParamsFromRawParams(datas));
                }
                catch (ApplicationException ex)
                {
                    DocsShowClient.Disconnect();

                    EndMethodsLibrary.EndMethods.End(ex.Message);
                }


                return readedMSG;
            }
            else
            {
                return readedMSG;
            }

        }
    }
}
