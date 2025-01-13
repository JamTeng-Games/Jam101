using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgShopCreate : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_ShopCreate;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            ShopData m = packet.Decode<ShopData>();
            JLog.Debug($"Recv {cmd} price: {m.refresh_money.price}");
            ShopHelper.Create(m);
        }
    }

}