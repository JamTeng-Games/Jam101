using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgCreateRoleFailed : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_CreateRoleFailed;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            //
            JLog.Debug($"Recv {cmd}");
        }
    }

}