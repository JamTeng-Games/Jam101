using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgEnterGame : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_EnterGame;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
        }
    }

}