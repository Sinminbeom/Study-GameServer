using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCoreTest
{
    internal class Session
    {
        Socket _socket;

        public void Start(Socket socket)
        {
            _socket = socket;

            SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _recvArgs.SetBuffer(new byte[1024], 0, 1024);


            RegisterRecv(_recvArgs);
        }

        public void Send(byte[] sendBuffer)
        {
            _socket.Send(sendBuffer);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void RegisterRecv(SocketAsyncEventArgs args)
        {
            bool pending = _socket.ReceiveAsync(args);
            if (pending == false)
            {
                OnRecvCompleted(null, args);
            }
        }

        public void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {

                string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred);
                Console.WriteLine($"[From Client] {recvData} || BytesTransferred = {args.BytesTransferred}");
                RegisterRecv(args);
            }
            else
            {
                //Console.WriteLine(args.SocketError.ToString());
            }
        }
    }
}
