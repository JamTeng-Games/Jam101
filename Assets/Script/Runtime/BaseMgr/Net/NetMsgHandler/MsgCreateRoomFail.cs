using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgCreateRoomFail : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_CreateRoomFail;
        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            G.Event.Send(GlobalEventId.CreateRoomFail);
        }
    }

}