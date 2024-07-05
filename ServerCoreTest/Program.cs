using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCoreTest
{
    internal class Program
    {
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                Session session = new Session();
                session.Start(clientSocket);

                byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to MMORPG Server");
                session.Send(sendBuffer);

                Thread.Sleep(1000);

                session.Disconnect();

                //byte[] recvBuffer = new byte[1024];
                //int recvBytes = clientSocket.Receive(recvBuffer);
                //string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvBytes);
                //Console.WriteLine($"[From Cient] {recvData}");

                //byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome Client!!!!");
                //clientSocket.Send(sendBuffer);

                //clientSocket.Shutdown(SocketShutdown.Both);
                //clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static void Main(string[] args)
        {

            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Listener listener = new Listener();
            listener.Init(endPoint, OnAcceptHandler);
            while (true)
            {

            }
            //Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //try
            //{
            //    listenSocket.Bind(endPoint);

            //    listenSocket.Listen(10);

            //    while (true)
            //    {
            //        Console.WriteLine("Listening.....");
            //        Socket clientSocket = listenSocket.Accept();

            //        byte[] recvBuffer = new byte[1024];
            //        int recvBytes = clientSocket.Receive(recvBuffer);
            //        string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvBytes);
            //        Console.WriteLine($"[From Cient] {recvData}");

            //        byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome Client!!!!");
            //        clientSocket.Send(sendBuffer);

            //        clientSocket.Shutdown(SocketShutdown.Both);
            //        clientSocket.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

        }
    }
}