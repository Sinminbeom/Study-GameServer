using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class PacketManager
    {
        #region Singleton
        static PacketManager _instance;

        public static PacketManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PacketManager();
                return _instance;
            }
        }
        #endregion

        Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();

        public void Register()
        {
            _onRecv.Add((ushort)PacketID.PlayerInfoReq, MakePacket<PlayerInfoReq>);
        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
        {
            
        }

        public void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T: IPacket, new()
        {
            T pkt = new T();
            pkt.Read(buffer);
        }
    }
}
