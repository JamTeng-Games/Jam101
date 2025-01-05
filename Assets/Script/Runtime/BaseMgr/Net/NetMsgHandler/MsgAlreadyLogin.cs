using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgAlreadyLogin : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_AlreadyLogin;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            LoginSuccess m = packet.Decode<LoginSuccess>();
            JLog.Debug($"Recv {cmd}, {m}");
            G.Login.ProcessLoginSuccess(m);
        }
    }

}