using System;
using J.Core;

namespace J.Runtime.Event
{

    public class EventMgr
    {
        private static JEvent<GlobalEventId> _event;

        public void Init()
        {
            _event = JEvent<GlobalEventId>.Create<GlobalEventId>();
        }

        public void Shutdown()
        {
            _event.Clear();
            _event = null;
        }

        public static void Subscribe(GlobalEventId name, Action action)
        {
            _event.Subscribe(name, action);
        }

        public static void Subscribe<T>(GlobalEventId name, Action<T> action)
        {
            _event.Subscribe(name, action);
        }

        public static void Subscribe<T1, T2>(GlobalEventId name, Action<T1, T2> action)
        {
            _event.Subscribe(name, action);
        }

        public static void Subscribe<T1, T2, T3>(GlobalEventId name, Action<T1, T2, T3> action)
        {
            _event.Subscribe(name, action);
        }

        public static void Unsubscribe(GlobalEventId name, Action action)
        {
            _event.Unsubscribe(name, action);
        }

        public static void Unsubscribe<T>(GlobalEventId name, Action<T> action)
        {
            _event.Unsubscribe(name, action);
        }

        public static void Unsubscribe<T1, T2>(GlobalEventId name, Action<T1, T2> action)
        {
            _event.Unsubscribe(name, action);
        }

        public static void Unsubscribe<T1, T2, T3>(GlobalEventId name, Action<T1, T2, T3> action)
        {
            _event.Unsubscribe(name, action);
        }

        public static void Send(GlobalEventId name)
        {
            _event.Send(name);
        }

        public static void Send<T>(GlobalEventId name, T info)
        {
            _event.Send(name, info);
        }

        public static void Send<T1, T2>(GlobalEventId name, T1 info1, T2 info2)
        {
            _event.Send(name, info1, info2);
        }

        public static void Send<T1, T2, T3>(GlobalEventId name, T1 info1, T2 info2, T3 info3)
        {
            _event.Send(name, info1, info2, info3);
        }
    }

}