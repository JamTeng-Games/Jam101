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
            RoomHelper.UserUpdateReady(m);
        }
    }

}