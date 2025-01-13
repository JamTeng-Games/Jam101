using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgShopUpdateGoods : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_ShopUpdateGoods;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
            ShopGoodsUpdate m = packet.Decode<ShopGoodsUpdate>();
            ShopHelper.UpdateGoods(m);
        }
    }

}