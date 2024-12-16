using System;
using System.Collections.Generic;

namespace Jam.Core
{

    internal interface ISignalCallback
    {
        public int TypeId { get; }
        public Delegate CallbackHandler { get; }
    }

    public class SignalCallback : ISignalCallback
    {
        public int TypeId => 0;
        public Delegate CallbackHandler => callback;
        public Action callback;
    }

    public class SignalCallbackObject : ISignalCallback
    {
        public int TypeId => -1;
        public object arg;
        public Delegate CallbackHandler => callback;
        public Action<object> callback;
    }

    public class SignalCallback<T> : ISignalCallback
    {
        public int TypeId => 1;
        public Delegate CallbackHandler => callback;
        public Action<T> callback;
    }

    public class SignalCallback<T1, T2> : ISignalCallback
    {
        public int TypeId => 2;
        public Delegate CallbackHandler => callback;
        public Action<T1, T2> callback;
    }

    public class SignalCallback<T1, T2, T3> : ISignalCallback
    {
        public int TypeId => 3;
        public Delegate CallbackHandler => callback;
        public Action<T1, T2, T3> callback;
    }

    // Signal type
    public class Signal<TSignalType> where TSignalType : Enum
    {
        private struct SignalCommand
        {
            public bool isSubscribe;
            public TSignalType name;
            public ISignalCallback @signal;
        }

        private readonly Dictionary<TSignalType, List<ISignalCallback>> _transDic0;
        private readonly Dictionary<TSignalType, List<ISignalCallback>> _transDic1;
        private readonly Dictionary<TSignalType, List<ISignalCallback>> _transDic2;
        private readonly Dictionary<TSignalType, List<ISignalCallback>> _transDic3;
        private readonly Dictionary<TSignalType, List<ISignalCallback>> _transDicObj;
        private readonly List<SignalCommand> _commands;
        private readonly Queue<SignalCallbackObject> _signalObjs;

        private int _lockCount = 0;
        private bool IsLocked => _lockCount != 0;

        private Signal<TSignalType> _parent;
        private bool IsAttached => _parent != null;

        public static Signal<T> Create<T>() where T : Enum
        {
            Signal<T> t = new Signal<T>();
            return t;
        }

        private Signal()
        {
            _transDic0 = new Dictionary<TSignalType, List<ISignalCallback>>(32);
            _transDic1 = new Dictionary<TSignalType, List<ISignalCallback>>(32);
            _transDic2 = new Dictionary<TSignalType, List<ISignalCallback>>(32);
            _transDic3 = new Dictionary<TSignalType, List<ISignalCallback>>(32);
            _transDicObj = new Dictionary<TSignalType, List<ISignalCallback>>(32);
            _commands = new List<SignalCommand>(8);
            _signalObjs = new Queue<SignalCallbackObject>(8);
        }

        public void Dispose()
        {
            Clear();
        }

        // 挂接到另一个Signal，挂接后的事件会转发给起父Signal
        // 这样只要在根Signal上注册事件
        public void Attach(Signal<TSignalType> parent)
        {
            _parent = parent;
        }

        // 解除挂接
        public void Detach()
        {
            _parent = null;
        }

        public void Listen(TSignalType name, Action action)
        {
            SignalCallback @signal = new SignalCallback() { callback = action };
            Listen(name, @signal);
        }

        public void Listen<T>(TSignalType name, Action<T> action)
        {
            SignalCallback<T> @signal = new SignalCallback<T>() { callback = action };
            Listen(name, @signal);
        }

        public void Listen<T1, T2>(TSignalType name, Action<T1, T2> action)
        {
            SignalCallback<T1, T2> @signal = new SignalCallback<T1, T2>() { callback = action };
            Listen(name, @signal);
        }

        public void Listen<T1, T2, T3>(TSignalType name, Action<T1, T2, T3> action)
        {
            SignalCallback<T1, T2, T3> @signal = new SignalCallback<T1, T2, T3>() { callback = action };
            Listen(name, @signal);
        }

        public void ListenObj(TSignalType name, Action<object> action)
        {
            SignalCallbackObject @signal = new SignalCallbackObject() { callback = action };
            Listen(name, @signal);
        }

        public void UnListen(TSignalType name, Action action)
        {
            SignalCallback @signal = new SignalCallback() { callback = action };
            UnListen(name, @signal);
        }

        public void UnListen<T>(TSignalType name, Action<T> action)
        {
            SignalCallback<T> @signal = new SignalCallback<T>() { callback = action };
            UnListen(name, @signal);
        }

        public void UnListen<T1, T2>(TSignalType name, Action<T1, T2> action)
        {
            SignalCallback<T1, T2> @signal = new SignalCallback<T1, T2>() { callback = action };
            UnListen(name, @signal);
        }

        public void UnListen<T1, T2, T3>(TSignalType name, Action<T1, T2, T3> action)
        {
            SignalCallback<T1, T2, T3> @signal = new SignalCallback<T1, T2, T3>() { callback = action };
            UnListen(name, @signal);
        }

        public void UnListenObj(TSignalType name, Action<object> action)
        {
            SignalCallbackObject @signal = new SignalCallbackObject() { callback = action };
            UnListen(name, @signal);
        }

        public void Notify(TSignalType name)
        {
            if (IsAttached)
            {
                _parent.Notify(name);
            }

            if (_transDic0.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans0 = trans as SignalCallback;
                    trans0.callback?.Invoke();
                }
                Unlock();
            }
        }

        public void Notify<T>(TSignalType name, T arg)
        {
            if (IsAttached)
            {
                _parent.Notify(name, arg);
            }

            if (_transDic1.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans1 = trans as SignalCallback<T>;
                    trans1.callback?.Invoke(arg);
                }
                Unlock();
            }
        }

        public void Notify<T1, T2>(TSignalType name, T1 arg1, T2 arg2)
        {
            if (IsAttached)
            {
                _parent.Notify(name, arg1, arg2);
            }

            if (_transDic2.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans2 = trans as SignalCallback<T1, T2>;
                    trans2.callback?.Invoke(arg1, arg2);
                }
                Unlock();
            }
        }

        public void Notify<T1, T2, T3>(TSignalType name, T1 arg1, T2 arg2, T3 arg3)
        {
            if (IsAttached)
            {
                _parent.Notify(name, arg1, arg2, arg3);
            }

            if (_transDic3.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var trans3 = trans as SignalCallback<T1, T2, T3>;
                    trans3.callback?.Invoke(arg1, arg2, arg3);
                }
                Unlock();
            }
        }

        /// 延迟触发事件，会推迟到下一帧或者这帧末尾
        public void NotifyObj(TSignalType name, object arg)
        {
            if (IsAttached)
            {
                _parent.Notify(name, arg);
            }

            if (_transDicObj.TryGetValue(name, out var list))
            {
                Lock();
                foreach (var trans in list)
                {
                    var transObj = trans as SignalCallbackObject;
                    transObj.arg = arg;
                    _signalObjs.Enqueue(transObj);
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
            if (_signalObjs.Count == 0)
                return;

            while (_signalObjs.TryDequeue(out var signalObj))
            {
                signalObj.callback?.Invoke(signalObj.arg);
            }
        }

        #region Private Methods

        private void Listen(TSignalType name, ISignalCallback @signal)
        {
            if (!GetSignalList(name, @signal, out var transList, out var callbackLists))
            {
                callbackLists = new List<ISignalCallback>(16);
                transList.Add(name, callbackLists);
            }
            if (IsLocked)
            {
                AddCommand(true, name, @signal);
                return;
            }
            callbackLists!.Add(@signal);
        }

        private void UnListen(TSignalType name, ISignalCallback @signal)
        {
            if (!GetSignalList(name, @signal, out var transList, out var callbackLists))
                return;

            for (int i = callbackLists.Count - 1; i >= 0; i--)
            {
                if (Equals(callbackLists[i].CallbackHandler, @signal.CallbackHandler))
                {
                    if (IsLocked)
                    {
                        AddCommand(false, name, @signal);
                        return;
                    }
                    callbackLists.RemoveAt(i);
                    return;
                }
            }
        }

        private bool GetSignalList(TSignalType name,
                                   ISignalCallback @signal,
                                   out Dictionary<TSignalType, List<ISignalCallback>> transList,
                                   out List<ISignalCallback> callbackLists)
        {
            transList = default;
            if (@signal.TypeId == 0)
            {
                transList = _transDic0;
            }
            if (@signal.TypeId == 1)
            {
                transList = _transDic1;
            }
            if (@signal.TypeId == 2)
            {
                transList = _transDic2;
            }
            if (@signal.TypeId == 3)
            {
                transList = _transDic3;
            }
            if (@signal.TypeId == -1)
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

        private void AddCommand(bool isSubscribe, TSignalType name, ISignalCallback @signal)
        {
            _commands.Add(new SignalCommand { isSubscribe = isSubscribe, name = name, @signal = @signal });
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

        private void ExecuteCommand(in SignalCommand cmd)
        {
            if (cmd.isSubscribe)
            {
                Listen(cmd.name, cmd.@signal);
            }
            else
            {
                UnListen(cmd.name, cmd.@signal);
            }
        }

        #endregion Private Methods
    }

}