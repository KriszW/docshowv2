using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Pozicionalo
{
    class Client
    {
        const int MaxErrNum = 10;
        int errcounter;
        int UpdateRate;
        public bool ReceiveCMD { get; set; }

        public IPAddress ClientIP { get; private set; }
        TcpClient ClientTCP;
        private readonly NetworkStream ClientStream;

        private readonly DateTime ConnectTime;

        public Client(TcpClient client,NetworkStream ns,string IP,string updateRate)
        {
            errcounter = 0;
            ClientIP = IPAddress.Parse(IP);

            UpdateRate = int.Parse(updateRate);

            ClientTCP = client;

            ClientTCP.ReceiveTimeout = UpdateRate + 5000 ;
            ClientTCP.SendTimeout = UpdateRate + 5000;

            ClientTCP.ReceiveBufferSize = Datas.PacketSize;
            ClientTCP.SendBufferSize = Datas.PacketSize;
            ClientStream = ns;

            ConnectTime = DateTime.Now;

            ReceiveCMD = true;

            Thread checkerThread = new Thread(new ThreadStart(GetCMD))
            {
                Name = "checkerCMDfor" + ClientIP
            };
            checkerThread.Start();
        }

        public void GetCMD()
        {
            while (ReceiveCMD)
            {
               try
                {

                    byte[] buffer = new byte[ClientTCP.ReceiveBufferSize];

                    ClientStream.Read(buffer, 0, buffer.Length);

                    string msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0').Replace("/checkLife", "");

                    string answ = "";

                    if (msg =="")
                    {
                        answ = "/alive";

                        buffer = Encoding.UTF8.GetBytes(answ);
                        
                        ClientStream.Write(buffer, 0, buffer.Length);

                        Thread.Sleep(ClientTCP.ReceiveTimeout-4500);

                        continue;
                    }
                    

                    string cmd = ManageCMDs.GetCMD(msg);
                    msg = msg.Replace("/"+cmd+" ", "");

                    answ = ManageCMDs.WhichCMD(cmd, msg);

                    if (answ==null)
                    {
                        answ = "nem található parancs";
                    }

                    if (answ.StartsWith("###"))
                    {
                        answ = answ.Remove(0, 3);
                    }

                    buffer = Encoding.UTF8.GetBytes(answ);
                    ClientStream.Write(buffer, 0, buffer.Length);

                    Thread.Sleep(500);

                    continue;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("A " + ClientIP + " bezárta a kapcsolatot");
                    IOs.MakeLog("A " + ClientIP + " bezárta a kapcsolatot::" + ex.Message, 0);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode==SocketError.TimedOut)
                    {
                        if (errcounter<10)
                        {
                            errcounter++;
                            ClientTCP.ReceiveTimeout += errcounter * 1000;
                            ClientTCP.SendTimeout += errcounter * 1000;
                            GetCMD();
                        }
                        else
                        {
                            Console.WriteLine("A szerver nem válaszolt a megadott időn belűl!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hiba lépett fel a szerverrel való kommunikáció közben");
                        IOs.MakeLog("Hiba lépett fel a szerverrel való kommunikáció közben::"+ex.Message+":::"+ex.SocketErrorCode.ToString(),0);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hiba lépett fel a csomagfogádasa közben");
                    IOs.MakeLog("A " + ClientIP + " ismeretlen hiba lépett fel::" + ex.Message, 0);
                }
                ReceiveCMD = false;

            }
            Disconnect();
        }
        public void Disconnect()
        {
            ServerGetCMD.RemoveClient(this);
            ClientTCP.Close();

            Console.WriteLine(ClientIP + " lecsatlakozott ekkor "+DateTime.Now+" innen "+ClientIP);
            IOs.MakeLog(ClientIP + " lecsatlakozott innen " + ClientIP,0);
        }

    }
}
