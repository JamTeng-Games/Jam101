using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgNotifyChooseHero : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_NotifyChooseHero;
        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            ChooseHeroRes res = packet.Decode<ChooseHeroRes>();
            G.Data.UserData.hero = res.hero;
            G.Event.Send(GlobalEventId.HeroChange, res.hero);
        }
    }

}