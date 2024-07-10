using ServerCoreTest;
using System.Net;
using System.Text;

namespace DummyClientTest
{
    public class GameSession : Session
    {
        public override void OnConnect(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnect = {endPoint}");

            byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");
            Send(sendBuffer);

            Thread.Sleep(1000);

            Disconnect();
        }

        public override void OnDisconnect(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnect = {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Client] {recvData} || BytesTransferred = {buffer.Count}");

            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred Bytes = {numOfBytes}");
        }
    }

    internal class Program
    {
        static Listener listener = new Listener();
        private static void Main(string[] args)
        {

            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //Listener listener = new Listener();
            listener.Init(endPoint, () => { return new GameSession(); });
            while (true)
            {

            }

        }
    }
}