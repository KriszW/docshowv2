using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    static class SendCommandMethods
    {

        public static void SendCommand(Client client,string cmd,string[] parameters)
        {
            string msg = $"/{cmd} ";

            if (parameters.Length != 0)
            {
                foreach (var parameter in parameters)
                {
                    msg += $"{parameter} ";
                }
            }

            client.Sender.SendMSG(msg);
        }

    }
}
