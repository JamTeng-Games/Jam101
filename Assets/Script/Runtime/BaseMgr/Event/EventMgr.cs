using System;
using Jam.Core;

namespace Jam.Runtime.Event
{

    public class EventMgr : IMgr, ITickable
    {
        private EventCenter<GlobalEventId> eventCenter;

        public EventMgr()
        {
            eventCenter = EventCenter<GlobalEventId>.Create<GlobalEventId>();
        }

        public void Shutdown(bool isAppQuit)
        {
            eventCenter.Clear();
            eventCenter = null;
        }

        public void Tick(float deltaTime)
        {
            eventCenter.Tick();
        }

        public void Subscribe(GlobalEventId name, Action action)
        {
            eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T>(GlobalEventId name, Action<T> action)
        {
            eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2>(GlobalEventId name, Action<T1, T2> action)
        {
            eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2, T3>(GlobalEventId name, Action<T1, T2, T3> action)
        {
            eventCenter.Subscribe(name, action);
        }

        public void SubscribeObj(GlobalEventId name, Action<object> action)
        {
            eventCenter.SubscribeObj(name, action);
        }

        public void Unsubscribe(GlobalEventId name, Action action)
        {
            eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T>(GlobalEventId name, Action<T> action)
        {
            eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2>(GlobalEventId name, Action<T1, T2> action)
        {
            eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2, T3>(GlobalEventId name, Action<T1, T2, T3> action)
        {
            eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe(GlobalEventId name, Action<object> action)
        {
            eventCenter.UnsubscribeObj(name, action);
        }

        public void Send(GlobalEventId name)
        {
            eventCenter.Send(name);
        }

        public void Send<T>(GlobalEventId name, T info)
        {
            eventCenter.Send(name, info);
        }

        public void Send<T1, T2>(GlobalEventId name, T1 info1, T2 info2)
        {
            eventCenter.Send(name, info1, info2);
        }

        public void Send<T1, T2, T3>(GlobalEventId name, T1 info1, T2 info2, T3 info3)
        {
            eventCenter.Send(name, info1, info2, info3);
        }

        public void SendObj(GlobalEventId name, object arg)
        {
            eventCenter.SendObj(name, arg);
        }
    }

}