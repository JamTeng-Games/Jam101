using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgRoomUserInfos : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RoomUserInfos;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomUserInfos m = packet.Decode<RoomUserInfos>();
            JLog.Info($"Recv {cmd}: {m.users.Count}");
            RoomHelper.UpdateUserInfos(m);
        }
    }

}