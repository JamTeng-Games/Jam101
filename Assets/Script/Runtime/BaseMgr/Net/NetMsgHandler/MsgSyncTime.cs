using Jam.Core;
using Jam.Runtime.Data_;

namespace Jam.Runtime.Net_
{

    public class MsgSyncTime : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_SyncTime;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            SyncTimeData time = packet.Decode<SyncTimeData>();
            // JLog.Debug($"Recv {cmd} {time.time}");
            G.Data.SetTime(time.time);
        }
    }

}