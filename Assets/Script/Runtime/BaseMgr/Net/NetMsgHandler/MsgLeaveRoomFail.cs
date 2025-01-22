using Jam.Runtime.Event;

namespace Jam.Runtime.Net_
{

    public class MsgLeaveRoomFail : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_LeaveRoomFail;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            G.Event.Send(GlobalEventId.LeaveRoomFail);
        }
    }

}