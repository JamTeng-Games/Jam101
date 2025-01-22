using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgRoomUserLeave : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RoomUserLeave;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomSeat m = packet.Decode<RoomSeat>();
            JLog.Info($"Recv {cmd}: {m.seat}");
            RoomHelper.UserLeave(m);
        }
    }

}