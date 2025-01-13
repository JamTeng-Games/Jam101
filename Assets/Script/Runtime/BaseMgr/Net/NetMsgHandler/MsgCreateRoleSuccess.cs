using Jam.Cfg;
using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgCreateRoleSuccess : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_CreateRoleSuccess;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoleData m = packet.Decode<RoleData>();
            JLog.Debug($"Recv {cmd}, {m}");
            G.Data.UserData.rid = m.rid;
            G.Data.UserData.name = m.name;
            G.Data.UserData.hero = m.hero;
            G.Event.Send(GlobalEventId.RoleDataUpdate);
            G.UI.Close(UIPanelId.ChangeName);
        }
    }

}