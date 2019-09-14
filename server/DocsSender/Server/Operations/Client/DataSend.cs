using IOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocsShowServer
{
    public class DataSend
    {
        Client Client { get; set; }

        public DataSend(Client client)
        {
            Client = client;
        }

        //msg küldése
        public void SendMSG(string text)
        {
            //az üzenet bájtjai
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            try
            {
                //a bájtok elküldése
                Client.ClientStream.Write(bytes, 0, bytes.Length);
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A {Client.ClientIP.ToString()} bezárta a kapcsolatot");
                Logger.MakeLog($"A {Client.ClientIP.ToString()} bezárta a kapcsolatot: {ex.Message}");
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}: A kliens({Client.ClientIP.ToString()}) nem volt elérhető a megadott időn belül");
                    Logger.MakeLog("A kliens nem volt elérhető a megadott időn belül:" + ex.Message);
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen hiba lépett fel a {Client.ClientIP.ToString()} kliensnél: {ex.Message}");
                    Logger.MakeLog($"Ismeretlen hiba lépett fel a {Client.ClientIP.ToString()} kliensnél: {ex.Message}\nHiba: {ex.SocketErrorCode}");
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen Hiba lépett fel a kommunikáció közben a {Client.ClientIP.ToString()} klienssel");
                Logger.MakeLog($"A {Client.ClientIP.ToString()} klinsnél ismeretlen hiba lépett fel a kommunikáció közben:" + ex.Message);
            }
            Client.Disconnect();
        }

        //bájtok küldése
        public void SendBytes(byte[] bytes, int offset, int length)
        {
            try {
                Client.ClientStream.Write(bytes, offset, length);
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A {Client.ClientIP.ToString()} bezárta a kapcsolatot");
                Logger.MakeLog($"A {Client.ClientIP.ToString()} bezárta a kapcsolatot: {ex.Message}");
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}: A kliens({Client.ClientIP.ToString()}) nem volt elérhető a megadott időn belül");
                    Logger.MakeLog("A kliens nem volt elérhető a megadott időn belül:" + ex.Message);
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen hiba lépett fel a {Client.ClientIP.ToString()} kliensnél: {ex.Message}");
                    Logger.MakeLog($"Ismeretlen hiba lépett fel a {Client.ClientIP.ToString()} kliensnél: {ex.Message}\nHiba: {ex.SocketErrorCode}");
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen Hiba lépett fel a kommunikáció közben a {Client.ClientIP.ToString()} klienssel");
                Logger.MakeLog($"A {Client.ClientIP.ToString()} klinsnél ismeretlen hiba lépett fel a kommunikáció közben:" + ex.Message);
            }
            Client.Disconnect();
        }

        //egy fájl elküldése
        public void SendAllData(byte[] data)
        {
            //a fájl hossza
            byte[] dataLength = BitConverter.GetBytes(data.Length);

            //a fájl hosszának elküldése
            SendBytes(dataLength, 0, 4);

            //várjon válaszra
            Client.Reader.ReadMSG();

            //mennyit bájtot küldött el
            int bytesSent = 0;
            //mennyi maradt
            int bytesLeft = data.Length;

            //addig csinálja amíg maradt bájt
            while(bytesLeft > 0)
            {
                //a maradék lekérése
                int curDataSize = Math.Min(Client.ClientTCP.ReceiveBufferSize, bytesLeft);

                //az adatok elküldése a curDataSizeról
                SendBytes(data, bytesSent, curDataSize);

                //az elküldötthöz hozzáadás
                bytesSent += curDataSize;
                //a maradból elvevés
                bytesLeft -= curDataSize;
            }
        }

        //egy fájl elküldése
        public void SendData(byte[] data)
        {
            try
            {
                SendAllData(data);
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:A {Client.ClientIP.ToString()} bezárta a kapcsolatot");
                Logger.MakeLog($"A {Client.ClientIP.ToString()} bezárta a kapcsolatot: {ex.Message}");
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}: A kliens({Client.ClientIP.ToString()}) nem volt elérhető a megadott időn belül");
                    Logger.MakeLog("A kliens nem volt elérhető a megadott időn belül:" + ex.Message);
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen hiba lépett fel a {Client.ClientIP.ToString()} kliensnél: {ex.Message}");
                    Logger.MakeLog($"Ismeretlen hiba lépett fel a {Client.ClientIP.ToString()} kliensnél: {ex.Message}\nHiba: {ex.SocketErrorCode}");
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()}:Ismeretlen Hiba lépett fel a kommunikáció közben a {Client.ClientIP.ToString()} klienssel");
                Logger.MakeLog($"A {Client.ClientIP.ToString()} klinsnél ismeretlen hiba lépett fel a kommunikáció közben:" + ex.Message);
            }
            Client.Disconnect();
        }
    }
}
