using System;
using J.Core;

namespace J.Runtime.Input
{

    public class InputEvent
    {
        private JEvent<InputEventId> _eventCenter;

        public InputEvent()
        {
            _eventCenter = JEvent<InputEventId>.Create<InputEventId>();
        }

        public void Subscribe(InputEventId name, Action action)
        {
            _eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T>(InputEventId name, Action<T> action)
        {
            _eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2>(InputEventId name, Action<T1, T2> action)
        {
            _eventCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2, T3>(InputEventId name, Action<T1, T2, T3> action)
        {
            _eventCenter.Subscribe(name, action);
        }

        public void Unsubscribe(InputEventId name, Action action)
        {
            _eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T>(InputEventId name, Action<T> action)
        {
            _eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2>(InputEventId name, Action<T1, T2> action)
        {
            _eventCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2, T3>(InputEventId name, Action<T1, T2, T3> action)
        {
            _eventCenter.Unsubscribe(name, action);
        }

        public void Send(InputEventId name)
        {
            _eventCenter.Send(name);
        }

        public void Send<T>(InputEventId name, T info)
        {
            _eventCenter.Send(name, info);
        }

        public void Send<T1, T2>(InputEventId name, T1 info1, T2 info2)
        {
            _eventCenter.Send(name, info1, info2);
        }

        public void Send<T1, T2, T3>(InputEventId name, T1 info1, T2 info2, T3 info3)
        {
            _eventCenter.Send(name, info1, info2, info3);
        }

        public void Clear()
        {
            _eventCenter.Clear();
        }
    }

}