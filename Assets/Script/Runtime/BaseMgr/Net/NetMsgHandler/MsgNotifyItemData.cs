using System.Collections.Generic;
using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgNotifyItemData : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_NotifyItemData;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug($"Recv {cmd}");
            ItemBagData m = packet.Decode<ItemBagData>();
            ItemBagHelper.OnUpdateItems(m);
        }
    }

}