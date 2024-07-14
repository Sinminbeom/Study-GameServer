using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
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
    class ServerSession : PacketSession
    {
        static unsafe void ToBytes(byte[] array, int offset, ulong value)
        {
            fixed (byte* ptr = &array[offset])
                *(ulong*)ptr = value;
        }
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { size = 4, packetId = (ushort)PacketID.PlayerInfoReq, playerId = 100 };

            for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> segment = SendBufferHelper.Open(4096);
                byte[] size = BitConverter.GetBytes(packet.size);
                byte[] packetId = BitConverter.GetBytes(packet.packetId);
                byte[] playerId = BitConverter.GetBytes(packet.playerId);

                ushort count = 0;
                bool success = true;



                //success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset, segment.Count), packet.size);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), packet.packetId);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), packet.playerId);
                count += 8;

                success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset, segment.Count), count);

                ArraySegment<byte> sendBuffer = SendBufferHelper.Close(count);

                if (success)
                    Send(sendBuffer);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected {endPoint}");
        }

        //public override int OnRecv(ArraySegment<byte> buffer)
        //{
        //    string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
        //    Console.WriteLine($"[From Server] {recvData}");
        //    return buffer.Count;
        //}

        public override int OnRecvPacket(ArraySegment<byte> buffer)
        {
            int dataSize = BitConverter.ToUInt16(buffer.Array, 0);
            int packetId = BitConverter.ToUInt16(buffer.Array, 2);
            Console.WriteLine($"size = {dataSize}, packetId = {packetId}");
            return dataSize;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
