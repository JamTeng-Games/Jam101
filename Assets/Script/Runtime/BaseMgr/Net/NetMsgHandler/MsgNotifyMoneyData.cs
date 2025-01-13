using System.Collections.Generic;
using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgNotifyMoneyData : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_NotifyMoneyData;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
            MoneyBagData m = packet.Decode<MoneyBagData>();
            foreach (var money in m.money_bag)
            {
                JLog.Debug($"Money: {money.id} {money.amount}");
            }
            MoneyBagHelper.OnUpdateMoneys(m);
        }
    }

}