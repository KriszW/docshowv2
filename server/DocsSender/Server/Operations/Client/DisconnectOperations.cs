using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class DisconnectOperations
    {
        static void SetDefaultAllClientOnKilokok(string ip)
        {
            var kilokok = from kiloko in CommonDatas.Kilokok where kiloko.Client.ClientIP.ToString() == ip select kiloko;

            foreach (var item in kilokok)
            {
                item.DeleteClient();
            }
        }

        public static void SetKilokosOffline(string ip)
        {
            //az asztalok kiszedése
            List<string> Asztalok = GetMachineInfosOperations.GetAsztalokFromIP(ip);

            if (Asztalok!=null)
            {
                //az asztalokon végig menni és az összeset nullázni
                foreach (var asztal in Asztalok)
                {
                    try
                    {
                        CommonDatas.GUI.Setter.SetKilokoOffline(asztal);

                        IKilokoModel kiloko = DataOperations.GetKilokoModel(asztal);
                        kiloko.DeleteClient();
                    }
                    catch (Exception)
                    {

                    }
                }
            }

        }
    }
}
