using Jam.Runtime.Helpers;

namespace Jam.Runtime.Net_
{

    public class MsgLeaveRoomSucc : IMsgHandler
    {
        public NetCmd Cmd => NetCmd.SC_LeaveRoomSucc;

        public void HandleMsg(NetCmd cmd, Packet packet)
        {
            RoomHelper.OnLeaveRoomSucc();
        }
    }

}