using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgRoomUserEnter : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RoomUserEnter;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomUser m = packet.Decode<RoomUser>();
            JLog.Info($"Recv SC_RoomUserEnter: {m.seat}, {m.name}");
            RoomHelper.UserEnter(m);
        }
    }

}