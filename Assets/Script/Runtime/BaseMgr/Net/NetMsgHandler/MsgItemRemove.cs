using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgItemRemove : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_ItemRemove;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            BagItem m = packet.Decode<BagItem>();
            JLog.Debug($"Recv {cmd}, {m}");
            ItemBagHelper.OnRemoveItem(m.id, m.amount);
        }
    }

}