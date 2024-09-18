using System;

namespace J.Core
{

    public static class JTimer
    {
        private static long _now;
        private static float _deltaTime;
        private static float _fixedDeltaTime;
        private static float _time;
        private static int _frameCount;
        private static readonly DateTime _1970DateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static ThreadTimer _thread;
        private static MainTimer _global;
        private static MainTimer _globalLate;
        private static MainTimer _globalFix;
        private static MainTimer _scene;

        private static event Action _destroyAction;

        public static long Now => _now;
        public static float DeltaTime => _deltaTime;
        public static float FixedDeltaTime => _fixedDeltaTime;
        public static float Time => _time;
        public static int FrameCount => _frameCount;

        public static MainTimer Global
        {
            get
            {
                if (_global == null)
                {
                    _global = new MainTimer();
                }
                return _global;
            }
        }

        public static MainTimer GlobalLate
        {
            get
            {
                if (_globalLate == null)
                {
                    _globalLate = new MainTimer();
                }
                return _globalLate;
            }
        }

        public static MainTimer GlobalFix
        {
            get
            {
                if (_globalFix == null)
                {
                    _globalFix = new MainTimer();
                }
                return _globalFix;
            }
        }

        public static ThreadTimer Thread
        {
            get
            {
                if (_thread == null)
                {
                    _thread = new ThreadTimer();
                }
                return _thread;
            }
        }

        public static void Shutdown()
        {
            _destroyAction?.Invoke();
        }

        public static void Tick(float deltaTime)
        {
            _now = DateTime.Now.Ticks;
            _deltaTime = UnityEngine.Time.deltaTime;
            _time = UnityEngine.Time.time;
            _frameCount = UnityEngine.Time.frameCount;
            _fixedDeltaTime = UnityEngine.Time.fixedDeltaTime;

            Global.Tick();
        }

        public static void LateTick()
        {
            GlobalLate.Tick();
        }

        public static void FixedTick()
        {
            GlobalFix.Tick();
        }

        public static void AddOnShutdown(Action action)
        {
            _destroyAction += action;
        }

        // public static long CalcTimeSpan()
        // {
        //     return (long)(DateTime.UtcNow - _1970DateTime).TotalSeconds;
        // }
        //
        // public static long CalcTimeSpanMm()
        // {
        //     return (long)(DateTime.UtcNow - _1970DateTime).TotalMilliseconds;
        // }
        //
        // public static DateTime GetDateTimeByTimestamp(long timestamp)
        // {
        //     var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //     return epoch.AddSeconds(timestamp);
        // }
        //
        // public static DateTime GetLocalTimeByTimestamp(long timestamp)
        // {
        //     var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //     return epoch.AddSeconds(timestamp).ToLocalTime();
        // }
        //
        // public static DateTime GetLocalTimeByDateTime(DateTime time)
        // {
        //     return time.ToLocalTime();
        // }
        //
        // public static long GetTimeStampMilliSecond()
        // {
        //     return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        // }
        //
        // public static long GetTimeStampSecond()
        // {
        //     return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // }
    }

}