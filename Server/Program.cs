using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using ServerCore;

namespace Server
{
    //public class Knight
    //{
    //    public int hp;
    //    public int attack;
    //    public string name;
    //    public List<int> skills = new List<int>();
    //}
    public class Packet
    {
        public ushort size;
        public ushort packetId;
    }
    class GameSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected {endPoint}");

            //Knight knight = new Knight() { hp = 100, attack = 10 };
            Packet packet = new Packet() { size = 4, packetId = 10 };

            //byte[] sendBuffer = new byte[4096];
            ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            byte[] buffer = BitConverter.GetBytes(packet.size);
            byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            Array.Copy(buffer, 0, openSegment.Array, 0, buffer.Length);
            Array.Copy(buffer2, 0, openSegment.Array, buffer.Length, buffer2.Length);
            ArraySegment<byte> sendBuffer = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            //int test1 = BitConverter.ToInt32(sendBuffer.Array, 0);
            //int test2 = BitConverter.ToInt32(sendBuffer.Array, 4);
            //Console.WriteLine($"!!!!!!!!!!!!! {test1} : {test2}");
            Send(sendBuffer);

            //byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
            //Send(sendBuff);

            Thread.Sleep(5000);

            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected {endPoint}");
        }

        public override int OnRecvPacket(ArraySegment<byte> buffer)
        {
            int dataSize = BitConverter.ToUInt16(buffer.Array, 0);
            int packetId = BitConverter.ToUInt16(buffer.Array, 2);
            Console.WriteLine($"size = {dataSize}, packetId = {packetId}");
            return dataSize;
        }

        //public override int OnRecv(ArraySegment<byte> buffer)
        //{
        //    string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
        //    Console.WriteLine($"[From Client] {recvData}");
        //    return buffer.Count;
        //}

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }

    class Program
    {
        static Listener _listener = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                GameSession session = new GameSession();
                session.Start(clientSocket);

                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
                session.Send(sendBuff);

                Thread.Sleep(1000);

                session.Disconnect();
                session.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 문지기
            _listener.Init(endPoint, () => { return new GameSession(); });
            Console.WriteLine("Listening...");

            while (true)
            {

            }
        }

        /*
        static Listener _listener = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                // 받는다
                byte[] recvBuff = new byte[1024];
                int recvBytes = clientSocket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"[From Client] {recvData}");

                // 보낸다
                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
                clientSocket.Send(sendBuff);

                // 쫓아낸다
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 문지기
            _listener.Init(endPoint, OnAcceptHandler);
            Console.WriteLine("Listening...");

            while (true)
            {

            }
        }
        */
    }
}