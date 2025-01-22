using Jam.Cfg;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgJoinRoomSucc : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_JoinRoomSucc;
        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomData m = packet.Decode<RoomData>();
            RoomHelper.OnJoinRoomSucc(m);
            G.UI.Open(UIPanelId.Room);
        }
    }

}