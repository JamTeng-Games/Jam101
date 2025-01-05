using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgRegisterFail : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_RegisterFail;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RegisterFail m = packet.Decode<RegisterFail>();
            JLog.Debug($"Recv {cmd}");
            G.Login.ProcessRegisterFail(m);
        }
    }

}