using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerInfoReq;

// 해당 패킷이 다 조립이 되었으면 무엇을 호출할지를 정의한다.
class PacketHandler
{

    public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        PlayerInfoReq p = packet as PlayerInfoReq;

        Console.WriteLine($"PlayerInfoReq {{ {p.playerId}, {p.name} }}");

        foreach (Skill skill in p.skills)
        {
            Console.WriteLine($"Skill ({skill.id})({skill.level})({skill.duration})");
        }
    }

    public static void TestHandler(PacketSession session, IPacket packet)
    {
    }
}