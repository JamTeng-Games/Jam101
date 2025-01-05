using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgLoginSucc : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_LoginSucc;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            LoginSuccess m = packet.Decode<LoginSuccess>();
            JLog.Debug($"Recv {cmd}, {m}");
            G.Login.ProcessLoginSuccess(m);
        }
    }

}