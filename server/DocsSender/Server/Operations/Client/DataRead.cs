using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace DocsShowServer
{
    public class DataRead
    {
        Client Client { get; set; }

        public DataRead(Client client)
        {
            Client = client;
        }

        //egy üzenet olvasása
        public string ReadMSG()
        {
            //a buffer amibe betölti
            byte[] buffer = new byte[Client.ClientTCP.ReceiveBufferSize];

            //ha csatlakozva van
            if (Client.ClientTCP.Connected)
            {
                //ha tud olvasni
                if (Client.ClientStream.CanRead)
                {
                    try
                    {
                        //olvassa ki az összes adatot
                        Client.ClientStream.Read(buffer, 0, buffer.Length);

                        //adja vissza a végéről eltávólítva az üres karaktereket
                        string msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0');

                        return msg;
                    }
                    catch (IOException ex) {
                        Console.WriteLine($"{DateTime.Now.ToString()}: A {Client.ClientIP.ToString()} bezárta a kapcsolatot");
                        Logger.MakeLog($"A {Client.ClientIP.ToString()} bezárta a kapcsolatot: {ex.Message}" );
                    }
                    catch (SocketException ex) {
                        if (ex.SocketErrorCode == SocketError.TimedOut) {
                            Console.WriteLine($"{DateTime.Now.ToString()}: A kliens({Client.ClientIP.ToString()}) nem volt elérhető a megadott időn belül");
                            Logger.MakeLog("A kliens nem volt elérhető a megadott időn belül:" + ex.Message);
                        }
                        else {
                            Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen hiba lépett fel a {Client.ClientIP.ToString() } kliensnél:{ex.Message}");
                            Logger.MakeLog($"Ismeretlen hiba lépett fel a {Client.ClientIP.ToString() } kliensnél:{ex.Message}\nHiba:{ex.SocketErrorCode}");
                        }
                    }
                    catch (ThreadAbortException)
                    {

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{DateTime.Now.ToString()}: Hiba lépett fel a kommunikáció közben a {Client.ClientIP.ToString()} kliensnél");
                        Logger.MakeLog($"A { Client.ClientIP.ToString() } ismeretlen hiba lépett fel a kommunikáció közben:{ex.Message}" );
                    }
                }
            }
            Client.Disconnect();
            return "";
        }
    }
}
