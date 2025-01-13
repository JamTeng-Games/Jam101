using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgMoneyAdd : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_MoneyAdd;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            BagMoney m = packet.Decode<BagMoney>();
            JLog.Debug($"Recv {cmd}, {m}");
            MoneyBagHelper.OnAddMoney(m.id, m.amount);
        }
    }

}