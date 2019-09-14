using CommonDataModel;
using IOs;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using static Client.DocsShowClient;

namespace Client
{
    class NativeDocsShowClientMethods
    {
        public static void ReadMethod()
        {
            StandardsForGoodPosition = new string[Datas.CountOfMonitors, Datas.MaxCikkCount];

            try
            {
                while (true)
                {
                    GetDocsProtocol.GetDocs();
                }
            }
            catch (IOException ex)
            {
                string msg = $"A kapcsolatot bezárta a {ServerIP} szerver: {ex.Message}";
                Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
                Logger.MakeLog(msg);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AccessDenied)
            {
                string msg = $"A {ServerIP} szerver letiltotta az adatkommunikációt";
                Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
                Logger.MakeLog(msg + ":" + ex.Message);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
            {
                string msg = $"A {ServerIP} szerver nem volt elérhető a megadott időn belül";

                Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
                Logger.MakeLog(msg + ": " + ex.Message);
            }
            catch (SocketException ex)
            {
                string msg = $"Ismeretlen hiba lépett fel a { ServerIP} szerveren";

                Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
                Logger.MakeLog(msg + ": " + ex.Message + "\nA sockethiba kódja: " + ex.SocketErrorCode);
            }
            catch (Exception ex)
            {
                string msg = $"Ismeretlen hiba lépett fel a { ServerIP} szerveren a kommunikáció közben";

                Console.WriteLine($"{ DateTime.Now.ToString()}:{ msg}");
                Logger.MakeLog(msg + ": " + ex.Message);
            }
            Thread.Sleep(1000);

            InPDF = false;
        }

        public static void Connect()
        {
            try
            {
                //a connect megindítása
                ConnectOperations.MakeConnect();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"{ DateTime.Now.ToString()}:{ ex.Message}");
                Logger.MakeLog(ex.Message);
            }
            catch (SocketException ex)
            {
                //ha a cím már használatban van exception jön
                if (SocketError.AddressAlreadyInUse == ex.SocketErrorCode)
                {
                    string errMSG = "Az IP cím már használatban van a " + ClientTCP.Client.RemoteEndPoint.ToString() + " szerveren";

                    Console.WriteLine($"{ DateTime.Now.ToString()}:{ errMSG}");
                    Logger.MakeLog(errMSG + ":" + ex.Message);

                    Console.WriteLine("Nyomj egy entert a kilépéshez");
                    Console.ReadLine();
                    Environment.Exit(1); //1-es kód ahhoz, hogy az IP cím már használatban van
                }
                //ha a csatlkozás meghiúsult
                else if (SocketError.ConnectionRefused == ex.SocketErrorCode)
                {
                    string errMSG = "A kapcsolatot visszautasította a " + ClientTCP.Client.RemoteEndPoint.ToString() + " szerver";

                    Console.WriteLine($"{ DateTime.Now.ToString()}:{ errMSG}");
                    Logger.MakeLog(errMSG + ":" + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ DateTime.Now.ToString()}:A kapcsolódás 5 próba utána is sikertelen volt");
                Logger.MakeLog("A kapcsolódás 5 próba utána is sikertelen volt: " + ex.Message);
            }
        }
    }
}
