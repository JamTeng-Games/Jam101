using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgNotifyRoleData : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_NotifyRoleData;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoleData m = packet.Decode<RoleData>();
            JLog.Debug($"Recv {cmd}, {m}");
            G.Data.UserData.name = m.name;
            G.Data.UserData.hero = m.hero;
            G.Event.Send(GlobalEventId.RoleDataUpdate);
        }
    }

}