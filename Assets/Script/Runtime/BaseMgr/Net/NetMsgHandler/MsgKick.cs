using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgKick : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_Kick;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
        }
    }

}