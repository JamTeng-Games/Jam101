using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgRoomUserChat : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RoomUserChat;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomUserChat m = packet.Decode<RoomUserChat>();
            RoomHelper.UserChat(m);
        }
    }

}