using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DocsShowServer;
using System.IO;
using IOs;
using System.Windows.Forms;

namespace DocsShowServer
{
    class Server
    {
        #region fields and props

        public static int MaxClientCount { get; private set; }

        public static int ServerPort { get; private set; }
        public static string PathToLogFile { get; set; }
        public static bool ClientConnecting { get; set; }

        public static IPAddress ServerIP { get; private set; }
        static TcpListener ServerListener;
        
        public static List<Client> Clients { get; private set; }

        #endregion

        #region constructors and decunstructors

        public static void InIt(int port)
        {
            bool res = int.TryParse(System.Configuration.ConfigurationManager.AppSettings["MaxClientCount"],out int maxCount);

            if (!res)
            {
                maxCount = 20;
            }

            MaxClientCount = maxCount;

            //új lista létrehozása
            Clients = new List<Client>();

            //log file beállítása
            PathToLogFile = "log.txt";

            //DocsShowServerip beállítása
            ServerIP= IPAddress.Parse(FileOperations.GetLocalIPAddress());

            try
            {
                //listener inicializálása
                ServerListener = new TcpListener(ServerIP,port);

                //listener indítása
                ServerListener.Start();
                //szerverport beállítása
                ServerPort = port;
                //a szerver induálásról informálás
                Console.WriteLine($"{DateTime.Now.ToString()}:A szerver elindult felcsatlakozni a: " + ServerIP+":"+ServerPort+" címen tudtok");

                //a listener thread indítása
                //ha a listener thread backgroundba van, akkor bezárodik a program, ha a GUIt leállítják
                Thread listenThread = new Thread(new ThreadStart(Listen))
                {
                    Name = "listen_doksiKiszolgalo",
                    IsBackground=true
                };
                listenThread.Start();
            }
            catch (Exception)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A szervert nem sikerült elindítani mert ezen a porton: {port} már fut valamilyen szerver!");
                MessageBox.Show($"A szervert nem sikerült elindítani mert ezen a porton: {port} már fut valamilyen szerver!","FATAL_ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.ReadLine();
                Environment.Exit(3);
            }
        }

        #endregion

        #region listen

        public static void Listen()
        {
            while (true)
            {
                //a tcp client megszerzése
                TcpClient client = ServerListener.AcceptTcpClient();

                try
                {
                    Task.Run(() => 
                    {
                        //a connectprotocol végrehajtása
                        ConnectProtocol.MakeConnect(client);
                    });

                }
                catch (IOException)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:A kapcsolatot a kliens bezárta: {client.Client.RemoteEndPoint.ToString()}");
                    Logger.MakeLog("A kapcsolatot a kliens bezárta: "+client.Client.RemoteEndPoint.ToString());
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        Console.WriteLine($"{DateTime.Now.ToString()}:A { client.Client.RemoteEndPoint.ToString() } kliens megpróbált mégegyszer felcsatlakozni a szerverre");
                        Logger.MakeLog($"A { client.Client.RemoteEndPoint.ToString() } kliens megpróbált mégegyszer felcsatlakozni a szerverre");
                    }
                    if (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        Console.WriteLine($"{DateTime.Now.ToString()}:A { client.Client.RemoteEndPoint.ToString() } kliens nem válaszolt a megadott időn belűl");
                        Logger.MakeLog($"A { client.Client.RemoteEndPoint.ToString() } kliens nem válaszolt a megadott időn belűl");
                    }
                    else
                    {
                        Logger.MakeLog($"A { client.Client.RemoteEndPoint.ToString() }  kliensnél ismeretlen hiba lépett fel  :  "+ex.Message);
                    }
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:{ex.Message}");
                    Logger.MakeLog(ex.Message, 0);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:A kliens érvénytelen IP címet adott meg: {ex.Message}:{ex.ParamName}");
                    Logger.MakeLog($"A kliens érvénytelen IP címet adott meg: {ex.Message}:{ex.ParamName}");
                }
                catch (ThreadStateException)
                {

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:A kliens bontotta a kapcsolatot, még mielőtt az létrejött volna a { client.Client.RemoteEndPoint.ToString() } címről hibakód: { ex.Message}");
                    Logger.MakeLog($"A kliens bontotta a kapcsolatot, még mielőtt az létrejött volna a { client.Client.RemoteEndPoint.ToString() } címről hibakód: { ex.Message}");
                }
                finally
                {
                    ClientConnecting = false;
                }
            }
        }

        #endregion
    }
}
