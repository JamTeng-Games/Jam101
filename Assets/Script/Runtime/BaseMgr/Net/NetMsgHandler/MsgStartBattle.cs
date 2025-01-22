using Cysharp.Threading.Tasks;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgStartBattle : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_StartBattle;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            BattleStart m = packet.Decode<BattleStart>();
            G.Event.Send(GlobalEventId.EnterCombat);
        }
    }

}