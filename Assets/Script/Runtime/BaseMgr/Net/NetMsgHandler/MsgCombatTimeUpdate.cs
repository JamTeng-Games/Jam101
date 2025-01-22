using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgCombatTimeUpdate : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_CombatTimeUpdate;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            CombatTimeUpdateData m = packet.Decode<CombatTimeUpdateData>();
            JLog.Debug($"Recv {cmd}, {m.time}");
            G.Event.Send<int>(GlobalEventId.CombatTimeUpdate, m.time);
        }
    }

}