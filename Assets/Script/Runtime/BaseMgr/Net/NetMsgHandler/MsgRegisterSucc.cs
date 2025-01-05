using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgRegisterSucc : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RegisterSucc;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RegisterSuccess m = packet.Decode<RegisterSuccess>();
            JLog.Debug($"Recv {cmd}");
            G.Login.ProcessRegisterSuccess(m);
        }
    }

}