using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class MsgShopDestroy : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_ShopDestroy;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            JLog.Debug("MsgShopDestroy");
        }
    }

}