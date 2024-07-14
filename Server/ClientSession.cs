using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    public class PlayerInfoReq : Packet
    {
        public long playerId;
    }
    public class PlayerInfoOk : Packet
    {
        public int hp;
        public int attack;
    }
    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2
    }
    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected {endPoint}");

            Packet packet = new Packet() { size = 4, packetId = 10 };

            ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            byte[] buffer = BitConverter.GetBytes(packet.size);
            byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            Array.Copy(buffer, 0, openSegment.Array, 0, buffer.Length);
            Array.Copy(buffer2, 0, openSegment.Array, buffer.Length, buffer2.Length);
            ArraySegment<byte> sendBuffer = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            Send(sendBuffer);

            Thread.Sleep(5000);

            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected {endPoint}");
        }

        public override int OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort count = 0;
            int dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            int packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch ((PacketID)packetId)
            {
                case PacketID.PlayerInfoReq:
                    {
                        long playerId = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);
                        count += 8;
                        Console.WriteLine($"PlayerInfoReq: {playerId}");
                    }
                    break;
                case PacketID.PlayerInfoOk:
                    {
                        int hp = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
                        count += 2;
                        int attack = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
                        count += 2;
                        Console.WriteLine($"PlayerInfoOk: {hp}, {attack}");
                    }
                    break;
            }

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
}
