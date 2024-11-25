using System;
using Jam.Core;

namespace Jam.Runtime.Event
{

    public class EventMgr : IMgr, ITickable
    {
        private EventCenter<GlobalEventId> s_eventCenter;

        public EventMgr()
        {
            s_eventCenter = EventCenter<GlobalEventId>.Create<GlobalEventId>();
        }

        public void Shutdown(bool isAppQuit)
        {
            s_eventCenter.Clear();
            s_eventCenter = null;
        }

        public void Tick(float deltaTime)
        {
            s_eventCenter.Tick();
        }

        public void Subscribe(GlobalEventId name, Action action)
        {
            s_eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T>(GlobalEventId name, Action<T> action)
        {
            s_eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2>(GlobalEventId name, Action<T1, T2> action)
        {
            s_eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2, T3>(GlobalEventId name, Action<T1, T2, T3> action)
        {
            s_eventCenter.Subscribe(name, action);
        }

        public void SubscribeObj(GlobalEventId name, Action<object> action)
        {
            s_eventCenter.SubscribeObj(name, action);
        }

        public void Unsubscribe(GlobalEventId name, Action action)
        {
            s_eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T>(GlobalEventId name, Action<T> action)
        {
            s_eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2>(GlobalEventId name, Action<T1, T2> action)
        {
            s_eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2, T3>(GlobalEventId name, Action<T1, T2, T3> action)
        {
            s_eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe(GlobalEventId name, Action<object> action)
        {
            s_eventCenter.UnsubscribeObj(name, action);
        }

        public void Send(GlobalEventId name)
        {
            s_eventCenter.Send(name);
        }

        public void Send<T>(GlobalEventId name, T info)
        {
            s_eventCenter.Send(name, info);
        }

        public void Send<T1, T2>(GlobalEventId name, T1 info1, T2 info2)
        {
            s_eventCenter.Send(name, info1, info2);
        }

        public void Send<T1, T2, T3>(GlobalEventId name, T1 info1, T2 info2, T3 info3)
        {
            s_eventCenter.Send(name, info1, info2, info3);
        }

        public void SendObj(GlobalEventId name, object arg)
        {
            s_eventCenter.SendObj(name, arg);
        }
    }

}