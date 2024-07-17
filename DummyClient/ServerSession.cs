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
        public string name;
        public List<SkillInfo> skills = new List<SkillInfo>();

        public struct SkillInfo
        {
            public int id;
            public short level;
            public float duration;

            public bool Write(Span<byte> span, ref ushort count)
            {
                bool success = true;

                
                success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.id);
                count += sizeof(int);
                success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.level);
                count += sizeof(short);
                success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.duration);
                count += sizeof(float);

                return success;
            }
        }

        public PlayerInfoReq()
        {
            this.packetId = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> segment)
        {
            Span<byte> span = new Span<byte>(segment.Array, segment.Offset, segment.Count);

            ushort count = 0;
            //this.size = BitConverter.ToUInt16(segment.Array, segment.Offset);
            count += 2;
            //this.packetId = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += 2;
            //this.playerId = BitConverter.ToInt64(segment.Array, segment.Offset + count);
            //this.playerId = BitConverter.ToInt64(new ArraySegment<byte>(segment.Array, segment.Offset + count, segment.Count - count));
            //this.playerId = BitConverter.ToInt64(new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count - count));
            this.playerId = BitConverter.ToInt64(span.Slice(count, span.Length - count));
            count += 8;

            //ushort nameLen = BitConverter.ToUInt16(new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count - count));
            ushort nameLen = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
            count += 2;
            this.name = Encoding.Unicode.GetString(span.Slice(count, nameLen));
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            byte[] size = BitConverter.GetBytes(this.size);
            byte[] packetId = BitConverter.GetBytes(this.packetId);
            byte[] playerId = BitConverter.GetBytes(this.playerId);

            ushort count = 0;
            bool success = true;

            Span<byte> span = new Span<byte>(segment.Array, segment.Offset, segment.Count);

            count += 2;
            //success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), this.packetId);
            success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.packetId);
            count += 2;
            //success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, segment.Offset + count, segment.Count - count), this.playerId);
            success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), this.playerId);
            count += 8;

            //ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(this.name);
            ushort nameLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), nameLen);
            count += sizeof(ushort);
            count += nameLen;

            success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)skills.Count);
            count += sizeof(ushort);

            foreach (SkillInfo skill in skills)
            {
                skill.Write(span, ref count);
            }

            //success &= BitConverter.TryWriteBytes(span, count);
            success &= BitConverter.TryWriteBytes(span, count);


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

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 100, name = "subin" };

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
                        Console.WriteLine($"PlayerInfoReq {{ {packet.playerId}, {packet.name} }}");
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
