using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class ClientMethods
    {
        #region getclient

        //kliens kiszedése az IP alapján
        public static Client GetClient(string IP)
        {
            ParameterChecking.CheckParamValidate(IP);

            foreach (var item in Server.Clients)
                if (item.ClientIP.ToString() == IP)
                    return item;
            return null;
        }

        #endregion

        #region remove client

        //kliens eltávolítása az IP alapján
        public static void RemoveClient(string IP)
        {
            ParameterChecking.CheckParamValidate(IP);

            Client client = GetClient(IP);
            Server.Clients.Remove(client);
        }

        //kliens eltávolitása maga az objektum alapján
        public static void RemoveClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException("client", "A kliens az eltávolításra nem lehet null!");

            Server.Clients.Remove(client);
        }

        #endregion
    }
}
