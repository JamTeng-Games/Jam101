using System;
using Jam.Core;

namespace Jam.Runtime.Input_
{

    public class InputEvent
    {
        private EventCenter<InputEventId> _eventCenterCenter;

        public InputEvent()
        {
            _eventCenterCenter = EventCenter<InputEventId>.Create<InputEventId>();
        }

        public void Subscribe(InputEventId name, Action action)
        {
            _eventCenterCenter.Subscribe(name, action);
        }

        public void Subscribe<T>(InputEventId name, Action<T> action)
        {
            _eventCenterCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2>(InputEventId name, Action<T1, T2> action)
        {
            _eventCenterCenter.Subscribe(name, action);
        }

        public void Subscribe<T1, T2, T3>(InputEventId name, Action<T1, T2, T3> action)
        {
            _eventCenterCenter.Subscribe(name, action);
        }

        public void Unsubscribe(InputEventId name, Action action)
        {
            _eventCenterCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T>(InputEventId name, Action<T> action)
        {
            _eventCenterCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2>(InputEventId name, Action<T1, T2> action)
        {
            _eventCenterCenter.Unsubscribe(name, action);
        }

        public void Unsubscribe<T1, T2, T3>(InputEventId name, Action<T1, T2, T3> action)
        {
            _eventCenterCenter.Unsubscribe(name, action);
        }

        public void Send(InputEventId name)
        {
            _eventCenterCenter.Send(name);
        }

        public void Send<T>(InputEventId name, T info)
        {
            _eventCenterCenter.Send(name, info);
        }

        public void Send<T1, T2>(InputEventId name, T1 info1, T2 info2)
        {
            _eventCenterCenter.Send(name, info1, info2);
        }

        public void Send<T1, T2, T3>(InputEventId name, T1 info1, T2 info2, T3 info3)
        {
            _eventCenterCenter.Send(name, info1, info2, info3);
        }

        public void Clear()
        {
            _eventCenterCenter.Clear();
        }
    }

}