using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgShopRefresh : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_ShopRefresh;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
            ShopData m = packet.Decode<ShopData>();
            JLog.Debug($"Recv {cmd} price: {m.refresh_money.price}");
            ShopHelper.Refresh(m);
        }
    }

}