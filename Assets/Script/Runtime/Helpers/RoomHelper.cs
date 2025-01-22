using Jam.Runtime.Data_;
using Jam.Runtime.Event;

namespace Jam.Runtime.Helpers
{

    public static class RoomHelper
    {
        public const int SeatMaxCount = 8;

        public static bool IsInRoom()
        {
            return G.Data.RoomData.id != 0;
        }

        public static void OnJoinRoomSucc(RoomData room)
        {
            G.Data.RoomData.id = room.id;
            G.Data.RoomData.name = room.name;
            G.Data.RoomData.users = room.users;
            G.Data.RoomData.seat = room.seat;
            G.Event.Send(GlobalEventId.JoinRoomSuccess);
        }

        public static void OnLeaveRoomSucc()
        {
            G.Data.RoomData.id = 0;
            G.Data.RoomData.name = "";
            G.Data.RoomData.users = null;
            G.Data.RoomData.seat = 0;
            G.Event.Send(GlobalEventId.LeaveRoomSuccess);
        }

        public static void UpdateUserInfos(RoomUserInfos roomUserInfos)
        {
            G.Data.RoomData.users = roomUserInfos.users;
            G.Event.Send(GlobalEventId.RoomUserInfosUpdate);
        }

        public static void UserEnter(RoomUser roomUser)
        {
            G.Data.RoomData.users.Add(roomUser);
            G.Event.Send(GlobalEventId.RoomUserEnter, roomUser);
        }

        public static void UserLeave(RoomSeat roomSeat)
        {
            G.Data.RoomData.users.RemoveAll(u => u.seat == roomSeat.seat);
            G.Event.Send(GlobalEventId.RoomUserLeave, roomSeat);
        }

        public static void UserUpdateReady(RoomSeatReady roomSeatReady)
        {
            var user = G.Data.RoomData.users.Find(u => u.seat == roomSeatReady.seat);
            if (user != null)
            {
                user.ready = roomSeatReady.ready;
                G.Event.Send(GlobalEventId.RoomUserReadyUpdate, roomSeatReady);
            }
        }

        public static void UserChat(RoomUserChat roomUserChat)
        {
            G.Event.Send(GlobalEventId.RoomUserChat, roomUserChat);
        }

        public static bool IsSelfReady()
        {
            var self = G.Data.RoomData.users.Find(u => u.seat == G.Data.RoomData.seat);
            return self != null && self.ready;
        }

        public static int ReadyCount()
        {
            return G.Data.RoomData.users.FindAll(u => u.ready).Count;
        }

        public static RoomUser GetRoomUserBySeat(int seat)
        {
            return G.Data.RoomData.users.Find(u => u.seat == seat);
        }

        public static bool AllSeatReady()
        {
            return G.Data.RoomData.users.TrueForAll(u => u.ready);
        }
    }

}