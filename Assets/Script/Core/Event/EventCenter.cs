using System;
using System.Collections.Generic;

namespace Jam.Core
{

    internal interface IEventCallback
    {
        public int TypeId { get; }
        public Delegate CallbackHandler { get; }
    }

    public class EventCallback : IEventCallback
    {
        public int TypeId => 0;
        public Delegate CallbackHandler => callback;
        public Action callback;
    }

    public class EventCallbackObject : IEventCallback
    {
        public int TypeId => -1;
        public object arg;
        public Delegate CallbackHandler => callback;
        public Action<object> callback;
    }

    public class EventCallback<T> : IEventCallback
    {
        public int TypeId => 1;
        public Delegate CallbackHandler => callback;
        public Action<T> callback;
    }

    public class EventCallback<T1, T2> : IEventCallback
    {
        public int TypeId => 2;
        public Delegate CallbackHandler => callback;
        public Action<T1, T2> callback;
    }

    public class EventCallback<T1, T2, T3> : IEventCallback
    {
        public int TypeId => 3;
        public Delegate CallbackHandler => callback;
        public Action<T1, T2, T3> callback;
    }

    // Event type
    public class EventCenter<TEventType> where TEventType : Enum
    {
        private struct EventCommand
        {
            public bool isSubscribe;
            public TEventType name;
            public IEventCallback @event;
        }

        private readonly Dictionary<TEventType, List<IEventCallback>> _transDic0;
        private readonly Dictionary<TEventType, List<IEventCallback>> _transDic1;
        private readonly Dictionary<TEventType, List<IEventCallback>> _transDic2;
        private readonly Dictionary<TEventType, List<IEventCallback>> _transDic3;
        private readonly Dictionary<TEventType, List<IEventCallback>> _transDicObj;
        private readonly List<EventCommand> _commands;
        private readonly Queue<EventCallbackObject> _eventObjs;

        private int _lockCount = 0;
        private bool IsLocked => _lockCount != 0;

        private EventCenter<TEventType> _parent;
        private bool IsAttached => _parent != null;

        public static EventCenter<T> Create<T>() where T : Enum
        {
            EventCenter<T> t = new EventCenter<T>();
            return t;
        }

        private EventCenter()
        {
            _transDic0 = new Dictionary<TEventType, List<IEventCallback>>(32);
            _transDic1 = new Dictionary<TEventType, List<IEventCallback>>(32);
            _transDic2 = new Dictionary<TEventType, List<IEventCallback>>(32);
            _transDic3 = new Dictionary<TEventType, List<IEventCallback>>(32);
            _transDicObj = new Dictionary<TEventType, List<IEventCallback>>(32);
            _commands = new List<EventCommand>(8);
            _eventObjs = new Queue<EventCallbackObject>(8);
        }

        public void Dispose()
        {
            Clear();
        }

        // 挂接到另一个Signal，挂接后的事件会转发给起父Signal
        // 这样只要在根Signal上注册事件
        public void Attach(EventCenter<TEventType> parent)
        {
            _parent = parent;
        }

        // 解除挂接
        public void Detach()
        {
            _parent = null;
        }

        public void Subscribe(TEventType name, Action action)
        {
            EventCallback @event = new EventCallback() { callback = action };
            Subscribe(name, @event);
        }

        public void Subscribe<T>(TEventType name, Action<T> action)
        {
            EventCallback<T> @event = new EventCallback<T>() { callback = action };
            Subscribe(name, @event);
        }

        public void Subscribe<T1, T2>(TEventType name, Action<T1, T2> action)
        {
            EventCallback<T1, T2> @event = new EventCallback<T1, T2>() { callback = action };
            Subscribe(name, @event);
        }

        public void Subscribe<T1, T2, T3>(TEventType name, Action<T1, T2, T3> action)
        {
            EventCallback<T1, T2, T3> @event = new EventCallback<T1, T2, T3>() { callback = action };
            Subscribe(name, @event);
        }

        public void SubscribeObj(TEventType name, Action<object> action)
        {
            EventCallbackObject @event = new EventCallbackObject() { callback = action };
            Subscribe(name, @event);
        }

        public void Unsubscribe(TEventType name, Action action)
        {
            EventCallback @event = new EventCallback() { callback = action };
            Unsubscribe(name, @event);
        }

        public void Unsubscribe<T>(TEventType name, Action<T> action)
        {
            EventCallback<T> @event = new EventCallback<T>() { callback = action };
            Unsubscribe(name, @event);
        }

        public void Unsubscribe<T1, T2>(TEventType name, Action<T1, T2> action)
        {
            EventCallback<T1, T2> @event = new EventCallback<T1, T2>() { callback = action };
            Unsubscribe(name, @event);
        }

        public void Unsubscribe<T1, T2, T3>(TEventType name, Action<T1, T2, T3> action)
        {
            EventCallback<T1, T2, T3> @event = new EventCallback<T1, T2, T3>() { callback = action };
            Unsubscribe(name, @event);
        }

        public void UnsubscribeObj(TEventType name, Action<object> action)
        {
            EventCallbackObject @event = new EventCallbackObject() { callback = action };
            Unsubscribe(name, @event);
        }

        public void Send(TEventType name)
        {
            if (IsAttached)
            {
                _parent.Send(name);
            }

            if (_transDic0.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans0 = trans as EventCallback;
                    trans0.callback?.Invoke();
                }
                Unlock();
            }
        }

        public void Send<T>(TEventType name, T arg)
        {
            if (IsAttached)
            {
                _parent.Send(name, arg);
            }

            if (_transDic1.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans1 = trans as EventCallback<T>;
                    trans1.callback?.Invoke(arg);
                }
                Unlock();
            }
        }

        public void Send<T1, T2>(TEventType name, T1 arg1, T2 arg2)
        {
            if (IsAttached)
            {
                _parent.Send(name, arg1, arg2);
            }

            if (_transDic2.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans2 = trans as EventCallback<T1, T2>;
                    trans2.callback?.Invoke(arg1, arg2);
                }
                Unlock();
            }
        }

        public void Send<T1, T2, T3>(TEventType name, T1 arg1, T2 arg2, T3 arg3)
        {
            if (IsAttached)
            {
                _parent.Send(name, arg1, arg2, arg3);
            }

            if (_transDic3.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans3 = trans as EventCallback<T1, T2, T3>;
                    trans3.callback?.Invoke(arg1, arg2, arg3);
                }
                Unlock();
            }
        }

        public void SendObj(TEventType name, object arg)
        {
            if (IsAttached)
            {
                _parent.Send(name, arg);
            }

            if (_transDicObj.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var transObj = trans as EventCallbackObject;
                    transObj.arg = arg;
                    _eventObjs.Enqueue(transObj);
                }
                Unlock();
            }
        }

        public void Clear()
        {
            _transDic0.Clear();
            _transDic1.Clear();
            _transDic2.Clear();
            _transDic3.Clear();
            _transDicObj.Clear();
            _commands.Clear();
            _lockCount = 0;
        }

        public void Tick()
        {
            if (_eventObjs.Count == 0)
                return;

            while (_eventObjs.TryDequeue(out var eventObj))
            {
                eventObj.callback?.Invoke(eventObj.arg);
            }
        }

        #region Private Methods

        private void Subscribe(TEventType name, IEventCallback @event)
        {
            if (!GetSignalList(name, @event, out var transList, out var callbackLists))
            {
                callbackLists = new List<IEventCallback>(16);
                transList.Add(name, callbackLists);
            }
            if (IsLocked)
            {
                AddCommand(true, name, @event);
                return;
            }
            callbackLists!.Add(@event);
        }

        private void Unsubscribe(TEventType name, IEventCallback @event)
        {
            if (!GetSignalList(name, @event, out var transList, out var callbackLists))
                return;

            for (int i = callbackLists.Count - 1; i >= 0; i--)
            {
                if (Equals(callbackLists[i].CallbackHandler, @event.CallbackHandler))
                {
                    if (IsLocked)
                    {
                        AddCommand(false, name, @event);
                        return;
                    }
                    callbackLists.RemoveAt(i);
                    return;
                }
            }
        }

        private bool GetSignalList(TEventType name,
                                   IEventCallback @event,
                                   out Dictionary<TEventType, List<IEventCallback>> transList,
                                   out List<IEventCallback> callbackLists)
        {
            transList = default;
            if (@event.TypeId == 0)
            {
                transList = _transDic0;
            }
            if (@event.TypeId == 1)
            {
                transList = _transDic1;
            }
            if (@event.TypeId == 2)
            {
                transList = _transDic2;
            }
            if (@event.TypeId == 3)
            {
                transList = _transDic3;
            }
            if (@event.TypeId == -1)
            {
                transList = _transDicObj;
            }
            return transList!.TryGetValue(name, out callbackLists);
        }

        private void Lock()
        {
            _lockCount++;
        }

        private void Unlock()
        {
            _lockCount--;
            if (_lockCount == 0)
                ApplyCommands();
        }

        private void AddCommand(bool isSubscribe, TEventType name, IEventCallback @event)
        {
            _commands.Add(new EventCommand { isSubscribe = isSubscribe, name = name, @event = @event });
        }

        private void ApplyCommands()
        {
            if (_commands.Count == 0)
                return;
            foreach (var cmd in _commands)
            {
                ExecuteCommand(cmd);
            }
            _commands.Clear();
        }

        private void ExecuteCommand(in EventCommand cmd)
        {
            if (cmd.isSubscribe)
            {
                Subscribe(cmd.name, cmd.@event);
            }
            else
            {
                Unsubscribe(cmd.name, cmd.@event);
            }
        }

        #endregion Private Methods
    }

}