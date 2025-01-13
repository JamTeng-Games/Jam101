using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgNotifyRoundData : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_NotifyRoundData;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
            RoundData m = packet.Decode<RoundData>();
            G.Data.UserData.round = m.round;
            G.Event.Send(GlobalEventId.RoundUpdate);
        }
    }

}