using System.Collections.Generic;

namespace Jam.Runtime.Data_
{

    public class RoomData : IData
    {
        public int id;
        public string name;
        public List<RoomUser> users;
        public int seat;

        protected override void OnDispose()
        {
        }
    }

}