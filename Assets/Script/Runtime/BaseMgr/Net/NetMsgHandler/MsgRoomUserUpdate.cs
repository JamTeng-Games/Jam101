using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgRoomUserUpdate : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RoomUserUpdate;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomSeatReady m = packet.Decode<RoomSeatReady>();
            JLog.Info($"Recv {cmd}: {m.seat} {m.ready}");
            RoomHelper.UserUpdateReady(m);
        }
    }

}