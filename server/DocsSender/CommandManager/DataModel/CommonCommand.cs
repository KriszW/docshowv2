using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class CommonCommand : ICommand
    {
        public CommonCommand(string cMD, string[] parameters)
        {
            CMD = cMD ?? throw new ArgumentNullException(nameof(cMD));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public string CMD { get; private set; }
        public string[] Parameters { get; private set; }

        public void SendCommand(Client client)
        {
            if (CMD != "")
            {
                SendCommandMethods.SendCommand(client,CMD,Parameters);
            }
        }
    }
}
