using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    public abstract class Packet
    {
        public ushort size;
        public ushort packetId;

        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> segment);
    }

    public class PlayerInfoReq : Packet
    {
        public long playerId;

        public PlayerInfoReq()
        {
            this.packetId = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            //this.size = BitConverter.ToUInt16(segment.Array, segment.Offset);
            count += 2;
            //this.packetId = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += 2;
            this.playerId = BitConverter.ToInt64(segment.Array, segment.Offset + count);
            count += 8;
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            byte[] size = BitConverter.GetBytes(this.size);
            byte[] packetId = BitConverter.GetBytes(this.packetId);
            byte[] playerId = BitConverter.GetBytes(this.playerId);

            ushort count = 0;
            bool success = true;

            count += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), this.packetId);
            count += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), this.playerId);
            count += 8;

            success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset, segment.Count), (ushort)4);

            if (success == false)
                return null;

            ArraySegment<byte> sendBuffer = SendBufferHelper.Close(count);
            return sendBuffer;
        }
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

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 100 };

            for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> segment = packet.Write();

                if (segment != null)
                    Send(segment);
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
            ushort count = 0;
            int dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            int packetId = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch ((PacketID)packetId)
            {
                case PacketID.PlayerInfoReq:
                    {
                        PlayerInfoReq packet = new PlayerInfoReq();
                        packet.Read(buffer);
                        Console.WriteLine($"PlayerInfoReq: {packet.playerId}");
                    }
                    break;
            }

            return dataSize;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Send Transferred bytes: {numOfBytes}");
        }
    }
}
