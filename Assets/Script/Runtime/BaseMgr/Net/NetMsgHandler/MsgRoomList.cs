using Jam.Runtime.Data_;
using Jam.Runtime.Event;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgRoomList : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RoomList;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomListRes m = packet.Decode<RoomListRes>();
            G.Event.Send(GlobalEventId.RoomListUpdate, m);
        }
    }

}