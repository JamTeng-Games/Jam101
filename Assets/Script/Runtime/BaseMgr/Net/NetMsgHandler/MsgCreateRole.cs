using Jam.Cfg;
using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgCreateRole : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_CreateRole;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            // 创建角色
            // G.UI.Open();
            JLog.Debug($"Recv {cmd}");
            G.UI.Open(UIPanelId.ChangeName);
        }
    }

}