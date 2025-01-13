using Jam.Core;
using Jam.Runtime.Net_;

namespace Jam.Runtime.Data_
{

    public class DataMgr : IMgr
    {
        private UserData _userData;
        private long _serverTime;

        public UserData UserData => _userData;
        public long ServerTime => _serverTime;

        public void Init()
        {
            _userData = new UserData();
        }

        public void Shutdown(bool isAppQuit)
        {
        }

        public void SetTime(long time)
        {
            _serverTime = time;
        }
    }

}