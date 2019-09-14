using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    static class GetAsztalszamMethods
    {
        public static void CheckIsValidAsztalSzam(List<string> asztalSzamok, Client newClient)
        {
            //ha üres akkor ne küldje el
            if (asztalSzamok.Count==0)
            {
                string msg = $"A {newClient.ClientIP.ToString()} kliens le lett csatlakoztatva: Nincs egy asztal sem kapcsolva a {CommonDatas.GepInfoPath} fájlban a klienshez";

                newClient.Sender.SendMSG($"/noKilokoSettedForYou \"Nincs a te IPdhez {newClient.ClientIP.ToString()} asztal kapcsolva a {CommonDatas.GepInfoPath} fájlban\"");
                throw new ServerClientException(msg);
            }
        }

        public static List<string> GetAllAsztalszam(string ip)
        {
            //az asztalok lekérdezése
            List<string> asztalSzamok = GetMachineInfosOperations.GetAsztalokFromIP(ip);

            if (asztalSzamok == null)
            {
                return new List<string>();
            }

            return asztalSzamok;
        }

    }
}
