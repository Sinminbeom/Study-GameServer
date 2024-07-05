using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoreTest
{
    public class Listener
    {
        Socket _listenSocket;
        public void Init()
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(10);

            SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

            RegisterAccept(_recvArgs);
        }

        public void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        public void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Socket clientSocket = args.AcceptSocket as Socket;

                if (clientSocket == null)
                    return;

                // 받아들이는걸 성공했으니 받아야지 Recv
                byte[] _recvBuffer = new byte[1024];
                int recvBytes = clientSocket.Receive(_recvBuffer);
                string recvData = Encoding.UTF8.GetString(_recvBuffer);
                Console.WriteLine($"[From Client] {recvData}");

                byte[] _sendBuffer = Encoding.UTF8.GetBytes("Welcome Client!!!");
                clientSocket.Send(_sendBuffer);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            else
            {
                Console.WriteLine(SocketError.SocketError.ToString());
            }

            RegisterAccept(args);
        }
    }
}
