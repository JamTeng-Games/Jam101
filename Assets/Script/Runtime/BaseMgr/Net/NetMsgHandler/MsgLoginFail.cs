using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgLoginFail : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_LoginFail;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            LoginFail m = packet.Decode<LoginFail>();
            JLog.Debug($"Recv {cmd}");
            G.Login.ProcessLoginFail(m);
        }
    }

}