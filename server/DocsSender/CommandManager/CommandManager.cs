using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    static class CommandManager
    {
        public static void SendCommand(ICommand command,Client client)
        {
            if (command is CommonCommand)
            {
                command.SendCommand(client);
            }
        }
    }
}
