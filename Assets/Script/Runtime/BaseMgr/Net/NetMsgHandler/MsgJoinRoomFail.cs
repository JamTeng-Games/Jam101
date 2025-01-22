using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgJoinRoomFail : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_JoinRoomFail;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            G.Event.Send(GlobalEventId.JoinRoomFail);
        }
    }

}