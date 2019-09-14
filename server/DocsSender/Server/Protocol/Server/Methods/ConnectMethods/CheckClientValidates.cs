using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class CheckClientValidates
    {
        public static bool IsTooManyClient(Client newClient)
        {
            bool output = true;

            if ((Server.Clients.Count + 1) >= Server.MaxClientCount)
            {
                newClient.Sender.SendMSG("/tooManyClients \"A szerver elérte a maximum kliensek számát\"");
                throw new ServerClientException($"A kliens {newClient.ClientIP.ToString()} nem tudott felcsatlakozni, mert a szerver elérte a maximum kliensek számát");
            }
            else
            {
                output = false;
            }

            return output;
        }
    }
}
