using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 해당 패킷이 다 조립이 되었으면 무엇을 호출할지를 정의한다.
class PacketHandler
{
        
    public static void S_ChatHandler(PacketSession session, IPacket packet)
    {
        S_Chat chatPacket = packet as S_Chat;
        ServerSession serverSession = session as ServerSession;
        //Console.WriteLine(chatPacket.chat);
        //if (chatPacket.playerId == 1)
        //    Console.WriteLine(chatPacket.chat);

    }
}