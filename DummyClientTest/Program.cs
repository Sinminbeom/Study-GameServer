using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClientTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(endPoint);
                Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");

                byte[] sendbuff = Encoding.UTF8.GetBytes("Hello World");
                socket.Send(sendbuff);

                byte[] recvBuff = new byte[1024];
                socket.Receive(recvBuff);

                string recvData = Encoding.UTF8.GetString(recvBuff);
                Console.WriteLine($"[From Server] {recvData}");

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            while(true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
