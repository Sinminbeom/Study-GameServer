using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static C_PlayerInfoReq;

// 해당 패킷이 다 조립이 되었으면 무엇을 호출할지를 정의한다.
class PacketHandler
{

    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        C_Chat chatPacket = packet as C_Chat;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        clientSession.Room.Push(
            () => clientSession.Room.Broadcast(clientSession, chatPacket.chat)
        );
        //clientSession.Room.Broadcast(clientSession, chatPacket.chat);


        //C_PlayerInfoReq p = packet as C_PlayerInfoReq;

        //Console.WriteLine($"PlayerInfoReq {{ {p.playerId}, {p.name} }}");

        //foreach (Skill skill in p.skills)
        //{
        //    Console.WriteLine($"Skill ({skill.id})({skill.level})({skill.duration})");
        //}
    }
}