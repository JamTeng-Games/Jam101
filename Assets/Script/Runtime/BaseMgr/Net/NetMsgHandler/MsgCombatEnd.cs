using Jam.Core;
using Jam.Runtime.Quantum_;

namespace Jam.Runtime.Net_
{

    public class MsgCombatEnd : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_CombatEnd;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
            G.Instance.QuantumChannel.DisconnectAsync(QuantumConnectFailReason.CombatTimeUp);
        }
    }

}