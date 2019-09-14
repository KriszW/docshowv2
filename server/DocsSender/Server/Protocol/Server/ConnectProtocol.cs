using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class ConnectProtocol
    {
        public static void MakeConnect(TcpClient client)
        {
            //a connecting truera állítása
            Server.ClientConnecting = true;

            string ip = "";

            try
            {
                //a stream megszerzése
                NetworkStream clientStream = client.GetStream();

                //az IP megszerzése
                ip = ConnectMethods.GetIPadd(clientStream, client);

                //A maximum cikkek számának az elküldése
                ConnectMethods.SendMaxCikkCount(clientStream, client);

                //megnézni hogy létezik-e már ilyen kliens
                //ha létezik akkor elküldeni az
                //ipinUse parancsot
                if (ClientOperations.ValidClient(ip))
                { 
                    ConnectMethods.SendRawMSG(clientStream, "/inUse");
                    return;
                }

                Client newClient = null;

                //send ok datas
                ConnectMethods.SendOkDatas(clientStream,client);

                //ha nincs akkor hozzáadás
                newClient = new Client(client, ip);

                CheckClientValidates.IsTooManyClient(newClient);

                Server.Clients.Add(newClient);

                //az asztalszámok megszerzése
                List<string> asztalSzamok = GetAsztalszamMethods.GetAllAsztalszam(ip);

                //az asztalszámok validitását vizsgálja meg
                GetAsztalszamMethods.CheckIsValidAsztalSzam(asztalSzamok, newClient);

                //a Kilőkőre beállítja a validKlienseket
                SetKilokoClients.SetKilokoClient(asztalSzamok,newClient);

                //az aktuális doksik elküldése
                SendingServerMethods.SendFirstDocs(asztalSzamok);

                Console.WriteLine($"{DateTime.Now.ToString()}:A {newClient.ClientIP} kliens sikeresen felcsatlakozott");
            }
            catch (ServerClientException ex)
            {
                DoDisconnect(ex.Message,ip);
            }
            catch(ApplicationException ex)
            {
                DoDisconnect(ex.Message,ip);
            }
            catch (Exception ex)
            {
                string msg = $"Hiba lépett fel a(z) {ip} kliens csatlakozása közben: {ex.Message}";

                DoDisconnect(msg,ip);
            }
            finally
            {
                //a connectelés leállítása
                Server.ClientConnecting = false;
            }
        }

        static void DoDisconnect(string msg,string ip)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}:{msg}");
            IOs.Logger.MakeLog(msg);

            Client newClient = ClientMethods.GetClient(ip);

            if (newClient != null)
            {
                newClient.Disconnect();
            }
        }
    }
}
