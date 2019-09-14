using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagerLib
{
    public static class CommandManager
    {
        public static void Manager(string cmd,string[] parameters)
        {
            switch (cmd)
            {
                case "/noKilokoSettedForYou":
                case "/tooManyClients":
                    throw new ApplicationException(parameters[0]);

                default:
                    break;
            }
        }
    }
}
