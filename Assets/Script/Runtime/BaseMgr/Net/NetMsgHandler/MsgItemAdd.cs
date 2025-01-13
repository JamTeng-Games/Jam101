using Jam.Core;
using Jam.Runtime.Data_;
using Jam.Runtime.Event;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgItemAdd : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_ItemAdd;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            BagItem m = packet.Decode<BagItem>();
            JLog.Debug($"Recv {cmd}, {m}");
            ItemBagHelper.OnAddItem(m.id, m.amount);
        }
    }

}